using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.IoC;

namespace Content.Client.Lobby;

public sealed class LobbyState : State
{
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;

    private LobbyHud? _lobby;

    protected override void Startup()
    {
        _lobby = new LobbyHud();

        LayoutContainer.SetAnchorAndMarginPreset(_lobby, LayoutContainer.LayoutPreset.Wide);

        _userInterface.StateRoot.AddChild(_lobby);
    }

    protected override void Shutdown()
    {
        _lobby?.Dispose();
    }
}