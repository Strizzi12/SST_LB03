using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pres
{
    /// <summary>
    /// Logging can be used without any other components therefor the exported functions of the
    /// dll got wrapped, 2nd party dll does not support this functionality
    /// </summary>
    public class LoggingWrapper
    {
        
        #region ### WRAPP ###
        [DllImport("Logging.dll")]
        private static extern void logging_printVersion();

        [DllImport("Logging.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void logging_logError(IntPtr errorText, IntPtr fileName);
        #endregion

        #region ### INTERFACES ###
        /// <summary>
        /// Testfunctionality 
        /// Prints the filename, compiletime of dll and the version of the dll
        /// </summary>
        public static void Intf_printVersion()
        {
            logging_printVersion();
        }

        /// <summary>
        /// Logs the given errotext and filename into a textfile called LogFile.txt which is created in the directory of the exe
        /// </summary>
        /// <param name="tmpErrorText"></param>
        /// <param name="tmpFileName"></param>
        public static void Intf_logError(string tmpErrorText, string tmpFileName)
        {
            logging_logError(Marshal.StringToHGlobalAnsi(tmpErrorText), Marshal.StringToHGlobalAnsi(tmpFileName));
        }
        #endregion
    }
}
