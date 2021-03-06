﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pres
{
    public class TransactionFunctions
    {
        /// <summary>
        /// Sends a remote transaction
        /// </summary>
        /// <param name="transaction"></param>
        public static void Send(RemoteTransaction transaction)
        {
            var factory = new ConnectionFactory();
            factory.Uri = "amqp://user10:User10ITS2016!@rabbit.binna.eu/";  //Insert your own user and password

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var transactionString = JsonConvert.SerializeObject(transaction);   //Awesome function

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                string pOrD = string.Empty;

                try
                {
                    var BicAsNumber = Convert.ToUInt32(transaction.Receiver.Bic);
                    if (BicAsNumber % 2 == 0)
                    {
                        pOrD = "production";
                    }
                    else
                    {
                        pOrD = "development";
                    }
                }
                catch (Exception)
                {
                    Debug.Print("Wrong Receiver Format");
                }

                channel.BasicPublish(exchange: "bank." + pOrD,  //Choose between bank.development and bank.production depending of the queue (e.g. 70 is production, 71 is development)
                                    routingKey: transaction.Receiver.Bic,   //This relates to the queue name of the receiver bank
                                    basicProperties: properties,    //Set the properties to persistent, otherwise the messages will get lost if the server restarts
                                    body: GetBytes(transactionString));

                //Interface Aufruf der Remote Transaction.
                TransactionWrapper.Intf_remoteTransfer(transaction.Sender.Iban, transaction.Sender.Bic, transaction.Receiver.Iban, transaction.Receiver.Bic, getToEuro(transaction), transaction.Currency, transaction.IsResponding);

                Console.WriteLine("[x] Message Sent");
            }
        }

        /// <summary>
        /// Receives a remote transaction
        /// </summary>
        public static void Receive()
        {
            var factory = new ConnectionFactory();
            factory.Uri = "amqp://user10:User10ITS2016!@rabbit.binna.eu/";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "10",
                                 durable: true, //Always use durable: true, because the queue on the server is configured this way. Otherwise you'll not be able to connect
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = GetString(ea.Body);
                        var transaction = JsonConvert.DeserializeObject<RemoteTransaction>(body);
                        try
                        {
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); //Important. When the message get's not acknowledged, it gets sent again

                            //acks
                            if (transaction.IsResponding && transaction.Errorcode != 0)
                            {
                                //transfer local bank to remote bank ack if it worked
                                if ((transaction.Sender.Bic != transaction.Receiver.Bic) && transaction.Amount >= 0)
                                {
                                    //local acc - -> +
                                    TransactionWrapper.Intf_deposit(Int32.Parse(TransactionWrapper.getAccIdFromIban(transaction.Sender.Iban)), getToEuro(transaction));
                                }
                                //local bank debits from remote bank ack if it worked
                                else if ((transaction.Sender.Bic != transaction.Receiver.Bic) && transaction.Amount < 0)
                                {
                                    //local acc + -> -
                                    TransactionWrapper.Intf_withdraw(Int32.Parse(TransactionWrapper.getAccIdFromIban(transaction.Sender.Iban)), getToEuro(transaction));
                                }
                                //transfer local bank to local bank ack if it worked
                                else if ((transaction.Sender.Bic == transaction.Receiver.Bic) && transaction.Amount >= 0)
                                {
                                    //local fromAcc - / local ToAcc + -> vice versa
                                    TransactionWrapper.Intf_deposit(Int32.Parse(TransactionWrapper.getAccIdFromIban(transaction.Sender.Iban)), getToEuro(transaction));
                                    TransactionWrapper.Intf_withdraw(Int32.Parse(TransactionWrapper.getAccIdFromIban(transaction.Receiver.Iban)), getToEuro(transaction));
                                }
                            }
                            //received message
                            else if (!transaction.IsResponding)
                            {
                                int errorCode = 0;
                                //transfer remote bank to us
                                if ((transaction.Sender.Bic != transaction.Receiver.Bic) && transaction.Amount >= 0)
                                {
                                    //local To Acc + / response
                                    errorCode = TransactionWrapper.Intf_deposit(Int32.Parse(TransactionWrapper.getAccIdFromIban(transaction.Receiver.Iban)),
                                        getToEuro(transaction));

                                    //response
                                    RemoteTransaction responseTransaction = new RemoteTransaction(transaction.Sender.Iban,
                                        transaction.Sender.Bic, transaction.Receiver.Iban, transaction.Receiver.Bic, transaction.Amount, transaction.Currency,
                                        transaction.Purpose, true, errorCode);
                                    Send(responseTransaction);
                                }
                                //remote debit from local account
                                else if ((transaction.Sender.Bic != transaction.Receiver.Bic) && transaction.Amount < 0)
                                {
                                    //local To Acc - / response
                                    errorCode = TransactionWrapper.Intf_withdraw(Int32.Parse(TransactionWrapper.getAccIdFromIban(transaction.Receiver.Iban)),
                                         getToEuro(transaction) * -1);

                                    //response
                                    RemoteTransaction responseTransaction = new RemoteTransaction(transaction.Sender.Iban,
                                        transaction.Sender.Bic, transaction.Receiver.Iban, transaction.Receiver.Bic, transaction.Amount, transaction.Currency,
                                        transaction.Purpose, true, errorCode);
                                    Send(responseTransaction);
                                }
                                //transfer from local bank to local bank
                                else if ((transaction.Sender.Bic == transaction.Receiver.Bic) && transaction.Amount >= 0)
                                {
                                    //no handling required at this point
                                }
                                //local debit from local account
                                else if ((transaction.Sender.Bic == transaction.Receiver.Bic) && transaction.Amount < 0)
                                {
                                    //no handling required at this point
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e);
                        }
                        //dont print acks
                        if (!transaction.IsResponding)
                        {
                            Console.WriteLine("[x] Received:");
                            PrintTransaction(transaction);
                        }
                    };
                    channel.BasicConsume(queue: "10",
                                     noAck: false,  //If noAck: false the command channel.BasicAck (see above) has to be implemented. Don't set it true, or the message will not get resubmitted, if the bank was offline
                                     consumer: consumer);
                    Thread.Sleep(1000);
                    channel.Dispose();
                }
                connection.Dispose();
            }
            factory = null;
            Thread thread = new Thread(TransactionFunctions.Receive);
            thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        static void PrintTransaction(RemoteTransaction transaction)
        {
            Console.WriteLine("Receiver Iban: " + transaction.Receiver.Iban);
            Console.WriteLine("Receiver Bic : " + transaction.Receiver.Bic);
            Console.WriteLine("Sender Iban  : " + transaction.Sender.Iban);
            Console.WriteLine("Sender Bic   : " + transaction.Sender.Bic);
            Console.WriteLine("Amount       : " + transaction.Amount);
            Console.WriteLine("Currency     : " + transaction.Currency.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmpTrans"></param>
        /// <returns></returns>
        static float getToEuro(RemoteTransaction tmpTrans)
        {
            float value = float.Parse(tmpTrans.Amount.ToString());

            switch (tmpTrans.Currency)
            {
                case ECurrency.Euro:
                    return value;
                case ECurrency.Dollar:
                    return value * 1.06f;
                case ECurrency.Pound:
                    return value * 0.85f;
                default:
                    return value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

    }
}
