using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FS
{
    public class CurrencyExchangeWrapper
	{
		#region ### WRAPPER ###
		[DllImport("CurrencyCalculator.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int changeExchangeRate(IntPtr fromCurrency, IntPtr toCurrency, double exchangeRate);

		[DllImport("CurrencyCalculator.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern double calculateExchange(IntPtr fromCurrency, IntPtr toCurrency, double amount);

		[DllImport("CurrencyCalculator.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int addCurrency(IntPtr fromCurrency, IntPtr toCurrency, double exchangeRate);
		#endregion

		#region ### INTERFACES ###
		/// <summary>
		/// Interface that calculates the new value of another currency.
		/// Note that those exchange rates are hardcoded to simplicity of the task.
		/// To select a currency, please insert the integer from below.
		/// EUR = 0
		///	USD = 1
		///	GBP = 2
		///	INR = 3
		///	JPY = 4
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns>Returns the new calculated value of the given amount.</returns>
		public static double Intf_exchange(double amount, int from, int to)
		{
			double returnValue = calculateExchange(Helper.StoIPtr(
				(from == 0) ? "EUR" :
				(from == 1) ? "USD" :
				(from == 2) ? "GBP" :
				(from == 3) ? "INR" :
				(from == 4) ? "JPY" : "EUR"), Helper.StoIPtr(
				(to == 0) ? "EUR" :
				(to == 1) ? "USD" :
				(to == 2) ? "GBP" :
				(to == 3) ? "INR" :
				(to == 4) ? "JPY" : "EUR"), amount);

			if(returnValue < 0)
				addNotExistingStandardCurrencies();

			return calculateExchange(Helper.StoIPtr(
				(from == 0) ? "EUR" : 
				(from == 1) ? "USD" : 
				(from == 2) ? "GBP" :
				(from == 3) ? "INR" :
				(from == 4) ? "JPY" : "EUR"), Helper.StoIPtr(
				(to == 0) ? "EUR" :
				(to == 1) ? "USD" :
				(to == 2) ? "GBP" :
				(to == 3) ? "INR" :
				(to == 4) ? "JPY" : "EUR"), amount);
		}

		/// <summary>
		/// Interface that prints the info, which currency which ID has.
		/// </summary>
		/// <returns>Returns the new calculated value of the given amount.</returns>
		public static void Intf_info()
		{
			throw new NotImplementedException("This interface is not implemented in the DLL");
		}

		/// <summary>
		/// This interface should edit a currency.
		/// Note that this interface throws a "Not Implemented Exception",
		/// since the imported .dll doesn´t support this function.
		/// </summary>
		/// <returns>Returns the new calculated value of the given amount.</returns>
		public static void Intf_changeCurrency(string from, string to, double rate)
		{
			changeExchangeRate(Helper.StoIPtr(from), Helper.StoIPtr(to), rate);
		}

		/// <summary>
		/// This interface should add a new currency.
		/// Note that this interface throws a "Not Implemented Exception",
		/// since the imported .dll doesn´t support this function.
		/// </summary>
		/// <returns>Returns the new calculated value of the given amount.</returns>
		public static void Intf_addNewCurrency(string from, string to, double rate)
		{
			addCurrency(Helper.StoIPtr(from), Helper.StoIPtr(to), rate);
		}
		#endregion

		#region ### HELPER FUNCTIONS ###
		private static void addNotExistingStandardCurrencies()
		{
			//EUR exchange rates.
			Intf_addNewCurrency("EUR", "EUR", 1);
			Intf_addNewCurrency("EUR", "USD", 1.0994);
			Intf_addNewCurrency("EUR", "GBP", 0.9039);
			Intf_addNewCurrency("EUR", "INR", 73.4858);
			Intf_addNewCurrency("EUR", "JPY", 114.439);

			//USD exchange rates.
			Intf_addNewCurrency("USD", "EUR", 0.90974);
			Intf_addNewCurrency("USD", "USD", 1);
			Intf_addNewCurrency("USD", "GBP", 0.82211);
			Intf_addNewCurrency("USD", "INR", 66.8391);
			Intf_addNewCurrency("USD", "JPY", 104.113);

			//GBP exchange rates.
			Intf_addNewCurrency("GBP", "EUR", 1.10738);
			Intf_addNewCurrency("GBP", "USD", 1.21701);
			Intf_addNewCurrency("GBP", "GBP", 1);
			Intf_addNewCurrency("GBP", "INR", 81.3616);
			Intf_addNewCurrency("GBP", "JPY", 126.708);

			//INR exchange rates.
			Intf_addNewCurrency("INR", "EUR", 0.01361);
			Intf_addNewCurrency("INR", "USD", 0.01496);
			Intf_addNewCurrency("INR", "GBP", 0.01229);
			Intf_addNewCurrency("INR", "INR", 1);
			Intf_addNewCurrency("INR", "JPY", 1.55739);

			//JPY exchange rates.
			Intf_addNewCurrency("JPY", "EUR", 0.00874);
			Intf_addNewCurrency("JPY", "USD", 0.00961);
			Intf_addNewCurrency("JPY", "GBP", 0.00789);
			Intf_addNewCurrency("JPY", "INR", 0.64203);
			Intf_addNewCurrency("JPY", "JPY", 1);
		}
		#endregion
	}
}
