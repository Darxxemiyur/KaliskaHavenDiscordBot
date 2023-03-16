using DisCatSharp.Entities;

namespace KaliskaHaven.DiscordClient
{
	/// <summary>
	/// Universal message container. Fancy way of just presenting what you want to display
	/// regardless of the implementation details.
	/// </summary>
	public class UniversalMessageBuilder
	{
		public IReadOnlyList<IReadOnlyList<DiscordComponent>> Components => _components ?? new();
		private List<List<DiscordComponent>>? _components;
		public IReadOnlyList<DiscordEmbedBuilder> Embeds => _embeds ?? new();
		private List<DiscordEmbedBuilder>? _embeds;
		public IReadOnlyDictionary<string, Stream> Files => _files ?? new();
		private Dictionary<string, Stream>? _files;
		public IReadOnlyList<IMention> Mentions => _mentions ?? new();
		private List<IMention>? _mentions;
		public string Content => _content ?? "";
		private string? _content;

		public UniversalMessageBuilder(DiscordMessage msg)
		{
			_components = msg.Components?.Select(x => x.Components.ToList())?.ToList();
			_embeds = msg.Embeds?.Select(x => new DiscordEmbedBuilder(x))?.ToList();
			_mentions = null;
			_content = msg.Content;
		}

		public UniversalMessageBuilder(UniversalMessageBuilder umm) : this(new[] { umm })
		{
		}

		public UniversalMessageBuilder(params UniversalMessageBuilder[] umm)
		{
			var umbs = umm.Where(x => x != null);
			_embeds = umbs?.Where(x => x.Embeds != null).SelectMany(x => x.Embeds)?.ToList();
			_components = umbs?.Where(x => x.Components != null).SelectMany(x => x.Components)?.Select(x => x.ToList())?.ToList();
			_files = umbs?.SelectMany(x => x.Files)?.ToDictionary(x => x.Key, x => x.Value);
			_mentions = umbs?.Where(x => x.Mentions != null).SelectMany(x => x.Mentions)?.ToList();
			_content = string.Concat(umbs?.Select(x => x.Content)?.Where(x => x != null) ?? new[] { "" });
		}

		public UniversalMessageBuilder(DiscordWebhookBuilder builder)
		{
			_components = builder?.Components?.Where(x => x.Components != null)?.Select(x => x.Components.ToList())?.ToList();
			_embeds = builder?.Embeds?.Select(x => new DiscordEmbedBuilder(x))?.ToList();
			_files = builder?.Files?.ToDictionary(x => x.FileName, x => x.Stream);
			_mentions = builder?.Mentions?.ToList();
			_content = builder?.Content;
		}

		public UniversalMessageBuilder(DiscordMessageBuilder builder)
		{
			_components = builder?.Components?.Where(x => x.Components != null)?.Select(x => x.Components.ToList())?.ToList();
			_embeds = builder?.Embeds?.Select(x => new DiscordEmbedBuilder(x))?.ToList();
			_files = builder?.Files?.ToDictionary(x => x.FileName, x => x.Stream);
			_mentions = builder?.Mentions?.ToList();
			_content = builder?.Content;
		}

		public UniversalMessageBuilder(DiscordInteractionResponseBuilder builder)
		{
			_components = builder?.Components?.Where(x => x.Components != null)?.Select(x => x.Components.ToList())?.ToList();
			_embeds = builder?.Embeds?.Select(x => new DiscordEmbedBuilder(x)).ToList();
			_files = builder?.Files?.ToDictionary(x => x.FileName, x => x.Stream);
			_mentions = builder?.Mentions?.ToList();
			_content = builder?.Content;
		}

		public UniversalMessageBuilder() => ResetBuilder();

		public UniversalMessageBuilder Append(string? appendix) => SetContent(Content + (appendix ?? ""));

		public UniversalMessageBuilder SetContent(string? content)
		{
			_content = content;
			return this;
		}

		public UniversalMessageBuilder NewWithDisabledComponents()
		{
			var components = Components.Select(y => y.Select(x => {
				if (x is DiscordButtonComponent f)
					return new DiscordButtonComponent(f).Disable();
				if (x is DiscordChannelSelectComponent c)
					return new DiscordChannelSelectComponent(c.Label, c.Placeholder, c.ChannelTypes, c.CustomId, c.MinimumSelectedValues ?? 1, c.MaximumSelectedValues ?? 1, true);
				if (x is DiscordMentionableSelectComponent m)
					return new DiscordMentionableSelectComponent(m.Label, m.Placeholder, m.CustomId, m.MinimumSelectedValues ?? 1, m.MaximumSelectedValues ?? 1, true);
				if (x is DiscordRoleSelectComponent r)
					return new DiscordRoleSelectComponent(r.Label, r.Placeholder, r.CustomId, r.MinimumSelectedValues ?? 1, r.MaximumSelectedValues ?? 1, true);
				if (x is DiscordStringSelectComponent s)
					return new DiscordStringSelectComponent(s.Label, s.Placeholder, s.Options, s.CustomId, s.MinimumSelectedValues ?? 1, s.MaximumSelectedValues ?? 1, true);
				if (x is DiscordUserSelectComponent u)
					return new DiscordUserSelectComponent(u.Label, u.Placeholder, u.CustomId, u.MinimumSelectedValues ?? 1, u.MaximumSelectedValues ?? 1, true);
				if (x is DiscordTextComponent t)
					return new DiscordTextComponent(t.Style, t.CustomId, t.Label, t.Placeholder, 0, 0, false, null);
				return x;
			}).ToArray()).ToArray();
			return new UniversalMessageBuilder(this).SetComponents(components);
		}

