using KaliskaHaven.Economy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Shop.External
{
	public class EconomyRequirement : IRequirement
	{
		public Currency Price {
			get;
		}
		public string RequirementType => nameof(EconomyRequirement);

		public EconomyRequirement(Currency price)
		{
			Price = price;
		}

		public Task<bool> AcceptCustomer(ICustomer customer) => throw new NotImplementedException();
	}
}
