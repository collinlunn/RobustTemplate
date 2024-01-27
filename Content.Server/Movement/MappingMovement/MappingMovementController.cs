using Content.Server.UI;
using Content.Shared.Movement;
using Content.Shared.Movement.MappingMovement;
using JetBrains.Annotations;

namespace Content.Server.Movement.MappingMovement
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
