using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FS
{
    /// <summary>
    /// manages functionalities of customers
    /// </summary>
    public class CustomerWrapper
    {
        #region ### WRAPPER ###

        [DllImport("Customer.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int createCustomer(IntPtr vn, IntPtr nn, IntPtr adr, IntPtr plz, IntPtr gebdatum);

        [DllImport("Customer.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int updateCustomer(int cid, IntPtr vn, IntPtr nn, IntPtr adr, IntPtr plz, IntPtr gebdatum);

        [DllImport("Customer.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int deactivateCustomer(int cid);

        [DllImport("Customer.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private static unsafe extern int getCustomer(int cid, IntPtr destDddress, int length);
        #endregion

        #region ### INTERFACES ###
        /// <summary>
        /// Interface creates a customer with the given parameters
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="street"></param>
        /// <param name="postCodePlace"></param>
        /// <param name="nr"></param>
        /// <param name="birthDate"></param>
        /// <returns>Returns the ID of the created customer or an error code</returns>
        public static int Intf_createCustomer(string firstName, string lastName, string street, string postCodePlace, int nr, string birthDate)
        {
            return createCustomer(Helper.StoIPtr(firstName), Helper.StoIPtr(lastName), Helper.StoIPtr(street + " " + nr.ToString()), Helper.StoIPtr(postCodePlace), Helper.StoIPtr(birthDate));
        }

        /// <summary>
        /// Updates customer with the given parameters
        /// </summary>
        /// <param name="cusID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="plzOrt"></param>
        /// <param name="street"></param>
        /// <param name="hausNr"></param>
        /// <param name="gebDate"></param>
        /// <returns>Returns the value 0 if update was successfull  or -1 if an error occured</returns>
        public static unsafe int Intf_updateCustomer(int cusID, string firstName, string lastName, string plzOrt, string street, int hausNr, string gebDate)
        {
            return updateCustomer(cusID, Helper.StoIPtr(firstName), Helper.StoIPtr(lastName), Helper.StoIPtr(street + " " + hausNr), Helper.StoIPtr(plzOrt), Helper.StoIPtr(gebDate));
        }

        /// <summary>
        /// Deletes customer with the given ID
        /// </summary>
        /// <param name="cusID"></param>
        /// <returns>Returns the value 0 if deletion was successful or -1 if the deletion failed</returns>
        public static int Intf_deleteCustomer(int cusID)
        {
            return deactivateCustomer(cusID);
        }

        #endregion
    }
}
