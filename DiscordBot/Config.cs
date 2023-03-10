using System.Xml.Serialization;
using System.Runtime.Serialization;
using Name.Bayfaderix.Darxxemiyur.Common.Extensions;

namespace KaliskaHaven.DiscordClient
{
	public sealed class Config
	{
		public DiscordConfig Discord {
			get; set;
		}
		public static Config Default {
			get;
		}
		static Config() => Default = new();

	}
}