using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VE
{
    public class AccountWrapper
    {
        #region ### WRAPPER ###
        [DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int xmlcontroler_createAccount(int tmpType, float tmpValue);

        [DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int xmlcontroler_manageAccount(int tmpAccID, int tmpType);

        [DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int xmlcontroler_closeAccount(int tmpAccID);

        [DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern bool xmlcontroler_getBankStatement(int tmpAccID);
        #endregion

        #region ### INTERFACES ###
        /// <summary>
        /// Interface creates an account with the given parameters
        /// </summary>
        /// <param name="typ"></param>
        /// <param name="money"></param>
        /// <param name="name"></param>
        /// <returns>Returns the ID of the created account or an error code</returns>
        public static int Intf_createAccount(int typ, double money, string name)
        {
            //parameter name not used for this function because its not needed/cannot be stored but required for other dlls compatibility

            return xmlcontroler_createAccount(typ, (float)money);
        }

        /// <summary>
        /// Edits the account type of according account by it´s acc ID
        /// </summary>
        /// <param name="tmpAccID"></param>
        /// <param name="tmpNewType"></param>
        /// <returns>Returns modified account ID if operation was successful or an error code if it failed</returns>
        public static int Intf_editAccount(int tmpAccID, int tmpNewType)
        {
            return xmlcontroler_manageAccount(tmpAccID, tmpNewType);
        }

        /// <summary>
        /// Deletes the according account by it´s ID, incl all the references of it in customers
        /// </summary>
        /// <param name="tmpAccID"></param>
        /// <returns>Returns the closed account´s ID if the deletion was successful or an error code if the deletion failed</returns>
        public static int Intf_deleteAccount(int tmpAccID)
        {
            return xmlcontroler_closeAccount(tmpAccID);
        }

        /// <summary>
        /// Creates a bankstatement of the given account in the directory of the exe
        /// </summary>
        /// <param name="tmpAccID"></param>
        /// <returns>>Returns 0 if the creation of the bankstatement was successfull or an error code if the creation failed</returns>
        public static int Intf_createBankStatement(int tmpAccID)
        {
            bool boolRetrun = xmlcontroler_getBankStatement(tmpAccID);

            //converts the bool return value to int for compatibility to 2nd party dll
            int intReturn = 0;
            if (boolRetrun)
            {
                return intReturn;
            }
            else
            {
                intReturn = -1;
                return intReturn;
            }
        }



        #endregion
    }
}
