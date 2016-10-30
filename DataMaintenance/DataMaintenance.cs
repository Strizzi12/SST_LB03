using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS
{
    public class DataMaintenance
    {
        #region ### COSTANT FILENAMES ###
        private const string DBname = "test.sqlite";
        private const string LogFilename = "LogFile.txt";
        private const string BankStatementname = "BankStatement AccountNR";
        private const string XMLFilename = "MyXMLFile.xml";
        #endregion

        #region ### WRAPPER ###
        //no import of dll functions required - data handling is done by the component
        #endregion

        #region ### INTERFACES ###
        /// <summary>
        /// Clears the data made by the component
        /// Note that this function expands the functionality of the dlls with a data reset option for the user
        /// </summary>
        public static void Intf_clearData()
        {
            if (File.Exists(DBname))
            {
                File.Delete(DBname);
            }

            if (File.Exists(LogFilename))
            {
                File.Delete(LogFilename);
            }

            if (File.Exists(BankStatementname))
            {
                File.Delete(BankStatementname);
            }

            if (File.Exists(XMLFilename))
            {
                File.Delete(XMLFilename);
            }
        }
        #endregion
    }
}
