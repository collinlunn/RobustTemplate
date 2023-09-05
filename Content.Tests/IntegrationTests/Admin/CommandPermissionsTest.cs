using Content.Client.Admin;
using Content.Server.Admin;
using Content.Shared.Admin;
using Robust.Shared.Console;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Toolshed;
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

			await client.WaitPost(() =>
			{
				var consolePerms = clientAdmin.ConsolePermissions;
				Assert.NotNull(consolePerms);
				var adminConsoleCommands = consolePerms.Keys;
				var availableCommands = clientConsole.AvailableCommands.Keys;

				foreach (var command in adminConsoleCommands)
				{
					Assert.That(availableCommands.Contains(command),
						$"Client console doesn't have command {command}");
				}
				foreach (var command in availableCommands)
				{
					Assert.That(adminConsoleCommands.Contains(command),
						$"Client admin manager doesn't have command {command}");
				}
			});

			var serverAdmin = server.System<ServerAdminManager>();
			var serverConsole = server.ResolveDependency<IConsoleHost>();
			var serverToolbox = server.ResolveDependency<ToolshedManager>();

			await server.WaitPost(() =>
			{
				var adminConsoleCommands = serverAdmin.ServerConsolePermissions.Keys;
				var availableCommands = serverConsole.AvailableCommands.Keys;

				foreach (var command in adminConsoleCommands)
				{
					Assert.That(availableCommands.Contains(command),
						$"Server console doesn't have command {command}");
				}
				foreach (var command in availableCommands)
				{
					Assert.That(adminConsoleCommands.Contains(command),
						$"Server admin manager doesn't have command {command}");
				}
			});

			await server.WaitPost(() =>
			{
				var adminToolboxCommands = serverAdmin.ToolboxPermissions.Keys;
				var toolboxEnv = serverToolbox.DefaultEnvironment;
				var availableToolboxCommands = toolboxEnv.AllCommands().Select(command => command.FullName());

				foreach (var command in adminToolboxCommands)
				{
					Assert.That(availableToolboxCommands.Contains(command),
						$"Toolbox doesn't have command {command}");
				}
				foreach (var command in availableToolboxCommands)
				{
					Assert.That(adminToolboxCommands.Contains(command),
						$"Server admin manager doesn't have toolbox command {command}");
				}
			});

			client.Dispose();
			server.Dispose();
		}
	}
}
