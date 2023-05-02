using Content.Client.UI;
using Content.Shared.Lobby;
using Content.Shared.UI;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;

namespace Content.Client.Lobby
{
	public sealed class LobbyStateUi : ClientStateUi
	{
		[Dependency] private readonly IUserInterfaceManager _userInterface = default!;
		[Dependency] private readonly ClientUiManager _uiManager = default!;

		private LobbyHud? _lobbyHud;

		public override void HandleEvent(UiEventMessage uiEvent)
		{
			
		}

		public override void HandleState(UiStateMessage uiState)
		{

		}

		public override void OnOpen()
		{
			DebugTools.Assert(_lobbyHud == null);
			_lobbyHud = new LobbyHud();
			_lobbyHud.StartGameButton.OnPressed += _ => _uiManager.SendUiInput(this, new StartGameInputMessage());
			_lobbyHud.StartMappingButton.OnPressed += _ => _uiManager.SendUiInput(this, new StartMappingInputMessage());

			LayoutContainer.SetAnchorAndMarginPreset(_lobbyHud, LayoutContainer.LayoutPreset.HorizontalCenterWide);
			_userInterface.StateRoot.AddChild(_lobbyHud);
		}

		public override void OnClose()
		{
			_lobbyHud?.Dispose();
			_lobbyHud = null;
		}
	}
}
