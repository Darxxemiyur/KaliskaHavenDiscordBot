using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Shop
{
	public interface ICustomer
	{
		Task AcceptPostResult(IPostResult postResult);
		Task RevertPostResult(IPostResult postResult);
		Task AcceptPreRequestiment(IPreRequestiment postResult);
	}
}
