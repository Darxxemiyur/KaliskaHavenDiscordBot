using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Shop
{
	public interface IPreRequestiment
	{
		Task<bool> DoesSatisfy(ICustomer customer);
	}
}
