using Content.Shared.PlayerMovement;
using Robust.Client.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Physics.Components;
using SQLitePCL;

namespace Content.Client.PlayerMovement
{
	public sealed class PlayerMovementController : SharedPlayerMovementController
	{
		[Dependency] private readonly IPlayerManager _playerManager = default!;

		public override void UpdateBeforeSolve(bool prediction, float frameTime)
		{
			base.UpdateBeforeSolve(prediction, frameTime);

			if (_playerManager.LocalPlayer?.ControlledEntity is not { Valid: true } player)
				return;

			player.EnsureComponentWarn<PhysicsComponent>().Predict = true;
			SetPlayerVelocity(player);
		}
	}
}
