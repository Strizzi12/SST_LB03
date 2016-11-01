using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SST_LB02
{
    class MainApplication
    {
        static void Main(string[] args)
        {
            //clear existing data of directory
            VE.DataMaintenance.Intf_clearData();
            FS.DataMaintenance.Intf_clearData();

            int VEcusID, FScusID = 0;
            // Customer functions
            VEcusID = VE.CustomerWrapper.Intf_createCustomer("Mike", "Thomas", "Breitenfelderstrasse", "5020 Salzburg", 47, "13.11.1992");
            FScusID = FS.CustomerWrapper.Intf_createCustomer("Mike", "Thomas", "Breitenfelderstrasse", "5020 Salzburg", 47, "13.11.1992");

            VE.CustomerWrapper.Intf_updateCustomer(VEcusID, "Mike", "Anders", "AnotherPlace", "AnotherStreet", 1111, "11.11.1111");
            FS.CustomerWrapper.Intf_updateCustomer(FScusID, "Mike", "Anders", "AnotherPlace", "AnotherStreet", 1111, "11.11.1111");

            VE.CustomerWrapper.Intf_deleteCustomer(VEcusID);
            FS.CustomerWrapper.Intf_deleteCustomer(FScusID);

            VEcusID = VE.CustomerWrapper.Intf_createCustomer("Betty", "Katzian", "Breitenfelderstrasse", "5020 Salzburg", 47, "13.11.1992");
            FScusID = FS.CustomerWrapper.Intf_createCustomer("Betty", "Katzian", "Breitenfelderstrasse", "5020 Salzburg", 47, "13.11.1992");
            //--------------------------------------------------------------------------
            // Account functions
            int VEaccID, FSaccID = 0;
            VEaccID = VE.AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
            FSaccID = FS.AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");

            //Account edit
            VE.AccountWrapper.Intf_editAccount(VEaccID, 1);
            try
            {
                //interface throws notimplex bc 2nd party dll does not include this functionality
                FS.AccountWrapper.Intf_editAccount(FSaccID, 1);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            //Account deletion
            VE.AccountWrapper.Intf_deleteAccount(VEaccID);
            try
            {
                //interface throws notimplex bc 2nd party dll does not include this functionality
                FS.AccountWrapper.Intf_deleteAccount(FSaccID);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            ////create bankstatement for account
            int VEaccID2, FSaccID2, VEaccID3, FSaccID3 = 0;
            VEaccID2 = VE.AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
            FSaccID2 = FS.AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
			VEaccID3 = VE.AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");
			FSaccID3 = FS.AccountWrapper.Intf_createAccount(0, 1000, "TestNameAcc");

			//Attach customer to account
			FS.AccountWrapper.Intf_attachAccount(FSaccID2, FScusID);

			//Creating transactions
			VE.TransactionWrapper.Intf_transfer(VEcusID, VEaccID2, VEaccID3, 100, 0);
			FS.TransactionWrapper.Intf_transfer(FScusID, FSaccID2, FSaccID3, 100, 0);

			VE.TransactionWrapper.Intf_deposit(VEaccID2, 500);
			FS.TransactionWrapper.Intf_deposit(FSaccID2, 500);

			VE.TransactionWrapper.Intf_withdraw(VEaccID2, 500);
			FS.TransactionWrapper.Intf_withdraw(FSaccID2, 500);

            VE.AccountWrapper.Intf_createBankStatement(VEaccID2);
            FS.AccountWrapper.Intf_createBankStatement(FSaccID2);

			FS.TransactionWrapper.Intf_transfer(FScusID, FSaccID2, FSaccID3, 100, 0);
			FS.TransactionWrapper.Intf_transfer(FScusID, FSaccID2, FSaccID3, 100, 0);

			FS.AccountWrapper.Intf_createBankStatement(FSaccID2);
        }
    }
}
