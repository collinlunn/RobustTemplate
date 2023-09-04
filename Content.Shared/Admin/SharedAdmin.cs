using Robust.Shared.ContentPack;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.Admin
{
	public abstract class SharedAdminManager : EntitySystem
	{
		[Dependency] protected readonly IResourceManager Res = default!;

		public const string SharedCommandPermPath = "/Commands/sharedCommandPerms.yml";
		public const string ClientCommandPermPath = "/Commands/clientCommandPerms.yml";
		public const string ServerCommandPermPath = "/Commands/serverCommandPerms.yml";
		public const string ToolboxCommandPermPath = "/Commands/toolboxCommandPerms.yml";

		protected IReadOnlyDictionary<string, AdminFlags> ConsolePermissions => _consolePermissions;
		private readonly Dictionary<string, AdminFlags> _consolePermissions = new();

		protected abstract IEnumerable<string> ConsolePermPaths { get; }

		public override void Initialize()
		{
			base.Initialize();

			foreach (var path in ConsolePermPaths)
			{
				if (!Res.TryContentFileRead(new ResPath(path), out var consoleStream))
				{
					Log.Error($"Couldn't find console permission file {path}");
					continue;
				}
				LoadPermissionsFromStream(consoleStream, _consolePermissions);
			}
		}

		protected void LoadPermissionsFromStream(Stream stream, Dictionary<string, AdminFlags> permissions)
		{
			using var reader = new StreamReader(stream, EncodingHelpers.UTF8);
			var yamlStream = new YamlStream();
			yamlStream.Load(reader);
			var root = (YamlSequenceNode)yamlStream.Documents[0].RootNode;

			foreach (var child in root)
			{
				var map = (YamlMappingNode)child;

				var commands = map.GetNode<YamlSequenceNode>("Commands").Select(p => p.AsString());
				var flagsNode = map.GetNode("Flags");

				var flagName = flagsNode.AsString();

				if (!TryGetFlagFromName(flagName, out var flag))
				{
					Log.Error($"{flagName} does not correspond to an {nameof(AdminFlags)}");
					continue;
				}

				foreach (var cmd in commands)
				{
					if (permissions.ContainsKey(cmd))
					{
						Log.Error($"Duplicate command {cmd} in config file");
						continue;
					}

					permissions.Add(cmd, (AdminFlags)flag);
				}
			}
		}

		private static bool TryGetFlagFromName(string name, [NotNullWhen(true)] out AdminFlags? flag)
		{
			flag = name switch
			{
				"None" => AdminFlags.None,
				"AdminMenu" => AdminFlags.AdminMenu,
				"Debug" => AdminFlags.Debug,
				"Server" => AdminFlags.Server,
				"Host" => AdminFlags.Host,
				_ => null,
			};
			return flag != null;
		}
	}

	[Serializable, NetSerializable]
	public sealed class PlayerPermissions
	{
		public AdminFlags Permissions { get; set; } = AdminFlags.None;

		public bool CanSpawn()
		{
			return Permissions.HasFlag(AdminFlags.Debug);
		}

		public bool CanAdminMenu()
		{
			return Permissions.HasFlag(AdminFlags.AdminMenu);
		}

		public bool IsHost()
		{
			return Permissions.HasFlag(AdminFlags.Host);
		}
	}

	[Flags]
	public enum AdminFlags
	{
		None = 0,
		AdminMenu = 1 << 0, //can use admin menus (bans, etc)
		Debug = 1 << 1, //can manipulate game (vv, etc)
		Server = 1 << 2, //can manipulate server (shutdown, etc)
		Host = 1 << 3, //can execute dangerous server-side scripts
		All = ~0,
	}

	[Serializable, NetSerializable]
	public sealed class UpdatePlayerPermissionsEvent : EntityEventArgs
	{
		public PlayerPermissions PlayerPermissions { get; }

		public UpdatePlayerPermissionsEvent(PlayerPermissions perms)
		{
			PlayerPermissions = perms;
		}
	}
}
