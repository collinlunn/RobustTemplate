using Robust.Shared.GameStates;

namespace Content.Shared.Movement.PlayerMovement
{
    /// <summary>
    ///		For moving player around on paused maps while mapping.
    /// </summary>
    [Access(typeof(SharedPlayerMovementController))]
	[NetworkedComponent]
    [RegisterComponent]
    public sealed partial class PlayerMovementComponent : Component
    {
		[DataField("speed")]
		public ushort Speed = 1;
    }
}
