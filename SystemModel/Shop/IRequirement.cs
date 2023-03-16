namespace KaliskaHaven.Shop;

public interface IRequirement
{
	string RequirementType {
		get;
	}

	/// <summary>
	/// List of types the ICustomer is required to have access to.
	/// </summary>
	IEnumerable<Type> RequiredTypes {
		get;
	}

	/// <summary>
	/// Accept customer visit.
	/// </summary>
	/// <param name="customer"></param>
	/// <returns>True if customer meets requirements. False otherwise.</returns>
	Task<bool> CustomerVisit(ICustomer customer);
}
