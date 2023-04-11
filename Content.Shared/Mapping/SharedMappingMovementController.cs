using Robust.Shared.Physics.Controllers;
using Robust.Shared.GameObjects;
using Robust.Shared.Physics.Systems;
using Robust.Shared.IoC;
using Robust.Shared.Timing;
using Content.Shared.Movement;

namespace Content.Shared.Mapping
{
    public abstract class SharedMappingMovementController : VirtualController
	{
		[Dependency] protected readonly SharedPhysicsSystem _physics = default!;
		[Dependency] protected readonly IGameTiming _timing = default!;


		protected void SetPlayerVelocity(
			EntityUid mappingPlayer, MoveButtonTrackerComponent movebuttonTracker, MappingMovementComponent mappingMovement)
		{
			var newVelocityDir = movebuttonTracker.HeldButtons.ToVelocityDir();
			var speed = mappingMovement.Speed;

			var newVelocity = newVelocityDir * speed;
			PhysicsSystem.SetLinearVelocity(mappingPlayer, newVelocity);
		}
	}
}
