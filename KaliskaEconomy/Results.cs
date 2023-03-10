using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Economy
{
	public sealed class WithdrawResult
	{
		/// <summary>
		/// Whether the operation was successful.
		/// </summary>
		public bool IsSuccesful {
			get; set;
		}
		/// <summary>
		/// Check for WithdrawResult.IsSuccessful.
		/// </summary>
		public Currency? Withdrawn {
			get; set;
		}
		public WithdrawResult()
		{
			IsSuccesful = false;
			Withdrawn = null;
		}
		public WithdrawResult(Currency amount)
		{
			IsSuccesful = true;
			Withdrawn = amount;
		}
	}
}
