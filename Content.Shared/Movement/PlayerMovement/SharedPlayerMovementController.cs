using Robust.Shared.Physics.Controllers;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.Movement.PlayerMovement
{
    public abstract class SharedPlayerMovementController : VirtualController
    {
        [Dependency] protected readonly SharedPhysicsSystem _physics = default!;
        [Dependency] protected readonly IGameTiming _timing = default!;


        protected void SetPlayerVelocity(
            EntityUid mappingPlayer, MoveButtonTrackerComponent movebuttonTracker, PlayerMovementComponent mappingMovement)
        {
            var newVelocityDir = movebuttonTracker.HeldButtons.ToVelocityDir();
            var speed = mappingMovement.Speed;

            var newVelocity = newVelocityDir * speed;
            PhysicsSystem.SetLinearVelocity(mappingPlayer, newVelocity);
        }
    }
}
