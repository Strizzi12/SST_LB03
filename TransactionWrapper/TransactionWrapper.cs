using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pres
{
    public class TransactionWrapper
	{
		#region ### WRAPPER ###
		[DllImport("BankDealings.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int createTransaction(int cid, int src_accid, int dest_accid, IntPtr zweck, double amount);

		[DllImport("Account.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int deposit(int accid, double amount);

		[DllImport("Account.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern int withdraw(int accid, double amount);
		#endregion

		#region ### INTERFACES ###
		/// <summary>
		/// Interface that transfers a given value from one acount to another account.
		/// It also accepts values from another currencies.
		/// To select a currency, please insert the integer from below.
		/// EUR = 0
		///	USD = 1
		///	GBP = 2
		///	INR = 3
		///	JPY = 4
		/// </summary>
		/// <param name="tmpFromAccID"></param>
		/// <param name="tmpToAccID"></param>
		/// <param name="tmpValue"></param>
		/// <param name="currency"></param>
		/// <returns>Returns true if the transfer is successfully.</returns>
		public static int Intf_transfer(int custId, int tmpFromAccID, int tmpToAccID, float tmpValue, int currency)
		{
			if (currency == 0)
				return createTransaction(custId, tmpFromAccID, tmpToAccID, Helper.StoIPtr("transfer"), tmpValue);
			else
				return createTransaction(custId, tmpFromAccID, tmpToAccID, Helper.StoIPtr("transfer"), Convert.ToSingle(CurrencyExchangeWrapper.Intf_exchange(tmpValue, currency, 0)));
		}

		/// <summary>
		/// Interface that withdraws the given value from the given account.
		/// It also accepts values from another currencies.
		/// To select a currency, please insert the integer from below.
		/// </summary>
		/// <param name="tmpAccID"></param>
		/// <param name="tmpValue"></param>
		/// <returns>Returns true if the withdraw is successfully.</returns>
		public static int Intf_withdraw(int tmpAccID, float tmpValue)
		{
			return withdraw(tmpAccID, tmpValue);
		}

		/// <summary>
		/// Interface that deposits the given value to the given account.
		/// It also accepts values from another currencies.
		/// To select a currency, please insert the integer from below.
		/// </summary>
		/// <param name="tmpAccID"></param>
		/// <param name="tmpValue"></param>
		/// <returns>Returns true if the withdraw is successfully.</returns>
		public static int Intf_deposit(int tmpAccID, float tmpValue)
		{
			return deposit(tmpAccID, tmpValue);
		}

		#endregion
	}
}
