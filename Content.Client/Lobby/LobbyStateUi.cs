using Content.Client.UI;
using Content.Shared.UI;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;

namespace Content.Client.Lobby
{
	public sealed class LobbyStateUi : ClientStateUiConnection
	{
		[Dependency] private readonly IUserInterfaceManager _userInterface = default!;

		private LobbyHud? _lobbyHud;

		public override void HandleState(UiStateMessage uiState)
		{

		}

		public override void OnLoad()
		{
			DebugTools.Assert(_lobbyHud == null);
			_lobbyHud = new LobbyHud();
			//_lobbyHud.StartGameButton.OnPressed += _ => SendUiInput(new StartGameInputMessage());
			//_lobbyHud.StartMappingButton.OnPressed += _ => SendUiInput(new StartMappingInputMessage());

			LayoutContainer.SetAnchorAndMarginPreset(_lobbyHud, LayoutContainer.LayoutPreset.HorizontalCenterWide);
			_userInterface.StateRoot.AddChild(_lobbyHud);
		}

		public override void OnUnload()
		{
			_lobbyHud?.Dispose();
			_lobbyHud = null;
		}
	}
}
