namespace KaliskaHaven.DiscordClient.Interaction;

public enum CommandArgumentType
{
	@String,
	Number,
	User,
	Channel,
	Role,
	Integer,
}

public sealed class CommandArgument
{
	public CommandArgumentType Kind {
		get; set;
	}

	public bool IsOptional {
		get; set;
	}

	public string Name {
		get; set;
	}
}

public sealed class CommandCallback
{
	public List<CommandArgument> Arguments {
		get;
	}

	public CommandCallback()
	{
		Arguments = new();
	}
}

public sealed class CommandGroup
{
	public List<CommandGroup> Groups {
		get;
	}

	public List<CommandCallback> Callbacks {
		get;
	}

	public CommandGroup()
	{
		Groups = new();
		Callbacks = new();
	}
}
