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
		[Dependency] private readonly IResourceManager _res = default!;

		protected Dictionary<string, AdminFlags> TryLoadPermissions(string path)
		{
			if (!_res.TryContentFileRead(new ResPath(path), out var stream))
			{
				Log.Error($"Couldn't find permission file at {path}");
				return new(); //return empty dict with no perms
			}
			return LoadPermissionsFromStream(stream);
		}

		private Dictionary<string, AdminFlags> LoadPermissionsFromStream(Stream stream)
		{
			var output = new Dictionary<string, AdminFlags>();

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
					if (output.ContainsKey(cmd))
					{
						Log.Error($"Duplicate command {cmd} in config file");
						continue;
					}

					output.Add(cmd, (AdminFlags)flag);
				}
			}
			return output;
		}

		public static bool TryGetFlagFromName(string name, [NotNullWhen(true)] out AdminFlags? flag)
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

		public Dictionary<string, AdminFlags> CommandPermissions = new();

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
		public Dictionary<string, AdminFlags> CommandPermissions { get; }

		public UpdatePlayerPermissionsEvent(PlayerPermissions perms, Dictionary<string, AdminFlags> commandPermissions)
		{
			PlayerPermissions = perms;
			CommandPermissions = commandPermissions;
		}
	}
}
