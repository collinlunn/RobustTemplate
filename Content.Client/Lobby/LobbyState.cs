using Content.Shared.Lobby;
using Robust.Client;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;

namespace Content.Client.Lobby;

public sealed class LobbyState : State
{
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    [Dependency] private readonly IEntityNetworkManager _entityNetManager = default!;
	[Dependency] private readonly IClientNetManager _clientNetManager = default!;
	[Dependency] private readonly IGameController _gameController = default!;

	private LobbyHud? _lobbyHud;

    protected override void Startup()
    {
        _lobbyHud = new LobbyHud();
        _lobbyHud.StartGameButton.OnPressed += _ => StartGamePressed();

        LayoutContainer.SetAnchorAndMarginPreset(_lobbyHud, LayoutContainer.LayoutPreset.Wide);
		_lobbyHud.DisconnectButton.OnPressed += _ =>
		{
			_clientNetManager.ClientDisconnect("Client pressed disconnect button.");
		};
		_lobbyHud.QuitButton.OnPressed += _ =>
		{
			_gameController.Shutdown("Client pressed quit button.");
		};

		_userInterface.StateRoot.AddChild(_lobbyHud);
    }

    private void StartGamePressed()
    {
        _entityNetManager.SendSystemNetworkMessage(new StartGamePressedEvent());
    }

    protected override void Shutdown()
    {
        _lobbyHud?.Dispose();
    }
}