using Robust.Shared.Input.Binding;
using Robust.Shared.Input;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Players;
using Robust.Shared.GameObjects;
using Robust.Shared.Physics.Systems;
using Robust.Shared.IoC;
using Robust.Shared.Maths;

namespace Content.Shared.PlayerMovement
{
    public class SharedPlayerMovementController : VirtualController
    {
        [Dependency] protected readonly SharedPhysicsSystem _physics = default!;

        public override void Initialize()
        {
            base.Initialize();

            var upHandler = new PlayerMovementInputCmdHandler(this, Direction.North);
			var downHandler = new PlayerMovementInputCmdHandler(this, Direction.South);
			var leftHandler = new PlayerMovementInputCmdHandler(this, Direction.West);
			var rightHandler = new PlayerMovementInputCmdHandler(this, Direction.East);

			CommandBinds.Builder
                .Bind(EngineKeyFunctions.MoveUp, upHandler)
				.Bind(EngineKeyFunctions.MoveDown, downHandler)
				.Bind(EngineKeyFunctions.MoveLeft, leftHandler)
                .Bind(EngineKeyFunctions.MoveRight, rightHandler)
                .Register<SharedPlayerMovementController>();
        }

		public override void Shutdown()
		{
			base.Shutdown();
			CommandBinds.Unregister<SharedPlayerMovementController>();
		}
        
        private void HandleMovementInput(EntityUid entity, ushort subTick, bool buttonPressed)
        {

        }

        private sealed class PlayerMovementInputCmdHandler : InputCmdHandler
        {
            private readonly SharedPlayerMovementController _controller;
			private readonly Direction _dir;

			public PlayerMovementInputCmdHandler(SharedPlayerMovementController controller, Direction dir)
            {
                _controller = controller;
				_dir = dir;

			}

            //what does this do why do we return false here
            public override bool HandleCmdMessage(ICommonSession session, InputCmdMessage message)
            {
                if (message is not FullInputCmdMessage full || session?.AttachedEntity == null) return false;

                var buttonPressed = full.State == BoundKeyState.Down;
                _controller.HandleMovementInput(session.AttachedEntity.Value, message.SubTick, buttonPressed);
                return false;
            }
        }
    }
}
