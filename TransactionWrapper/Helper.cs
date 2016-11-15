using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pres
{
    class Helper
    {
        /// <summary>
        /// Returns given string as IntPtr
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static IntPtr StoIPtr(string tmp)
        {
            return Marshal.StringToHGlobalAnsi(tmp);
        }
    }
}
