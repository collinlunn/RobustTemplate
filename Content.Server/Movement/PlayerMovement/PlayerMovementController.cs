using Content.Shared.Movement;
using Content.Shared.Movement.PlayerMovement;
using JetBrains.Annotations;
using Robust.Shared.Physics.Components;

namespace Content.Server.Movement.PlayerMovement
{
    [UsedImplicitly]
    public sealed class PlayerMovementController : SharedPlayerMovementController
    {
        public override void UpdateBeforeSolve(bool prediction, float frameTime)
        {
            base.UpdateBeforeSolve(prediction, frameTime);

            var playerMovementQuery =
				EntityQueryEnumerator<PlayerMovementComponent>();
            while (playerMovementQuery.MoveNext(
				out var player, out var playerMovement))
            {
                SetPlayerKinematics(player, playerMovement, frameTime);
            }
        }
    }
}
