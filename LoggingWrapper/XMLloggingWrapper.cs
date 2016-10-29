using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VE
{
    public class LoggingWrapper
    {

        #region ### WRAPP ###
        [DllImport("Logging.dll")]
        private static extern void logging_printVersion();

        [DllImport("Logging.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void logging_logError(IntPtr errorText, IntPtr fileName);
        #endregion

        #region ### INTERFACES ###
        //public static void Intf_printVersion()
        //{
        //    logging_printVersion();
        //}

        //public static void Intf_logError(string tmpErrorText, string tmpFileName)
        //{
        //    logging_logError(Marshal.StringToHGlobalAnsi(tmpErrorText), Marshal.StringToHGlobalAnsi(tmpFileName));
        //}
        #endregion
    }
}
