using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pres;

namespace SST_LB02
{
    class MainApplication
    {
        static void Main(string[] args)
        {
            //clear existing data of directory
            DataMaintenance.Intf_clearData();
            //--------------------------------------------------------------------------
            int VEcusID, FScusID = 0;
            // Customer functions
            VEcusID = CustomerWrapper.Intf_createCustomer("Mike", "Thomas", "Breitenfelderstrasse", "5020 Salzburg", 47, "13.11.1992");

            CustomerWrapper.Intf_updateCustomer(VEcusID, "Mike", "Anders", "AnotherPlace", "AnotherStreet", 1111, "11.11.1111");

            CustomerWrapper.Intf_deleteCustomer(VEcusID);

            VEcusID = CustomerWrapper.Intf_createCustomer("Betty", "Katzian", "Breitenfelderstrasse", "5020 Salzburg", 47, "13.11.1992");
            //--------------------------------------------------------------------------
            // Account functions
            int VEaccID, FSaccID = 0;
            VEaccID = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");

            //Account edit
            try
            {
                //interface throws notimplex bc 2nd party dll does not include this functionality
                AccountWrapper.Intf_editAccount(FSaccID, 1);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            //Account deletion
            try
            {
                //interface throws notimplex bc 2nd party dll does not include this functionality
                AccountWrapper.Intf_deleteAccount(FSaccID);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            ////create transactions for the accounts
            int VEaccID2, FSaccID2, VEaccID3, FSaccID3 = 0;
            VEaccID2 = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
            FSaccID2 = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
			VEaccID3 = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
			FSaccID3 = AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");

			//Attach/Dettach customer to account
			AccountWrapper.Intf_attachAccount(FSaccID2, FScusID);

            AccountWrapper.Intf_dettachAccount(FSaccID2, FScusID);

            AccountWrapper.Intf_attachAccount(FSaccID2, FScusID);

            //Creating transactions
            TransactionWrapper.Intf_transfer(FScusID, FSaccID2, FSaccID3, 100, 0);

			TransactionWrapper.Intf_deposit(FSaccID2, 500);

			TransactionWrapper.Intf_withdraw(FSaccID2, 500);

            //create a bankstatement
            AccountWrapper.Intf_createBankStatement(FSaccID2);

			TransactionWrapper.Intf_transfer(FScusID, FSaccID2, FSaccID3, 100, 0);

            //create a bankstatement
            AccountWrapper.Intf_createBankStatement(FSaccID2);
        }
    }
}
