using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyExchangeWrapper;

namespace SST_LB02
{
    class MainApplication
    {
        static void Main(string[] args)
        {
            CurrencyExchangeWrapper.CurrencyExchangeWrapper.currencyExchange_printVersion();
            CurrencyExchangeWrapper.CurrencyExchangeWrapper.currencyExchange_info();
            double test = CurrencyExchangeWrapper.CurrencyExchangeWrapper.currencyExchange_exchange(45.12, 0, 1);
        }
    }
}
