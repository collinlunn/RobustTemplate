using Robust.Client.ComponentTrees;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.GameObjects;
using Robust.Shared.Input;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Content.Client.InGame;

public sealed class InGameState : State
{
    [Dependency] private readonly IInputManager _inputManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
	[Dependency] private readonly IEntityManager _entityManager = default!;
	[Dependency] private readonly IUserInterfaceManager _uiManager = default!;

	protected override void Startup()
    {
		_uiManager.LoadScreen<InGameHUDScreen>();
        _inputManager.KeyBindStateChanged += OnKeyBindStateChanged;
    }

    protected override void Shutdown()
    {
		_uiManager.UnloadScreen();
        _inputManager.KeyBindStateChanged -= OnKeyBindStateChanged;
    }

	/// <summary>
	///		Called when a keybind's state changes (up, down). Triggers the effects of said keybinds.
	/// </summary>
	/// <param name="args">Details what changed.</param>
    private void OnKeyBindStateChanged(ViewportBoundKeyEventArgs args)
    {
		var viewport = args.Viewport; // ?? _uiManager.MainViewport; to get entities/coords thru other ui elements
		HandleInput(args.KeyEventArgs, viewport);
	}

	private void HandleInput(BoundKeyEventArgs keyEvent, Control? viewportControl)
	{
		if (!_entitySystemManager.TryGetEntitySystem(out InputSystem? inputSys))
			return;

		var function = keyEvent.Function;
		var functionID = _inputManager.NetworkBindMap.KeyFunctionID(function);

		EntityCoordinates coordinates = default;
		EntityUid entityToClick = default;
		if (viewportControl is IViewportControl vp)
		{
			//Get entity under mouse when keybind state changed
			var mousePosWorld = vp.ScreenToMap(keyEvent.PointerLocation.Position);
			if (TryGetClickedEntity(mousePosWorld, out var foundEntity))
				entityToClick = (EntityUid)foundEntity;

			//Get coordinates of mouse when keybind state changed
			coordinates = _mapManager.TryFindGridAt(mousePosWorld, out var grid) ?
				grid.MapToGrid(mousePosWorld) :
				EntityCoordinates.FromMap(_mapManager, mousePosWorld);
		}

		var message = new FullInputCmdMessage(_timing.CurTick, _timing.TickFraction, functionID, keyEvent.State,
			coordinates, keyEvent.PointerLocation, entityToClick);

		// client side command handlers will always be sent the local player session.
		var session = _playerManager.LocalPlayer?.Session;
		if (inputSys.HandleInputCommand(session, function, message))
			keyEvent.Handle();
	}

    private bool TryGetClickedEntity(MapCoordinates coordinates, [NotNullWhen(true)] out EntityUid? foundEntity)
    {
		foundEntity = null;

		// Find all the entities intersecting our click
		var spriteTree = _entityManager.EntitySysManager.GetEntitySystem<SpriteTreeSystem>();
		var spriteEntries = spriteTree.QueryAabb(coordinates.MapId, Box2.CenteredAround(coordinates.Position, (1, 1)), true);

		//Order sprites top to bottom
		spriteEntries.OrderByDescending(spriteEntry => spriteEntry.Component.DrawDepth);
		
		foundEntity = spriteEntries.FirstOrNull()?.Uid;
		return foundEntity != null;
	}
}