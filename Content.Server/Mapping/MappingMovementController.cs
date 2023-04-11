using Content.Shared.Mapping;
using Content.Shared.Movement;
using JetBrains.Annotations;

namespace Content.Server.PlayerMovement
{
    [UsedImplicitly]
	public sealed class MappingMovementController : SharedMappingMovementController
	{
		public override void UpdateBeforeSolve(bool prediction, float frameTime)
		{
			base.UpdateBeforeSolve(prediction, frameTime);

			//Includes paused entities, as players will be on a paused map during mapping
			var mappingMovementQuery = AllEntityQuery<MoveButtonTrackerComponent, MappingMovementComponent>();
			while (mappingMovementQuery.MoveNext(out var mappingPlayer, out var movebuttonTracker, out var mappingMovement))
			{
				SetPlayerVelocity(mappingPlayer, movebuttonTracker, mappingMovement);
			}
		}
	}
}
