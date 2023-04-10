using Robust.Shared.Input.Binding;
using Robust.Shared.Input;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.GameObjects;
using Robust.Shared.Physics.Systems;
using Robust.Shared.IoC;
using System;
using Robust.Shared.Timing;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Content.Shared.Movement;
using static Content.Shared.Movement.MovementInputCmdHandler;

namespace Content.Shared.Mapping
{
	public abstract class SharedMappingMovementController : VirtualController
	{
		[Dependency] protected readonly SharedPhysicsSystem _physics = default!;
		[Dependency] protected readonly IGameTiming _timing = default!;

		public override void Initialize()
		{
			base.Initialize();

			SubscribeLocalEvent<MappingMovementComponent, ComponentGetState>(GetMappingMovementState);
			SubscribeLocalEvent<MappingMovementComponent, ComponentHandleState>(HandleMappingMovementState);

			var upHandler = new MovementInputCmdHandler(MoveButtons.Up, HandleMovementInput);
			var downHandler = new MovementInputCmdHandler(MoveButtons.Down, HandleMovementInput);
			var leftHandler = new MovementInputCmdHandler(MoveButtons.Left, HandleMovementInput);
			var rightHandler = new MovementInputCmdHandler(MoveButtons.Right, HandleMovementInput);

			CommandBinds.Builder
				.Bind(EngineKeyFunctions.MoveUp, upHandler)
				.Bind(EngineKeyFunctions.MoveDown, downHandler)
				.Bind(EngineKeyFunctions.MoveLeft, leftHandler)
				.Bind(EngineKeyFunctions.MoveRight, rightHandler)
				.Register<SharedMappingMovementController>();
		}

		private void GetMappingMovementState(EntityUid uid, MappingMovementComponent component, ref ComponentGetState args)
		{
			args.State = new MappingMovementComponentState(component.HeldButtons);
		}

		private void HandleMappingMovementState(EntityUid uid, MappingMovementComponent component, ref ComponentHandleState args)
		{
			if (args.Current is not MappingMovementComponentState state)
				return;

			component.HeldButtons = state.HeldButtons;
		}

		public override void Shutdown()
		{
			base.Shutdown();
			CommandBinds.Unregister<SharedMappingMovementController>();
		}

		private void HandleMovementInput(OnMoveButtonChangedArgs args)
		{
			var entity = args.Entity;
			var button = args.Button;
			var buttonPressed = args.ButtonPressed;

			var mappingMovement = entity.EnsureComponentWarn<MappingMovementComponent>();

			if (buttonPressed)
				mappingMovement.HeldButtons |= button;
			else
				mappingMovement.HeldButtons &= ~button;

			Dirty(mappingMovement);
		}

		protected void SetPlayerVelocity(EntityUid mappingPlayer)
		{
			var mappingMovement = mappingPlayer.EnsureComponentWarn<MappingMovementComponent>();
			var newVelocityDir = mappingMovement.HeldButtons.ToVelocityDir();
			var newVelocity = newVelocityDir * mappingMovement.Speed;
			PhysicsSystem.SetLinearVelocity(mappingPlayer, newVelocity);
		}
	}

	[Serializable, NetSerializable]
	public sealed class MappingMovementComponentState : ComponentState
	{
		public readonly MoveButtons HeldButtons;

		public MappingMovementComponentState(MoveButtons heldButtons)
		{
			HeldButtons = heldButtons;
		}
	}
}
