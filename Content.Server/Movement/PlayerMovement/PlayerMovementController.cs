using Content.Shared.Movement;
using Content.Shared.Movement.PlayerMovement;
using JetBrains.Annotations;

namespace Content.Server.Movement.PlayerMovement
{
    [UsedImplicitly]
    public sealed class PlayerMovementController : SharedPlayerMovementController
    {
        public override void UpdateBeforeSolve(bool prediction, float frameTime)
        {
            base.UpdateBeforeSolve(prediction, frameTime);

            //Includes paused entities, as players will be on a paused map during Player
            var PlayerMovementQuery = AllEntityQuery<MoveButtonTrackerComponent, PlayerMovementComponent>();
            while (PlayerMovementQuery.MoveNext(out var PlayerPlayer, out var movebuttonTracker, out var PlayerMovement))
            {
                SetPlayerVelocity(PlayerPlayer, movebuttonTracker, PlayerMovement);
            }
        }
    }
}
