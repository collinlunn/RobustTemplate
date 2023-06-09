using Content.Shared.Mapping;
using Content.Shared.Movement;
using JetBrains.Annotations;
using Robust.Client.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Physics.Components;

namespace Content.Client.Mapping
{
    [UsedImplicitly]
    public sealed class MappingMovementController : SharedMappingMovementController
	{
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        public override void UpdateBeforeSolve(bool prediction, float frameTime)
        {
            base.UpdateBeforeSolve(prediction, frameTime);

            if (_playerManager.LocalPlayer?.ControlledEntity is not { Valid: true } mappingPlayer)
                return;

            if (!TryComp<MappingMovementComponent>(mappingPlayer, out var mappingMovement))
                return;

			var moveButtonTracker = mappingPlayer.EnsureComponentWarn<MoveButtonTrackerComponent>();

			mappingPlayer.EnsureComponentWarn<PhysicsComponent>().Predict = true;
            SetPlayerVelocity(mappingPlayer, moveButtonTracker, mappingMovement);
        }
    }
}
