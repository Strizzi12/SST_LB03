using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SST_LB02
{
    class MainApplication
    {
        static void Main(string[] args)
        {
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
            VEaccID = VE.AccountWrapper.Intf_createAccount(1, 1000, "TestNameAcc");
            FSaccID = FS.AccountWrapper.Intf_createAccount(1, 1000, "TestNameAcc");

            VE.AccountWrapper.Intf_editAccount(VEaccID, 0);
            try
            {
                //interface throws notimplex bc 2nd party dll does not include this functionality
                FS.AccountWrapper.Intf_editAccount(FSaccID, 0);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            VE.AccountWrapper.Intf_deleteAccount(VEaccID);




        }
    }
}
