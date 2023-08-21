using Content.Shared.Mapping;
using Content.Shared.Movement;
using JetBrains.Annotations;
using Robust.Client.Player;
using Robust.Client.GameObjects;
using Robust.Client.Physics;

namespace Content.Client.Mapping
{
    [UsedImplicitly]
    public sealed class MappingMovementController : SharedMappingMovementController
	{
        [Dependency] private readonly IPlayerManager _playerManager = default!;

		public override void Initialize()
		{
			base.Initialize();
			SubscribeLocalEvent<MappingMovementComponent, PlayerAttachSysMessage>(OnPlayerAttached);
			SubscribeLocalEvent<MappingMovementComponent, UpdateIsPredictedEvent>(OnUpdatePredicted);
		}

		private void OnPlayerAttached(EntityUid uid, MappingMovementComponent component, PlayerAttachSysMessage args)
		{
			_physics.UpdateIsPredicted(uid);
		}

		private void OnUpdatePredicted(EntityUid uid, MappingMovementComponent component, ref UpdateIsPredictedEvent args)
		{
			// Enable prediction if an entity is controlled by the player
			if (uid == _playerManager.LocalPlayer?.ControlledEntity)
			{
				args.IsPredicted = true;
			}
		}

		public override void UpdateBeforeSolve(bool prediction, float frameTime)
        {
            base.UpdateBeforeSolve(prediction, frameTime);

            if (_playerManager.LocalPlayer?.ControlledEntity is not { Valid: true } mappingPlayer)
                return;

            if (!TryComp<MappingMovementComponent>(mappingPlayer, out var mappingMovement))
                return;

			var moveButtonTracker = mappingPlayer.EnsureComponentWarn<MoveButtonTrackerComponent>();

            SetPlayerVelocity(mappingPlayer, moveButtonTracker, mappingMovement);
        }
    }
}
