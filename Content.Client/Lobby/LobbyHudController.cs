using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Network;
using Robust.Client.UserInterface.Controls;
using Content.Shared.Lobby;
using Robust.Shared.Utility;
using Content.Client.UI;
using Content.Shared.UI;
using Content.Client.OptionsMenu;

namespace Content.Client.Lobby
{
	[UsedImplicitly]
	public sealed class LobbyHudController : UIController, IOnStateChanged<LobbyState>, IUiStateSubscriber
	{
		[Dependency] private readonly IUserInterfaceManager _userInterface = default!;
		[Dependency] private readonly IEntityNetworkManager _entityNetManager = default!;
		[Dependency] private readonly IClientNetManager _clientNetManager = default!;
		[Dependency] private readonly IGameController _gameController = default!;

		[UISystemDependency] private readonly ClientUiStateManager _uiStateMan = default!;

		private LobbyHud? _lobbyHud;

		public void OnStateEntered(LobbyState state)
		{
			DebugTools.Assert(_lobbyHud == null);
			_lobbyHud = new LobbyHud();
			_uiStateMan.AddSubscriber(this);

			_lobbyHud.StartGameButton.OnPressed += _ =>
			{
				_entityNetManager.SendSystemNetworkMessage(new StartGameButtonPressed());
				_lobbyHud.IdleAnimation.Visible = true;
			};
			_lobbyHud.StartMappingButton.OnPressed += _ =>
			{
				_entityNetManager.SendSystemNetworkMessage(new StartMappingButtonPressed());
				_lobbyHud.IdleAnimation.Visible = true;
			};
			_lobbyHud.JoinGameButton.OnPressed += _ =>
			{
				_entityNetManager.SendSystemNetworkMessage(new JoinGameButtonPressed());
				_lobbyHud.IdleAnimation.Visible = true;
			};
			_lobbyHud.OptionsButton.OnPressed += _ =>
			{
				_userInterface.GetUIController<OptionsMenuController>().ToggleWindow();
			};
			_lobbyHud.DisconnectButton.OnPressed += _ =>
			{
				_clientNetManager.ClientDisconnect("Client pressed disconnect button.");
			};
			_lobbyHud.QuitButton.OnPressed += _ =>
			{
				_gameController.Shutdown("Client pressed quit button.");
			};

			LayoutContainer.SetAnchorAndMarginPreset(_lobbyHud, LayoutContainer.LayoutPreset.HorizontalCenterWide);
			_userInterface.StateRoot.AddChild(_lobbyHud);
		}

		public void OnStateExited(LobbyState state)
		{
			_lobbyHud?.Dispose();
			_lobbyHud = null;
			_uiStateMan?.RemoveSubscriber(this); //null forgiveness as entity system are nulled on disconnect
		}

		public Enum UiKey => LobbyUiKey.Key;

		public void SetState(UiState state, UiConnectionStatus status)
		{
			if (state is DefaultUiState)
				_lobbyHud?.SetDefaultState();
			if (state is LobbyUiState lobbyState)
				_lobbyHud?.SetState(lobbyState);
		}
	}
}
