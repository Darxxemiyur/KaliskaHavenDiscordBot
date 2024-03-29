﻿using KaliskaHaven.Shop;

using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Glue.Shop;

public interface ICheck
{
	IAsyncEnumerable<IIdentifiable<ICartItem>> CheckItems {
		get;
	}

	Task AddToCheck(IIdentifiable<ICartItem> identifyable);

	Task RemoveFromCheck(IIdentifiable<ICartItem> identifyable);
}
