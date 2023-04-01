using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Client.UserInterface.Controls;
using Content.Shared.Lobby;
using Robust.Shared.Utility;

namespace Content.Client.Lobby
{
	[UsedImplicitly]
	public sealed class LobbyHudController : UIController, IOnStateEntered<LobbyState>, IOnStateExited<LobbyState>
	{
		[Dependency] private readonly IUserInterfaceManager _userInterface = default!;
		[Dependency] private readonly IEntityNetworkManager _entityNetManager = default!;
		[Dependency] private readonly IClientNetManager _clientNetManager = default!;
		[Dependency] private readonly IGameController _gameController = default!;

		private LobbyHud? _lobbyHud;

		public void OnStateEntered(LobbyState state)
		{
			DebugTools.Assert(_lobbyHud == null);
			_lobbyHud = new LobbyHud();
			_lobbyHud.SetReadyButton.OnPressed += _ =>
			{
				_entityNetManager.SendSystemNetworkMessage(new LobbyPlayerReadyEvent(true));
			};
			_lobbyHud.StartGameButton.OnPressed += _ =>
			{
				_entityNetManager.SendSystemNetworkMessage(new StartGamePressedEvent());
			};
			_lobbyHud.StartMappingButton.OnPressed += _ =>
			{
				_entityNetManager.SendSystemNetworkMessage(new StartMappingPressedEvent());
			};
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

		public void OnStateExited(LobbyState state)
		{
			_lobbyHud?.Dispose();
			_lobbyHud = null;
		}
	}
}
