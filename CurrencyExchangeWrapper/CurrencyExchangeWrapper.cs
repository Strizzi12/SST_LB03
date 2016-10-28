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
		[DllImport("CurrencyExchange.dll", CharSet = CharSet.Ansi)]
		private static extern double currencyExchange_exchange(double amount, int from, int to);
		[DllImport("CurrencyExchange.dll", CharSet = CharSet.Ansi)]
		private static extern void currencyExchange_info();
		[DllImport("CurrencyExchange.dll", CharSet = CharSet.Ansi)]
		private static extern void currencyExchange_printVersion();

		public static double exchange(double amount, int from, int to)
		{
			return currencyExchange_exchange(amount, from, to);
		}

		public static void info()
		{
			currencyExchange_info();
		}

		public static void printVersion()
		{
			currencyExchange_printVersion();
		}
	}
}