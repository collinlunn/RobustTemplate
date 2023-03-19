using Content.Shared.PlayerMovement;
using Robust.Shared.Physics.Components;

namespace Content.Server.PlayerMovement
{
	public sealed class PlayerMovementController : SharedPlayerMovementController
	{
		public override void UpdateBeforeSolve(bool prediction, float frameTime)
		{
			base.UpdateBeforeSolve(prediction, frameTime);

			foreach (var playerMovement in EntityQuery<PlayerMovementComponent>())
			{
				SetPlayerVelocity(player);
			}	
		}
	}
}
