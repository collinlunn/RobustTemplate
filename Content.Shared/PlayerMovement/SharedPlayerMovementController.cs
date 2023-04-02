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

namespace Content.Shared.PlayerMovement
{
    public abstract class SharedPlayerMovementController : VirtualController
    {
        [Dependency] protected readonly SharedPhysicsSystem _physics = default!;
		[Dependency] protected readonly IGameTiming _timing = default!;

		public override void Initialize()
        {
            base.Initialize();

			SubscribeLocalEvent<PlayerMovementComponent, ComponentGetState>(GetPlayerMovementState);
			SubscribeLocalEvent<PlayerMovementComponent, ComponentHandleState>(HandlePlayerMovementState);

            var upHandler = new PlayerMovementInputCmdHandler(this, MoveButtons.Up);
			var downHandler = new PlayerMovementInputCmdHandler(this, MoveButtons.Down);
			var leftHandler = new PlayerMovementInputCmdHandler(this, MoveButtons.Left);
			var rightHandler = new PlayerMovementInputCmdHandler(this, MoveButtons.Right);

			CommandBinds.Builder
                .Bind(EngineKeyFunctions.MoveUp, upHandler)
				.Bind(EngineKeyFunctions.MoveDown, downHandler)
				.Bind(EngineKeyFunctions.MoveLeft, leftHandler)
                .Bind(EngineKeyFunctions.MoveRight, rightHandler)
                .Register<SharedPlayerMovementController>();
        }

		private void GetPlayerMovementState(EntityUid uid, PlayerMovementComponent component, ref ComponentGetState args)
		{
			args.State = new PlayerMovementComponentState(component.HeldButtons);
		}

		private void HandlePlayerMovementState(EntityUid uid, PlayerMovementComponent component, ref ComponentHandleState args)
		{
			if (args.Current is not PlayerMovementComponentState state)
				return;

			component.HeldButtons = state.HeldButtons;
		}

		public override void Shutdown()
		{
			base.Shutdown();
			CommandBinds.Unregister<SharedPlayerMovementController>();
		}
        
        private void HandleMovementInput(EntityUid entity, ushort subTick, MoveButtons button, bool buttonPressed)
        {
			var playerMovement = entity.EnsureComponentWarn<PlayerMovementComponent>();

			if (buttonPressed)
				playerMovement.HeldButtons |= button;
			else
				playerMovement.HeldButtons &= ~button;
			
			playerMovement.LastInputTick = _timing.CurTick;
			playerMovement.LastInputSubTick = subTick;

			Dirty(playerMovement);
		}

		protected void SetPlayerVelocity(EntityUid player)
		{
			var playerMovement = player.EnsureComponentWarn<PlayerMovementComponent>();
			var newVelocityDir = TryGetVelocityDir(playerMovement.HeldButtons);
			var newVelocity = newVelocityDir * playerMovement.Speed;
			PhysicsSystem.SetLinearVelocity(player, newVelocity);
		}

		private sealed class PlayerMovementInputCmdHandler : InputCmdHandler
        {
            private readonly SharedPlayerMovementController _controller;
			private readonly MoveButtons _button;

			public PlayerMovementInputCmdHandler(SharedPlayerMovementController controller, MoveButtons button)
            {
                _controller = controller;
				_button = button;
			}

            public override bool HandleCmdMessage(ICommonSession session, InputCmdMessage message)
            {
                if (message is not FullInputCmdMessage full || session?.AttachedEntity == null) return false;

                var buttonPressed = full.State == BoundKeyState.Down;
                _controller.HandleMovementInput(session.AttachedEntity.Value, message.SubTick, _button, buttonPressed);
                return false; //return false to avoid blocking other keybinds
            }
        }

		/// <summary>
		///		Returns a unit vector in the direction the player is moving.
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
	public sealed class PlayerMovementComponentState : ComponentState
	{
		public readonly MoveButtons HeldButtons;

		public PlayerMovementComponentState(MoveButtons heldButtons)
		{
			HeldButtons = heldButtons;
		}
	}
}
