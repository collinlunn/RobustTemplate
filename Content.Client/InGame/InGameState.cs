using Robust.Client.GameObjects;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Client.State;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.GameObjects;
using Robust.Shared.Input;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Timing;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.InGame;

public sealed class InGameState : State
{
    [Dependency] private readonly IInputManager _inputManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;

    protected override void Startup()
    {
        _inputManager.KeyBindStateChanged += OnKeyBindStateChanged;
    }

    protected override void Shutdown()
    {
        _inputManager.KeyBindStateChanged -= OnKeyBindStateChanged;
    }

	/// <summary>
	///		Called when a keybind's state changes (up, down). Triggers the effects of said keybinds.
	/// </summary>
	/// <param name="args">Details what changed.</param>
    private void OnKeyBindStateChanged(ViewportBoundKeyEventArgs args)
    {
        // If there is no InputSystem, then there is nothing to forward to, and nothing to do here.
        if (!_entitySystemManager.TryGetEntitySystem(out InputSystem? inputSys))
            return;

		//When window is focused on top bar or a UI, substitute an appropriate ViewPort
		if (args.Viewport == null)
            return; //_uiManager.ActiveScreen!.GetWidget<>(MainViewport)!;

		var keyEventArgs = args.KeyEventArgs;
        var function = keyEventArgs.Function;
        var functionID = _inputManager.NetworkBindMap.KeyFunctionID(function);

        EntityCoordinates coordinates = default;
        EntityUid entityToClick = default;
        if (args.Viewport is IViewportControl vp)
        {
			//Get entity under mouse when keybind state changed
			var mousePosWorld = vp.ScreenToMap(keyEventArgs.PointerLocation.Position);
			if (TryGetClickedEntity(mousePosWorld, out var foundEntity))
				entityToClick = (EntityUid) foundEntity;

			//Get coordinates of mouse when keybind state changed
			coordinates = _mapManager.TryFindGridAt(mousePosWorld, out var grid) ? 
				grid.MapToGrid(mousePosWorld) :
                EntityCoordinates.FromMap(_mapManager, mousePosWorld);
        }

        var message = new FullInputCmdMessage(_timing.CurTick, _timing.TickFraction, functionID, keyEventArgs.State,
            coordinates, keyEventArgs.PointerLocation, entityToClick);

        // client side command handlers will always be sent the local player session.
        var session = _playerManager.LocalPlayer?.Session;
        if (inputSys.HandleInputCommand(session, function, message))
			keyEventArgs.Handle();
	}

    private bool TryGetClickedEntity(MapCoordinates coordinates, [NotNullWhen(true)] out EntityUid? foundEntity)
    {
		foundEntity = null;
        return foundEntity != null;
    }
}