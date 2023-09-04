using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Robust.Shared.Console;
using Robust.Shared.Toolshed;
using Robust.Shared.Utility;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.Admin
{
	public sealed class CommandPermissionTestReader
	{
		[Dependency] private readonly IConsoleHost _console = default!;
		[Dependency] private readonly ToolshedManager _toolshed = default!;

		public IReadOnlyDictionary<string, AdminFlags> Permissions => _permissions;
		private readonly Dictionary<string, AdminFlags> _permissions = new();

		public CommandPermissionTestReader()
		{
			IoCManager.InjectDependencies(this);
		}

		public void LoadPermissionsFromStream(Stream stream)
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
					Logger.Error($"{flagName} does not correspong to an {nameof(AdminFlags)}");
					continue;
				}

				foreach (var cmd in commands)
				{
					if (_permissions.ContainsKey(cmd))
					{
						Logger.Error($"Duplicate command {cmd} in config file");
						continue;
					}
					if (!_console.AvailableCommands.ContainsKey(cmd) && 
						!_toolshed.DefaultEnvironment.TryGetCommand(cmd, out _))
					{
						Logger.Error($"Unavailable command {cmd} in config file");
						continue;
					}
					_permissions.Add(cmd, (AdminFlags) flag);
				}
			}
		}

		private bool TryGetFlagFromName(string name, [NotNullWhen(true)] out AdminFlags? flag)
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
}
