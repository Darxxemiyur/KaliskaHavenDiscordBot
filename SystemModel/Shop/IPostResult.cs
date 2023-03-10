using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Shop
{
	public interface IPostResult
	{
		Task<bool> TryApplyOnCustomer(ICustomer customer);
		Task Revert(ICustomer customer);
	}
}
