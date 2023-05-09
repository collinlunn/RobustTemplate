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

		private LobbyUiState? _uiState;

		private LobbyHud? _lobbyHud;

		public void OnStateEntered(LobbyState state)
		{
			DebugTools.Assert(_lobbyHud == null);
			_lobbyHud = new LobbyHud();
			//TODO Add sub

			_lobbyHud.StartGameButton.OnPressed += _ =>
			{
				_entityNetManager.SendSystemNetworkMessage(new StartGameButtonPressed());
			};
			_lobbyHud.StartMappingButton.OnPressed += _ =>
			{
				_entityNetManager.SendSystemNetworkMessage(new StartMappingButtonPressed());
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
		}

		private void TrySetState(Enum uiKey, UiState state)
		{
			if (state is not LobbyUiState lobbyState)
				return;


		}

		public Enum UiKey => LobbyUiKey.Key;

		public UiState DefaultState => new LobbyUiState();

		public void SetState(UiState state)
		{
			if (state is not LobbyUiState lobbyState)
				return;

			_uiState = lobbyState;
			_lobbyHud?.SetState(_uiState);
		}

		public void AfterUiConnectionOpened()
		{
			//Do nothing
		}

		public void AfterUiConnectionClosed()
		{
			//Do nothing
		}
	}
}
