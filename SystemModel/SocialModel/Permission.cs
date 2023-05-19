namespace KaliskaHaven.SocialModel;

/// <summary>
/// Permission type. To allow implementation with specification of each permission as string.
/// </summary>
public abstract class Permission : IEquatable<Permission>
{
	public abstract string PermissionKind {
		get;
	}
	public static bool operator ==(Permission left, Permission right) => left.PermissionKind == right.PermissionKind;
	public static bool operator !=(Permission left, Permission right) => left.PermissionKind != right.PermissionKind;

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(this, obj))
			return true;
		if (ReferenceEquals(obj, null))
			return false;
		if (obj is not Permission perm)
			return false;
		return this == perm;
	}

	public bool Equals(Permission? other) => this.Equals((object?)other);

	public override int GetHashCode() => PermissionKind.GetHashCode();
}
