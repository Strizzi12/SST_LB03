using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pres
{
	public class CurrencyExchangeWrapper
	{
        #region ### WRAPPER ###
        [DllImport("CurrencyExchange.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern double currencyExchange_exchange(double amount, int from, int to);

		[DllImport("CurrencyExchange.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void currencyExchange_info();
		#endregion

		#region ### INTERFACES ###
		/// <summary>
		/// Interface that calculates the new value of another currency.
		/// To select a currency, please insert the integer from below. <para />
		/// EUR = 0 <para />
		///	USD = 1 <para />
		///	GBP = 2 <para />
		///	INR = 3 <para />
		///	JPY = 4 <para />
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns>Returns the new calculated value of the given amount.</returns>
		public static double Intf_exchange(double amount, int from, int to)
        {
            return currencyExchange_exchange(amount, from, to);
        }

		/// <summary>
		/// Interface that prints the info, which currency which ID has.
		/// </summary>
		/// <returns>Returns the new calculated value of the given amount.</returns>
		public static void Intf_info()
        {
            currencyExchange_info();
        }

		/// <summary>
		/// This interface should edit a currency.
		/// Note that this interface throws a "Not Implemented Exception",
		/// since the imported .dll doesn´t support this function.
		/// </summary>
		/// <returns>Returns the new calculated value of the given amount.</returns>
		public static void Intf_changeCurrency(string from, string to, double rate)
		{
			throw new NotImplementedException("This interface is not implemented in the DLL");
		}

		/// <summary>
		/// This interface should add a new currency.
		/// Note that this interface throws a "Not Implemented Exception",
		/// since the imported .dll doesn´t support this function.
		/// </summary>
		/// <returns>Returns the new calculated value of the given amount.</returns>
		public static void Intf_addNewCurrency(string from, string to, double rate)
		{
			throw new NotImplementedException("This interface is not implemented in the DLL");
		}
        #endregion

    }
}