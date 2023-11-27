using Content.Shared.Mapping;
using Content.Shared.Movement;
using JetBrains.Annotations;
using Robust.Client.Player;
using Robust.Client.GameObjects;
using Robust.Client.Physics;
using Robust.Shared.Player;

namespace Content.Client.Mapping
{
    [UsedImplicitly]
    public sealed class MappingMovementController : SharedMappingMovementController
	{
        [Dependency] private readonly IPlayerManager _playerManager = default!;

		public override void Initialize()
		{
			base.Initialize();
			SubscribeLocalEvent<MappingMovementComponent, PlayerAttachedEvent>(OnPlayerAttached);
			SubscribeLocalEvent<MappingMovementComponent, UpdateIsPredictedEvent>(OnUpdatePredicted);
		}

		private void OnPlayerAttached(EntityUid uid, MappingMovementComponent component, PlayerAttachedEvent args)
		{
			_physics.UpdateIsPredicted(uid);
		}

		private void OnUpdatePredicted(EntityUid uid, MappingMovementComponent component, ref UpdateIsPredictedEvent args)
		{
			// Enable prediction if an entity is controlled by the player
			if (uid == _playerManager?.LocalSession?.AttachedEntity)
			{
				args.IsPredicted = true;
			}
		}

		public override void UpdateBeforeSolve(bool prediction, float frameTime)
        {
            base.UpdateBeforeSolve(prediction, frameTime);

            if (_playerManager?.LocalSession?.AttachedEntity is not { Valid: true } mappingPlayer)
                return;

            if (!TryComp<MappingMovementComponent>(mappingPlayer, out var mappingMovement))
                return;

			var moveButtonTracker = Comp<MoveButtonTrackerComponent>(mappingPlayer);

            SetPlayerVelocity(mappingPlayer, moveButtonTracker, mappingMovement);
        }
    }
}
