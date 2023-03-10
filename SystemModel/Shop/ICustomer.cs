using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliskaHaven.Shop
{
	public interface ICustomer
	{
		Task AcceptPostResult(IPostResult postResult, ICartItem details);
		Task RevertPostResult(IPostResult postResult, ICartItem details);
		/// <summary>
		/// Accept requirement for judging.
		/// </summary>
		/// <param name="requirement">Requirement</param>
		/// <returns>True if requirement is satisfied. False otherwise.</returns>
		Task<bool> AcceptRequirement(IRequirement requirement, ICartItem details);
	}
}
