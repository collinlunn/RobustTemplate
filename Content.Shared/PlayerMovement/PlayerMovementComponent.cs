using Robust.Shared.Analyzers;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Timing;

namespace Content.Shared.PlayerMovement
{
	[Access(typeof(SharedPlayerMovementController))]
	[RegisterComponent]
	public sealed class PlayerMovementComponent : Component
	{
		public MoveButtons HeldButtons = MoveButtons.None;

		public GameTick LastInputTick;
		public ushort LastInputSubTick;

		[DataField("speed")]
		public ushort Speed = 1;
	}
}
