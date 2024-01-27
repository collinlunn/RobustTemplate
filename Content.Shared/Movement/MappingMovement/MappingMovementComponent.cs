using Content.Shared.Movement.MappingMovement;
using Robust.Shared.Analyzers;
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.Movement.MappingMovement
{
    /// <summary>
    ///		For moving player around on paused maps while mapping.
    /// </summary>
    [Access(typeof(SharedMappingMovementController))]
	[NetworkedComponent]
    [RegisterComponent]
    public sealed partial class MappingMovementComponent : Component
    {
		[DataField("speed")]
		public ushort Speed = 1;
    }
}
