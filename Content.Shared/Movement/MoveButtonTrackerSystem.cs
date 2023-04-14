using Robust.Shared.GameObjects;
using Robust.Shared.Input.Binding;
using Robust.Shared.Input;
using Robust.Shared.Players;

namespace Content.Shared.Movement
{
	public sealed class MoveButtonTrackerSystem : EntitySystem
	{
		public override void Initialize()
		{
			base.Initialize();

			var upHandler = new MovementInputCmdHandler(MoveButtons.Up, this);
			var downHandler = new MovementInputCmdHandler(MoveButtons.Down, this);
			var leftHandler = new MovementInputCmdHandler(MoveButtons.Left, this);
			var rightHandler = new MovementInputCmdHandler(MoveButtons.Right, this);

			CommandBinds.Builder
				.Bind(EngineKeyFunctions.MoveUp, upHandler)
				.Bind(EngineKeyFunctions.MoveDown, downHandler)
				.Bind(EngineKeyFunctions.MoveLeft, leftHandler)
				.Bind(EngineKeyFunctions.MoveRight, rightHandler)
				.Register<MoveButtonTrackerSystem>();
		}

		public override void Shutdown()
		{
			base.Shutdown();
			CommandBinds.Unregister<MoveButtonTrackerSystem>();
		}

		private void HandleMovementInput(EntityUid entity, MoveButtons button, bool buttonPressed)
		{
			var mappingMovement = entity.EnsureComponentWarn<MoveButtonTrackerComponent>();

			if (buttonPressed)
				mappingMovement.HeldButtons |= button;
			else
				mappingMovement.HeldButtons &= ~button;

			Dirty(mappingMovement);
		}

		public sealed class MovementInputCmdHandler : InputCmdHandler
		{
			private readonly MoveButtons _button;
			private readonly MoveButtonTrackerSystem _tracker;

			public MovementInputCmdHandler(MoveButtons button, MoveButtonTrackerSystem tracker)
			{
				_button = button;
				_tracker = tracker;
			}

			public override bool HandleCmdMessage(ICommonSession? session, InputCmdMessage message)
			{
				if (message is not FullInputCmdMessage full || session?.AttachedEntity == null) return false;

				var buttonPressed = full.State == BoundKeyState.Down;
				_tracker.HandleMovementInput(session.AttachedEntity.Value, _button, buttonPressed);
				return false; //return false to avoid blocking other keybinds
			}
		}
	}
}
