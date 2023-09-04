using Robust.Client;
using Robust.Server;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using static Robust.UnitTesting.RobustIntegrationTest;

[assembly: LevelOfParallelism(3)]

namespace Content.Tests
{
    public static class TestProcessManager
    {
        public static async Task<ServerIntegrationInstance> GetTestServer(TestProcessSettings? settings = null)
        {
			settings ??= new TestProcessSettings();
            var server = await GenerateServer(settings);
			return server;
		}

		public static async Task<ClientIntegrationInstance> GetTestClient(TestProcessSettings? settings = null)
		{
			settings ??= new TestProcessSettings();
			var client = await GenerateClient(settings);
			return client;
		}

		public static async Task<(ServerIntegrationInstance, ClientIntegrationInstance)> GetTestServerClient(TestProcessSettings? settings = null)
        {
			settings ??= new TestProcessSettings();
			var client = await GenerateClient(settings);
			var server = await GenerateServer(settings);

			if (settings.ClientConnected)
			{
				client.SetConnectTarget(server);
				await client.WaitPost(() =>
				{
					var netMgr = IoCManager.Resolve<IClientNetManager>();
					if (!netMgr.IsConnected)
					{
						netMgr.ClientConnect(null!, 0, null!);
					}
				});
				await ReallyBeIdle(server, client, 10);
				await client.WaitRunTicks(1);
			}
			return (server, client);
		}

		/// <summary>
		/// Runs the server/client in sync, but also ensures they are both idle each tick.
		/// </summary>
		/// <param name="pair">The server/client pair</param>
		/// <param name="runTicks">How many ticks to run</param>
		public static async Task ReallyBeIdle(ServerIntegrationInstance server, ClientIntegrationInstance client, int runTicks = 25)
		{
			for (var i = 0; i < runTicks; i++)
			{
				await client.WaitRunTicks(1);
				await server.WaitRunTicks(1);
				for (var idleCycles = 0; idleCycles < 4; idleCycles++)
				{
					await client.WaitIdleAsync();
					await server.WaitIdleAsync();
				}
			}
		}

		private static async Task<ClientIntegrationInstance> GenerateClient(TestProcessSettings settings)
        {
			await TestContext.Out.WriteLineAsync($"{nameof(GenerateClient)}: called by {TestContext.CurrentContext.Test.FullName}");

			var options = new ClientIntegrationOptions
			{
				ExtraPrototypes = settings.ExtraPrototypes,
				ContentStart = true,
				Options = new GameControllerOptions()
				{
					LoadConfigAndUserData = false,
				},
				ContentAssemblies = new[]
				{
					typeof(Shared.EntryPoint).Assembly,
					typeof(Client.EntryPoint).Assembly,
					typeof(TestProcessManager).Assembly
				}
			};
			options.BeforeStart += () =>
            {
				//Register mocks to IoC, load mock entity systems, register mock components
				var entSysMan = IoCManager.Resolve<IEntitySystemManager>();
                var compFactory = IoCManager.Resolve<IComponentFactory>();
				//entSysMan.LoadExtraSystemType<MockEntitySystem>();
				//compFactory.RegisterClass<MockComponent>();
				//IoCManager.Register<MockDependency>();
			};
            var client = new ClientIntegrationInstance(options);
			await client.WaitIdleAsync();
			return client;
        }

        private static async Task<ServerIntegrationInstance> GenerateServer(TestProcessSettings settings)
        {
			await TestContext.Out.WriteLineAsync($"{nameof(GenerateServer)}: called by {TestContext.CurrentContext.Test.FullName}");

			var options = new ServerIntegrationOptions
			{
				ExtraPrototypes = settings.ExtraPrototypes,
				ContentStart = true,
				Options = new ServerOptions()
				{

				},
				ContentAssemblies = new[]
				{
					typeof(Shared.EntryPoint).Assembly,
					typeof(Server.EntryPoint).Assembly,
					typeof(TestProcessManager).Assembly,
				}
			};
			options.BeforeStart += () =>
            {
				//Register mocks to IoC, load mock entity systems, register mock components
				var entSysMan = IoCManager.Resolve<IEntitySystemManager>();
                var compFactory = IoCManager.Resolve<IComponentFactory>();
                //entSysMan.LoadExtraSystemType<MockEntitySystem>();
                //compFactory.RegisterClass<MockComponent>();
                //IoCManager.Register<MockDependency>();
            };
            var server = new ServerIntegrationInstance(options);
            await server.WaitIdleAsync();
            return server;
        }
    }

    public sealed class TestProcessSettings
    {
        public string ExtraPrototypes { get; init; } = string.Empty;
		public bool ClientConnected { get; init; } = true;
	}
}
