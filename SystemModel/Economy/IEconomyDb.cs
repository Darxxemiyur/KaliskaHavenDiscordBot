using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Economy
{
	public interface IEconomyDb
	{
		public Task UpdateWallet(IWallet wallet);
		public Task LogTransaction(TransactionLog log);
	}
}
