using Content.Shared.Mapping;
using JetBrains.Annotations;

namespace Content.Server.PlayerMovement
{
	[UsedImplicitly]
	public sealed class MappingMovementController : SharedMappingMovementController
	{
		public override void UpdateBeforeSolve(bool prediction, float frameTime)
		{
			base.UpdateBeforeSolve(prediction, frameTime);

			var mappingMovementQuery = AllEntityQuery<MappingMovementComponent>();
			while (mappingMovementQuery.MoveNext(out var mappingPlayer, out _))
			{
				SetPlayerVelocity(mappingPlayer);
			}
		}
	}
}
