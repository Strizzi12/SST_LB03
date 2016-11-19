using System;
using System.Diagnostics;

namespace Pres
{
    public class RemoteTransaction
    {
        public Guid TransactionId { get; set; }
        public RemoteCustomer Sender { get; set; }
        public RemoteCustomer Receiver { get; set; }
        public Int32 Timestamp { get; set; }        // Unix format timestamp
        public int Errorcode { get; set; }         // 0 = no Error
        public string Purpose { get; set; }
        public double Amount { get; set; }          // if positive: transaction, negative: debiting
        public ECurrency Currency { get; set; }
        public bool IsResponding { get; set; }      // if Transaction is a Response

        /// <summary>
        /// 
        /// </summary>
        /// <param name="senderAccID"></param>
        /// <param name="senderBic"></param>
        /// <param name="receiverAccID"></param>
        /// <param name="receiverBic"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="purpose"></param>
        /// <param name="isResponding"></param>
        /// <param name="errorcode"></param>
        public RemoteTransaction(string senderAccID, string senderBic, string receiverAccID, string receiverBic, double amount, ECurrency currency, string purpose = "", bool isResponding = false, int errorcode = 0)
        {
            this.TransactionId = Guid.NewGuid();
            Sender = new RemoteCustomer(GenerateIbanFromAccID(senderAccID), senderBic);
            this.Amount = amount;
            Receiver = new RemoteCustomer(GenerateIbanFromAccID(receiverAccID), receiverBic);
            Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            this.Currency = currency;
            this.IsResponding = isResponding;
            this.Errorcode = errorcode;
            this.Purpose = purpose;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetStatus()
        {
            switch (this.Errorcode)
            {
                case 0:
                    return "Success";
                case -1:
                    return "Debiting not authorized";       // to debit = abbuchen :)
                case -2:
                    return "IBAN not found";
                case -3:
                    return "Internal DLL error";
                case -4:
                    return "Backflip";
                case -5:
                    return "Json-Format invalid / Data could not be converted";
                case -6:
                    return "Transaction rejected";
                case -7:
                    return "no Money on Account";
                default:
                    return "Error not spezified";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kontonummer"></param>
        /// <returns></returns>
        public static string GenerateIbanFromAccID(string kontonummer)
        {
            long pz = 0L;
            long dr = 0L;
            int kontoNrLength = kontonummer.Length;
            const int kontoNrTotalLength = 10;
            int fillUpCnt = kontoNrTotalLength - kontoNrLength;

            try
            {
                long tmp = Convert.ToInt64(kontonummer);
                dr = (1 + (tmp - 1) % 9);
                pz = 7 - dr % 7;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return string.Empty;
            }
            string returnVal = string.Empty;

            //build msg string
            returnVal += pz.ToString();
            for (int i = 0; i < fillUpCnt; i++)
            {
                returnVal += "0";
            }
            returnVal += kontonummer;

            return returnVal;
        }

		public static bool CheckPruefziffer(string iban)
		{
			long pz = 0L;
			long dr = 0L;
			string pruefzifferFromIban = iban[0].ToString();
			string rest = iban.Substring(1, iban.Length);
			try
			{
				long tmp = Convert.ToInt64(rest);
				dr = (1 + (tmp - 1) % 9);
				pz = 7 - dr % 7;
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message);
				return false;
			}
			string pruefzahlToCheack = pz.ToString();
			if (pruefzahlToCheack.Equals(pruefzifferFromIban))
				return true;
			else
				return false;
		}

	}

    /// <summary>
    /// 
    /// </summary>
    public class RemoteCustomer
    {
        public string Iban { get; set; }
        public string Bic { get; set; }

        public RemoteCustomer(string iban, string bic)
        {
            this.Iban = iban;
            this.Bic = bic;
        }
    }

    /*
     * umrechnung:
     * EURO 1
     * DOLLAR 1,1070
     * POUND 0,8889
    */
    public enum ECurrency { Euro, Dollar, Pound }
}