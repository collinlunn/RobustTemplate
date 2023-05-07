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
	public sealed class LobbyHudController : UIController, IOnStateChanged<LobbyState>
	{
		public Enum UiKey => LobbyUiKey.Key;
		public LobbyUiState DefaultState => new DefaultLobbyState();

		[Dependency] private readonly IUserInterfaceManager _userInterface = default!;
		//[Dependency] private readonly ClientUiStateManager _uiStateMan = default!; //this is carashing on inject
		[Dependency] private readonly IClientNetManager _clientNetManager = default!;
		[Dependency] private readonly IGameController _gameController = default!;

		private LobbyUiState _uiState = default!;

		private LobbyHud? _lobbyHud;

		public override void Initialize()
		{
			base.Initialize();
			SubscribeNetworkEvent<OpenUiConnectionMessage>((msg,_) => SetUiState(msg.State));
			SubscribeNetworkEvent<CloseUiConnectionMessage>((msg, _) => SetUiState(null));
			SubscribeNetworkEvent<StateUiConnectionMessage>((msg, _) => SetUiState(msg.State));
			//SetUiState(_uiStateMan.GetUiStateOrNull(UiKey));
		}

		public void OnStateEntered(LobbyState state)
		{
			DebugTools.Assert(_lobbyHud == null);
			_lobbyHud = new LobbyHud();
			_lobbyHud.SetState(_uiState);

			_lobbyHud.StartGameButton.OnPressed += _ =>
			{
				//IoCManager.Resolve<IEntityManager>.(new StartGamePressedEvent());
			};
			_lobbyHud.StartMappingButton.OnPressed += _ =>
			{
				//_entityNetManager.SendSystemNetworkMessage(new StartMappingPressedEvent());
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

		public void SetUiState(UiState? state)
		{
			if (state is not LobbyUiState lobbyState)
				return;

			_uiState = lobbyState ?? DefaultState;
			_lobbyHud?.SetState(_uiState);
		}

		private sealed class DefaultLobbyState : LobbyUiState
		{

		}
	}
}
