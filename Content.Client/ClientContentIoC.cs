using Content.Client.UserInterface;
using Robust.Shared.IoC;

namespace Content.Client;

internal static class ClientContentIoC
{
    public static void Register()
    {
        // DEVNOTE: IoCManager registrations for the client go here and only here.
        IoCManager.Register<StyleSheetManager, StyleSheetManager>();
    }
}