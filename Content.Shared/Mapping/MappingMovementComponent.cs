using Robust.Shared.Analyzers;
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.Mapping
{
	[Access(typeof(SharedMappingMovementController))]
	[NetworkedComponent]
	[RegisterComponent]
	public sealed class MappingMovementComponent : Component
	{
		public MoveButtons HeldButtons = MoveButtons.None;

		[DataField("speed")]
		public ushort Speed = 1;
	}
}
