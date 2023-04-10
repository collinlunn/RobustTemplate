using Robust.Shared.Input.Binding;
using Robust.Shared.Input;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Players;
using Robust.Shared.GameObjects;
using Robust.Shared.Physics.Systems;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using System;
using Robust.Shared.Timing;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

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

            var upHandler = new MappingMovementInputCmdHandler(this, MoveButtons.Up);
			var downHandler = new MappingMovementInputCmdHandler(this, MoveButtons.Down);
			var leftHandler = new MappingMovementInputCmdHandler(this, MoveButtons.Left);
			var rightHandler = new MappingMovementInputCmdHandler(this, MoveButtons.Right);

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
        
        private void HandleMovementInput(EntityUid entity, ushort subTick, MoveButtons button, bool buttonPressed)
        {
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
			var newVelocityDir = TryGetVelocityDir(mappingMovement.HeldButtons);
			var newVelocity = newVelocityDir * mappingMovement.Speed;
			PhysicsSystem.SetLinearVelocity(mappingPlayer, newVelocity);
		}

		private sealed class MappingMovementInputCmdHandler : InputCmdHandler
        {
            private readonly SharedMappingMovementController _controller;
			private readonly MoveButtons _button;

			public MappingMovementInputCmdHandler(SharedMappingMovementController controller, MoveButtons button)
            {
                _controller = controller;
				_button = button;
			}

            public override bool HandleCmdMessage(ICommonSession? session, InputCmdMessage message)
            {
                if (message is not FullInputCmdMessage full || session?.AttachedEntity == null) return false;

                var buttonPressed = full.State == BoundKeyState.Down;
                _controller.HandleMovementInput(session.AttachedEntity.Value, message.SubTick, _button, buttonPressed);
                return false; //return false to avoid blocking other keybinds
            }
        }

		/// <summary>
		///		Returns a unit vector in the direction the mapping player is moving.
		/// </summary>
		private static Vector2 TryGetVelocityDir(MoveButtons buttons)
		{
			var velocity = new Vector2(0, 0);

			if (buttons.HasFlag(MoveButtons.Up))
				velocity += new Vector2(0, 1);

			if (buttons.HasFlag(MoveButtons.Down))
				velocity += new Vector2(0, -1);

			if (buttons.HasFlag(MoveButtons.Left))
				velocity += new Vector2(-1, 0);

			if (buttons.HasFlag(MoveButtons.Right))
				velocity += new Vector2(1, 0);

			if (velocity.Length != 0)
				return velocity.Normalized; //convert to unit vector because this is just for direction
			else
				return velocity; //length 0 vector cannot be normalized so just return the zero vector
		}		
    }

	[Flags]
	public enum MoveButtons : byte
	{
		None = 0,
		Up = 1 << 0,
		Down = 1 << 1,
		Left = 1 << 2,
		Right = 1 << 3,
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
