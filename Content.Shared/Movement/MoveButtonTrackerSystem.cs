using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Input.Binding;
using Robust.Shared.Input;
using Robust.Shared.Serialization;
using System;
using Robust.Shared.Players;

namespace Content.Shared.Movement
{
	public sealed class MoveButtonTrackerSystem : EntitySystem
	{
		public override void Initialize()
		{
			base.Initialize();

			SubscribeLocalEvent<MoveButtonTrackerComponent, ComponentGetState>(GetMoveButtonTrackerState);
			SubscribeLocalEvent<MoveButtonTrackerComponent, ComponentHandleState>(HandleMoveButtonTrackerState);

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

		//Replace with auto once Engine is updated
		private void GetMoveButtonTrackerState(EntityUid uid, MoveButtonTrackerComponent component, ref ComponentGetState args)
		{
			args.State = new MoveButtonTrackerComponentState(component.HeldButtons);
		}

		//Replace with auto once Engine is updated
		private void HandleMoveButtonTrackerState(EntityUid uid, MoveButtonTrackerComponent component, ref ComponentHandleState args)
		{
			if (args.Current is not MoveButtonTrackerComponentState state)
				return;

			component.HeldButtons = state.HeldButtons;
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

	[Serializable, NetSerializable]
	public sealed class MoveButtonTrackerComponentState : ComponentState
	{
		public readonly MoveButtons HeldButtons;

		public MoveButtonTrackerComponentState(MoveButtons heldButtons)
		{
			HeldButtons = heldButtons;
		}
	}
}
