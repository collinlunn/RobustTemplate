using Content.Client.Admin;
using Content.Server.Admin;
using Content.Shared.Admin;
using Content.Shared.Helpers;
using Robust.Shared.Console;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Toolshed;
using Robust.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Tests.IntegrationTests.Admin
{
	[TestFixture]
	public sealed class CommandPermissionsTest
	{
		[Test]
		public async Task Test()
		{
			var (server, client) = await TestProcessManager.GetTestServerClient();

			var clientAdmin = client.System<ClientAdminManager>();
			var clientConsole = client.ResolveDependency<IConsoleHost>();

			var clientAdminManagerCommandsNotInConsole = new List<string>();
			var clientCommandsNotInAdminManager = new List<string>();

			await client.WaitPost(() =>
			{
				var consolePerms = clientAdmin.ConsolePermissions;
				DebugTools.Assert(consolePerms != null);
				var adminConsoleCommands = consolePerms.Keys;
				var availableCommands = clientConsole.AvailableCommands.Keys;

				foreach (var command in adminConsoleCommands)
				{
					if (!availableCommands.Contains(command))
						clientAdminManagerCommandsNotInConsole.Add(command);
				}
				foreach (var command in availableCommands)
				{
					if (!adminConsoleCommands.Contains(command))
						clientCommandsNotInAdminManager.Add(command);
				}
			});

			var serverAdminManagerCommandsNotInConsole = new List<string>();
			var serverCommandsNotInAdminManager = new List<string>();

			var serverAdmin = server.System<ServerAdminManager>();
			var serverConsole = server.ResolveDependency<IConsoleHost>();
			var serverToolbox = server.ResolveDependency<ToolshedManager>();

			await server.WaitPost(() =>
			{
				var adminConsoleCommands = serverAdmin.ServerConsolePermissions.Keys;
				var availableCommands = serverConsole.AvailableCommands.Keys;

				foreach (var command in adminConsoleCommands)
				{
					if (!availableCommands.Contains(command))
						serverAdminManagerCommandsNotInConsole.Add(command);
				}
				foreach (var command in availableCommands)
				{
					if (!adminConsoleCommands.Contains(command))
						serverCommandsNotInAdminManager.Add(command);
				}
			});

			var adminManagerToolboxCommandsNotInToolbox = new List<string>();
			var toolboxCommandsNotInAdminManager = new List<string>();

			await server.WaitPost(() =>
			{
				var adminToolboxCommands = serverAdmin.ToolboxPermissions.Keys;
				var toolboxEnv = serverToolbox.DefaultEnvironment;
				var availableToolboxCommands = toolboxEnv.AllCommands().Select(command => command.FullName());

				foreach (var command in adminToolboxCommands)
				{
					if (!availableToolboxCommands.Contains(command))
						adminManagerToolboxCommandsNotInToolbox.Add(command);
				}
				foreach (var command in availableToolboxCommands)
				{
					if (!adminToolboxCommands.Contains(command))
						toolboxCommandsNotInAdminManager.Add(command);
				}
			});

			client.Dispose();
			server.Dispose();

			var commandsAreMissing =
				clientAdminManagerCommandsNotInConsole.Any() ||
				clientCommandsNotInAdminManager.Any() ||
				serverAdminManagerCommandsNotInConsole.Any() ||
				serverCommandsNotInAdminManager.Any() ||
				adminManagerToolboxCommandsNotInToolbox.Any() ||
				toolboxCommandsNotInAdminManager.Any();

			if (commandsAreMissing)
			{
				Assert.Fail($"Commands are missing:\n" +
					$"{FailText(clientAdminManagerCommandsNotInConsole, "Client console doesn't have command")}\n" +
					$"{FailText(clientCommandsNotInAdminManager, "Client admin manager doesn't have command")}\n" +
					$"{FailText(serverAdminManagerCommandsNotInConsole, "Server console doesn't have command")}\n" +
					$"{FailText(serverCommandsNotInAdminManager, "Server admin manager doesn't have command")}\n" +
					$"{FailText(adminManagerToolboxCommandsNotInToolbox, "Toolbox doesn't have command")}\n" +
					$"{FailText(toolboxCommandsNotInAdminManager, "Server admin manager doesn't have toolbox command")}");
			}

			string FailText(List<string> missingCommands, string prefix)
			{
				var failText = string.Empty;
				foreach (var command in missingCommands)
				{
					failText += $"{prefix}: {command}\n";
				}
				return failText;
			}
		}
	}
}
