using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;
using System.Numerics;

namespace Content.Shared.Movement.PlayerMovement
{
    public abstract class SharedPlayerMovementController : VirtualController
    {
        [Dependency] protected readonly SharedPhysicsSystem _physics = default!;
        [Dependency] protected readonly IGameTiming _timing = default!;
		[Dependency] private readonly SharedTransformSystem _tranform = default!;

		protected void SetPlayerKinematics(
            EntityUid player, PlayerMovementComponent playerMovement, float frameTime)
        {
			var movebuttonTracker = Comp<MoveButtonTrackerComponent>(player);
			var position = _tranform.GetWorldPosition(player);
			
			var newVelocity = NewVelocity(
				PhysicsSystem.GetLinearVelocity(player, position),
				playerMovement.LinearDrag,
				movebuttonTracker.HeldButtons.ToVelocityDir(),
				playerMovement.Acceleration,
				playerMovement.MaxSpeed,
				frameTime);

			PhysicsSystem.SetLinearVelocity(player, newVelocity);
		}

		private static Vector2 NewVelocity(Vector2 velocity, float linearDrag, Vector2 moveDir, float acceleration, float maxSpeed, float frameTime)
		{
			var startingSpeed = velocity.Length();
			if (startingSpeed > 0)
			{
				var flatSpeedReduction = linearDrag * frameTime;
				var speedAfterReduction = Math.Max(startingSpeed - flatSpeedReduction, 0);
				velocity *= speedAfterReduction / startingSpeed;
			}
			velocity += moveDir * acceleration * frameTime;

			var newSpeed = velocity.Length();

			if (newSpeed > maxSpeed)
			{
				velocity.Normalize();
				velocity *= maxSpeed;
			}
			return velocity;
		}
	}
}
