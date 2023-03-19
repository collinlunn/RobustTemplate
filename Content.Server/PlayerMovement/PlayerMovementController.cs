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

			var query = EntityQueryEnumerator<PlayerMovementComponent>();
			while (query.MoveNext(out var player, out var _))
			{
				SetPlayerVelocity(player);
			}
		}
	}
}
