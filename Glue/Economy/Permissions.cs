using KaliskaHaven.SocialModel;

namespace KaliskaHaven.Glue.Economy;

public sealed class EconomyPermission
{
	public sealed class Deposit : Permission
	{
		public override string PermissionKind => $"{nameof(EconomyPermission)}.{nameof(Deposit)}_Permission";
	}

	public sealed class Withdraw : Permission
	{
		public override string PermissionKind => $"{nameof(EconomyPermission)}.{nameof(Withdraw)}_Permission";
	}

	public sealed class Transfer : Permission
	{
		public override string PermissionKind => $"{nameof(EconomyPermission)}.{nameof(Transfer)}_Permission";
	}

	public sealed class Convert : Permission
	{
		public override string PermissionKind => $"{nameof(EconomyPermission)}.{nameof(Convert)}_Permission";
	}
}
