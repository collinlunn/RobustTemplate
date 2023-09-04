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
				foreach (var command in clientAdmin.ConsolePermissions.Keys)
				{
					if (!clientConsole.AvailableCommands.ContainsKey(command))
					{
						throw new Exception($"Client console doesn't have command {command}");
					}
				}
			});

			var serverAdmin = server.System<ServerAdminManager>();
			var serverConsole = server.ResolveDependency<IConsoleHost>();
			var serverToolbox = server.ResolveDependency<ToolshedManager>();

			await server.WaitPost(() =>
			{
				foreach (var command in serverAdmin.ConsolePermissions.Keys)
				{
					if (!serverConsole.AvailableCommands.ContainsKey(command))
					{
						throw new Exception($"Server console doesn't have command {command}");
					}
				}
				foreach (var command in serverAdmin.ToolboxPermissions.Keys)
				{
					if (!serverToolbox.DefaultEnvironment.TryGetCommand(command, out _))
					{
						throw new Exception($"Toolbox doesn't have command {command}");
					}
				}
			});
			client.Dispose();
			server.Dispose();
		}
	}
}
