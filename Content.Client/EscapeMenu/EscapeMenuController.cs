using Content.Client.InGame;
using JetBrains.Annotations;
using Robust.Client.Input;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Utility;

namespace Content.Client.EscapeMenu;

[UsedImplicitly]
public sealed class EscapeMenuController : UIController, IOnStateEntered<InGameState>, IOnStateExited<InGameState>
{
	private EscapeMenu? _escapeWindow;

	public void OnStateEntered(InGameState state)
    {
        DebugTools.Assert(_escapeWindow == null);
        _escapeWindow = UIManager.CreateWindow<EscapeMenu>();

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