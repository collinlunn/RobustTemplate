using Content.Shared.Lobby;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Client.Lobby;

public sealed class LobbyState : State
{
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    [Dependency] private readonly IEntityNetworkManager _netManager = default!;

    private LobbyHud? _lobby;

    protected override void Startup()
    {
        _lobby = new LobbyHud();
        _lobby.OnConnectButtonPressed += StartGamePressed;

        LayoutContainer.SetAnchorAndMarginPreset(_lobby, LayoutContainer.LayoutPreset.Wide);

        _userInterface.StateRoot.AddChild(_lobby);
    }

    private void StartGamePressed(BaseButton.ButtonEventArgs obj)
    {
        _netManager.SendSystemNetworkMessage(new StartGamePressedEvent());
    }

    protected override void Shutdown()
    {
        _lobby?.Dispose();
    }
}