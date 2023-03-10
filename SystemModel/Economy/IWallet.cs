﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Economy
{
	public interface IWallet
	{
		/// <summary>
		/// Deposit currency to wallet. Always returns the total ammount.
		/// </summary>
		/// <param name="currency"></param>
		/// <returns></returns>
		public Task<TransactionLog> Deposit(Currency currency);
		/// <summary>
		/// Get specific Wallet's currency.
		/// </summary>
		/// <param name="currency">Type of currency to get</param>
		/// <returns>The currency of the type if any</returns>
		public Task<Currency?> Get(CurrencyType currency);
		/// <summary>
		/// Get Wallet's currencies.
		/// </summary>
		/// <returns></returns>
		public IAsyncEnumerable<Currency> GetAll();
		/// <summary>
		/// </summary>
		/// <param name="currency"></param>
		/// <returns></returns>
		public Task<TransactionLog> Withdraw(Currency currency);
	}
}
