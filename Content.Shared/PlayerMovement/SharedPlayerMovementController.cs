using Robust.Shared.Input.Binding;
using Robust.Shared.Input;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Players;
using Robust.Shared.GameObjects;
using Robust.Shared.Physics.Systems;
using Robust.Shared.IoC;

namespace Content.Shared.PlayerMovement
{
    public class SharedPlayerMovementController : VirtualController
    {
        [Dependency] protected readonly SharedPhysicsSystem _physics = default!;

        public override void Initialize()
        {
            base.Initialize();

            var cmdHandler = new PlayerMovementInputCmdHandler(this);

            CommandBinds.Builder
                .Bind(EngineKeyFunctions.MoveUp, cmdHandler)
                .Bind(EngineKeyFunctions.MoveLeft, cmdHandler)
                .Bind(EngineKeyFunctions.MoveRight, cmdHandler)
                .Bind(EngineKeyFunctions.MoveDown, cmdHandler)
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

            public PlayerMovementInputCmdHandler(SharedPlayerMovementController controller)
            {
                _controller = controller;
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
