using Content.Client.Admin;
using Content.Client.StyleSheets;
using Content.Client.UI;

namespace Content.Client;

internal static class ClientContentIoC
{
    public static void Register()
    {
        // DEVNOTE: IoCManager registrations for the client go here and only here.
        IoCManager.Register<StyleSheetManager>();
		IoCManager.Register<ClientAdminManager>();
	}
}