		public UniversalMessageBuilder AddContent(string content) => SetContent(_content + content);

		public UniversalMessageBuilder AddComponents(params DiscordComponent[] components)
		{
			_components ??= new();
			_components.Add(components.ToList());
			return this;
		}

		public UniversalMessageBuilder AddComponents(params DiscordComponent[][] components)
		{
			foreach (var row in components)
				AddComponents(row);

			return this;
		}

		public UniversalMessageBuilder SetComponents(params DiscordComponent[][]? components)
		{
			_components = components?.Select(x => x.ToList())?.ToList();
			return this;
		}

		public UniversalMessageBuilder AddEmbed(DiscordEmbedBuilder embed)
		{
			_embeds ??= new();
			_embeds.Add(embed);
			return this;
		}

		public UniversalMessageBuilder AddEmbeds(params DiscordEmbedBuilder[] embeds)
		{
			foreach (var row in embeds)
				AddEmbed(row);

			return this;
		}

		public UniversalMessageBuilder SetEmbeds(params DiscordEmbedBuilder[]? embeds)
		{
			_embeds = embeds?.ToList();

			return this;
		}

		public UniversalMessageBuilder SetFile(string name, Stream file)
		{
			_files ??= new();
			_files[name] = file;

			return this;
		}

		public UniversalMessageBuilder SetFiles(IReadOnlyDictionary<string, Stream>? files)
		{
			_files = files?.ToDictionary(x => x.Key, x => x.Value);

			return this;
		}

		public UniversalMessageBuilder OverrideFiles(IReadOnlyDictionary<string, Stream>? files)
		{
			if (files == null)
				return this;

			foreach (var file in _files ??= new())
				_files[file.Key] = file.Value;

			return this;
		}

		public UniversalMessageBuilder AddMention(IMention mention)
		{
			_mentions ??= new();
			_mentions.Add(mention);

			return this;
		}

		public UniversalMessageBuilder AddMentions(IEnumerable<IMention> mentions)
		{
			_mentions ??= new();
			_mentions.AddRange(mentions);

			return this;
		}

		public UniversalMessageBuilder ResetBuilder()
		{
			_components = null;
			_embeds = null;
			_files = null;
			_mentions = null;
			_content = null;

			return this;
		}

		public static implicit operator UniversalMessageBuilder(string msg) => new UniversalMessageBuilder().SetContent(msg);

		public static implicit operator UniversalMessageBuilder(DiscordComponent[][] msg) => new UniversalMessageBuilder().SetComponents(msg);

		public static implicit operator UniversalMessageBuilder(DiscordComponent[] msg) => new UniversalMessageBuilder().SetComponents(msg);

		public static implicit operator UniversalMessageBuilder(DiscordEmbedBuilder msg) => new UniversalMessageBuilder().AddEmbed(msg);

		public static implicit operator UniversalMessageBuilder(DiscordEmbedBuilder[] msg) => new UniversalMessageBuilder().AddEmbeds(msg);

		public static implicit operator UniversalMessageBuilder(DiscordWebhookBuilder msg) => new(msg);

		public static implicit operator UniversalMessageBuilder(DiscordMessageBuilder msg) => new(msg);

		public static implicit operator UniversalMessageBuilder(DiscordInteractionResponseBuilder msg) => new(msg);

		public static implicit operator DiscordWebhookBuilder(UniversalMessageBuilder umb)
		{
			var dwb = new DiscordWebhookBuilder();

			if (umb._components != null)
				foreach (var row in umb._components)
					dwb.AddComponents(row);

			if (umb._embeds != null)
				dwb.AddEmbeds(umb._embeds.Select(x => x.Build()));

			if (umb._content != null)
				dwb.WithContent(umb._content);

			if (umb._files != null)
				dwb.AddFiles(umb._files);

			if (umb._mentions != null)
				dwb.AddMentions(umb._mentions);

			return dwb;
		}

		public static implicit operator DiscordMessageBuilder(UniversalMessageBuilder umb)
		{
			var dmb = new DiscordMessageBuilder();

			if (umb._components != null)
				foreach (var row in umb._components)
					dmb.AddComponents(row);

			if (umb._embeds != null)
				dmb.AddEmbeds(umb._embeds.Select(x => x.Build()));

			if (umb._content != null)
				dmb.WithContent(umb._content);

			if (umb._files != null)
				dmb.WithFiles(umb._files);

			if (umb._mentions != null)
				dmb.WithAllowedMentions(umb._mentions);

			return dmb;
		}

		public static implicit operator DiscordInteractionResponseBuilder(UniversalMessageBuilder umb)
		{
			var dirb = new DiscordInteractionResponseBuilder();

			if (umb._components != null)
				foreach (var row in umb._components)
					dirb.AddComponents(row);

			if (umb._embeds != null)
				dirb.AddEmbeds(umb._embeds.Select(x => x.Build()));

			if (umb._content != null)
				dirb.WithContent(umb._content);

			if (umb._files != null)
				dirb.AddFiles(umb._files);

			if (umb._mentions != null)
				dirb.AddMentions(umb._mentions);

			return dirb;
		}
	}
}
