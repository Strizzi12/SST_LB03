﻿using System;
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
		[DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool xmlcontroler_transferMoney(int tmpFromAccID, int tmpToAccID, float tmpValue);

		[DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool xmlcontroler_depositMoney(int tmpAccID, float tmpValue);

		[DllImport("XMLControler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool xmlcontroler_withdrawMoney(int tmpAccID, float tmpValue);
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
		/// <returns>Returns 0 if the transfer is successfully.</returns>
		public static int Intf_transfer(int custId, int tmpFromAccID, int tmpToAccID, float tmpValue, int currency)
		{
			//Note that this DLL doesn´t need a customerID to create a transaction.
			if(currency == 0)
				return xmlcontroler_transferMoney(tmpFromAccID, tmpToAccID, tmpValue) ? 0 : -1;
			else
				return xmlcontroler_transferMoney(tmpFromAccID, tmpToAccID, Convert.ToSingle(CurrencyExchangeWrapper.Intf_exchange(tmpValue, currency, 0))) ? 0 : -1;
		}

		/// <summary>
		/// Interface that withdraws the given value from the given account.
		/// It also accepts values from another currencies.
		/// To select a currency, please insert the integer from below.
		/// </summary>
		/// <param name="tmpAccID"></param>
		/// <param name="tmpValue"></param>
		/// <returns>Returns 0 if the withdraw is successfully.</returns>
		public static int Intf_withdraw(int tmpAccID, float tmpValue)
		{
			return xmlcontroler_withdrawMoney(tmpAccID, tmpValue) ? 0 : -1;
		}

		/// <summary>
		/// Interface that deposits the given value to the given account.
		/// It also accepts values from another currencies.
		/// To select a currency, please insert the integer from below.
		/// </summary>
		/// <param name="tmpAccID"></param>
		/// <param name="tmpValue"></param>
		/// <returns>Returns 0 if the withdraw is successfully.</returns>
		public static int Intf_deposit(int tmpAccID, float tmpValue)
		{
			return xmlcontroler_depositMoney(tmpAccID, tmpValue) ? 0 : -1;
		}
		#endregion
	}
}
