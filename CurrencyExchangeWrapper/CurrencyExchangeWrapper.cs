using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeWrapper
{
    public class CurrencyExchangeWrapper
    {
        [DllImport("CurrencyExchange.dll")]
        public static extern double currencyExchange_exchange(double amount, int from, int to);
        [DllImport("CurrencyExchange.dll")]
        public static extern void currencyExchange_info();
        [DllImport("CurrencyExchange.dll")]
        public static extern void currencyExchange_printVersion();
    }
}
