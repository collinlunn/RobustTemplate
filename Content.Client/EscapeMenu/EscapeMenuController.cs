using Content.Client.InGame;
using Content.Client.MainMenu;
using JetBrains.Annotations;
using Robust.Client;
using Robust.Client.State;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Utility;
using System;

namespace Content.Client.EscapeMenu;

[UsedImplicitly]
public sealed class EscapeMenuController : UIController, IOnStateEntered<InGameState>, IOnStateExited<InGameState>
{
	[Dependency] private readonly IGameController _gameController = default!;
	[Dependency] private readonly IClientNetManager _netManager = default!;
	[Dependency] private readonly IStateManager _stateManager = default!;

	private EscapeMenu? _escapeWindow;

	public void OnStateEntered(InGameState state)
    {
        DebugTools.Assert(_escapeWindow == null);
        _escapeWindow = UIManager.CreateWindow<EscapeMenu>();
		_escapeWindow.Title = "Escape Menu";

		_escapeWindow.OptionsButton.OnPressed += _ =>
		{
			throw new NotImplementedException(); //TODO Make options menu
		};
		_escapeWindow.DisconnectButton.OnPressed += _ =>
		{
			CloseWindow();
			_netManager.ClientDisconnect("Client pressed disconnect button.");
			_stateManager.RequestStateChange<MainMenuState>();
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