using Content.Server.Admin;
using Content.Server.UI;

namespace Content.Server;

internal static class ServerContentIoC
{
    public static void Register()
    {
		// DEVNOTE: IoCManager registrations for the server go here and only here.
		IoCManager.Register<IAdminConsoleManager, AdminConsoleManager>();
		IoCManager.Register<ServerUiManager, ServerUiManager>();
	}
}