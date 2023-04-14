using Robust.Shared.Analyzers;
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement
{
	/// <summary>
	///		Keeps track of what movement buttons a player has held down, so movement systems can move them appropriately.
	/// </summary>
	[Access(typeof(MoveButtonTrackerSystem))]
	[AutoGenerateComponentState]
	[NetworkedComponent]
    [RegisterComponent]
    public sealed partial class MoveButtonTrackerComponent : Component
    {
		/// <summary>
		///		What buttons the player is holding down.
		/// </summary>
		[AutoNetworkedField]
		public MoveButtons HeldButtons = MoveButtons.None;
	}
}
