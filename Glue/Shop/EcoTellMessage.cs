using Name.Bayfaderix.Darxxemiyur.General;

namespace KaliskaHaven.Glue.Shop;

public record EcoTellMessage(EcoTellMsgEnum Type) : ITellMessage
{
	public string? Note {
		get;
	}
	public object? Message => Type;
	public static implicit operator TellMessage(EcoTellMessage msg) => new(msg);
}

/// <summary>
/// Messages
/// </summary>
public enum EcoTellMsgEnum
{
	GetWallet,
}
