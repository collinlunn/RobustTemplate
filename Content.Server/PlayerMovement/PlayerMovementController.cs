using Content.Shared.PlayerMovement;
using JetBrains.Annotations;

namespace Content.Server.PlayerMovement
{
	[UsedImplicitly]
	public sealed class PlayerMovementController : SharedPlayerMovementController
	{
		public override void UpdateBeforeSolve(bool prediction, float frameTime)
		{
			base.UpdateBeforeSolve(prediction, frameTime);

			//Get paused players too, so you can move around on paused maps for mapping
			//Regular players still frozen, since their PhysicsComponent hasnt been set to ignore pause
			var allQuery = AllEntityQuery<PlayerMovementComponent>(); 
			while (allQuery.MoveNext(out var player, out _))
			{
				SetPlayerVelocity(player);
			}
		}
	}
}
