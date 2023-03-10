using DisCatSharp;

using System.Diagnostics.CodeAnalysis;

namespace KaliskaHaven.DiscordClient
{
	[Serializable]
	public class DiscordConfig
	{
		[MemberNotNull]
		public string Token {
			get; set;
		}


		public DiscordConfiguration GetConfig => new() {
			Token = Token,
			Intents = DiscordIntents.All,
		};
	}
}