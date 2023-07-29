using Robust.Shared.GameObjects;
using Robust.Shared.Profiling;

namespace Content.Tests.IntegrationTests.Tests
{
    [TestFixture]
	[TestOf(typeof(TestProcessManager))]
	public sealed class TestProcessManagerTests
	{
		[Test]
		public async Task GenerateServerClientTest()
		{
			var (server, client) = await TestProcessManager.GetTestServerClient();
			client.Dispose();
			server.Dispose();
		}
	}
}
