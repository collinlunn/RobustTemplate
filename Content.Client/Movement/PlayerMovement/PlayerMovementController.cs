using Content.Shared.Movement;
using JetBrains.Annotations;
using Robust.Client.Player;
using Robust.Client.Physics;
using Robust.Shared.Player;
using Content.Shared.Movement.PlayerMovement;

namespace Content.Client.Movement.PlayerMovement
{
    [UsedImplicitly]
    public sealed class PlayerMovementController : SharedPlayerMovementController
    {
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<PlayerMovementComponent, PlayerAttachedEvent>(OnPlayerAttached);
            SubscribeLocalEvent<PlayerMovementComponent, UpdateIsPredictedEvent>(OnUpdatePredicted);
        }

        private void OnPlayerAttached(EntityUid uid, PlayerMovementComponent component, PlayerAttachedEvent args)
        {
            _physics.UpdateIsPredicted(uid);
        }

        private void OnUpdatePredicted(EntityUid uid, PlayerMovementComponent component, ref UpdateIsPredictedEvent args)
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

            if (_playerManager?.LocalSession?.AttachedEntity is not { Valid: true } PlayerPlayer)
                return;

            if (!TryComp<PlayerMovementComponent>(PlayerPlayer, out var PlayerMovement))
                return;

            var moveButtonTracker = Comp<MoveButtonTrackerComponent>(PlayerPlayer);

            SetPlayerVelocity(PlayerPlayer, moveButtonTracker, PlayerMovement);
        }
    }
}
