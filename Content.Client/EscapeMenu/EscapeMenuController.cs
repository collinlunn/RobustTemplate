using Content.Client.InGame;
using Content.Client.OptionsMenu;
using JetBrains.Annotations;
using Robust.Client;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Network;
using Robust.Shared.Utility;

namespace Content.Client.EscapeMenu;

[UsedImplicitly]
public sealed class EscapeMenuController : UIController, IOnStateChanged<InGameState>
{
	[Dependency] private readonly IGameController _gameController = default!;
	[Dependency] private readonly IClientNetManager _netManager = default!;
	[Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

	private EscapeMenu? _escapeWindow;

	public void OnStateEntered(InGameState state)
    {
        DebugTools.Assert(_escapeWindow == null);
        _escapeWindow = UIManager.CreateWindow<EscapeMenu>();

		_escapeWindow.OptionsButton.OnPressed += _ =>
		{
			_userInterfaceManager.GetUIController<OptionsMenuController>().ToggleWindow();
		};
		_escapeWindow.DisconnectButton.OnPressed += _ =>
		{
			CloseWindow();
			_netManager.ClientDisconnect("Client pressed disconnect button.");
		};
		_escapeWindow.QuitButton.OnPressed += _ =>
		{
			CloseWindow();
			_gameController.Shutdown("Client pressed quit button.");
		};

		CommandBinds.Builder
            .Bind(EngineKeyFunctions.EscapeMenu,
                InputCmdHandler.FromDelegate(_ => ToggleWindow()))
            .Register<EscapeMenuController>();
    }

	public void OnStateExited(InGameState state)
    {
		_escapeWindow?.Dispose();
		_escapeWindow = null;
		CommandBinds.Unregister<EscapeMenuController>();
    }

	private void ToggleWindow()
	{
		if (_escapeWindow == null)
			return;

		if (_escapeWindow.IsOpen)
		{
			CloseWindow();
		}
		else
		{
			_escapeWindow.OpenCentered();
		}
	}

	private void CloseWindow()
	{
		_escapeWindow?.Close();
	}
}