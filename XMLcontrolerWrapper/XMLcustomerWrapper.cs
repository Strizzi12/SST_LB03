using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VE
{
    /// <summary>
    /// manages functionalities of customers
    /// </summary>
    public class CustomerWrapper
    {
        #region ### WRAPPER ###
        [DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int xmlcontroler_createCustomer(IntPtr firstName, IntPtr lastname, IntPtr plzOrt, IntPtr strasse, int hausNr);

        [DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int xmlcontroler_updateCustomer(int cusID, IntPtr firstName, IntPtr lastname, IntPtr plzOrt, IntPtr strasse, int hausNr);

        [DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern bool xmlcontroler_deleteCustomerByID(int ID);
        #endregion

        #region ### INTERFACES ###
        /// <summary>
        /// Interface creates a customer with the given parameters
        /// Note that the parameter birthDate will not be used in this interface
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="street"></param>
        /// <param name="postCodePlace"></param>
        /// <param name="nr"></param>
        /// <param name="birthDate"></param>
        /// <returns>Returns the ID of the customer object or an error code</returns>
        public static int Intf_createCustomer(string firstName, string lastName, string street, string postCodePlace, int nr, string birthDate)
        {
            //parameter birthDate not used for this function because its not needed/cannot be stored but required for other dlls compatibility

            return xmlcontroler_createCustomer(Helper.StoIPtr(firstName), Helper.StoIPtr(lastName), Helper.StoIPtr(postCodePlace), Helper.StoIPtr(street), nr);
        }

        /// <summary>
        /// Updates customer with the given parameters
        /// Note that the parameter gebDate will not be used in this interface
        /// Loss of functionality for compatibility reasons: passing string params as "" or int values as 0 to only insert values to update
        /// Functionality cannot be implemented in C# interface of 2nd dll because required dll-function returns memory address which gets freed
        /// </summary>
        /// <param name="cusID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="plzOrt"></param>
        /// <param name="street"></param>
        /// <param name="hausNr"></param>
        /// <param name="gebDate"></param>
        /// <returns>Returns the value 0 if update was successfull  or -1 if an error occured</returns>
        public static int Intf_updateCustomer(int cusID, string firstName, string lastName, string plzOrt, string street, int hausNr, string gebDate)
        {
            //parameter gebDate not used for this function because its not needed/cannot be stored but required for other dlls compatibility

            int returnVal = xmlcontroler_updateCustomer(cusID, Helper.StoIPtr(firstName), Helper.StoIPtr(lastName), Helper.StoIPtr(plzOrt), Helper.StoIPtr(street), hausNr);

            //required to have same return behaviour for different dlls
            if (returnVal < 0)
            {
                returnVal = -1;
            }
            else
            {
                returnVal = 0;
            }

            return returnVal;
        }

        /// <summary>
        /// Deletes customer with the given ID
        /// </summary>
        /// <param name="cusID"></param>
        /// <returns>Returns the value 0 if deletion was successful or -1 if the deletion failed</returns>
        public static int Intf_deleteCustomer(int cusID)
        {
            bool returnValBool = xmlcontroler_deleteCustomerByID(cusID);
            int returnValInt = 0;

            //mapping required to be compatible with the return value of other dlls
            if (returnValBool)
            {
                returnValInt = 0;
            }
            else
            {
                returnValInt = -1;
            }

            return returnValInt;
        }


        #endregion
    }
}
