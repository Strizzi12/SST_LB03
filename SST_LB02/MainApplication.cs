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
			CurrencyExchangeWrapper.CurrencyExchangeWrapper.printVersion();
			CurrencyExchangeWrapper.CurrencyExchangeWrapper.info();
			double test = CurrencyExchangeWrapper.CurrencyExchangeWrapper.exchange(45.12, 0, 1);
			Console.WriteLine("Exchange = " + test);
		}
	}
}
