using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.Movement.PlayerMovement
{
    /// <summary>
    ///		Makes this entity respond to movement key inputs, uses momentum-based parameters.
    /// </summary>
    [Access(typeof(SharedPlayerMovementController))]
	[NetworkedComponent]
    [RegisterComponent]
    public sealed partial class PlayerMovementComponent : Component
    {
		/// <summary>
		///		Peak speed player can achieve.
		/// </summary>
		[ViewVariables(VVAccess.ReadWrite)]
		[DataField("maxSpeed")]
		public float MaxSpeed = 1;

		/// <summary>
		///		Acceleration (s^-2) in direction of movement buttons pressed.
		/// </summary>
		[ViewVariables(VVAccess.ReadWrite)]
		[DataField("acceleration")]
		public float Acceleration = 2;

		/// <summary>
		///		Constant deceleration (s^-2) applied at all times.
		/// </summary>
		[ViewVariables(VVAccess.ReadWrite)]
		[DataField("linearDrag")]
		public float LinearDrag = 1;

		/// <summary>
		///		Time (s) to reach full speed from stop.
		/// </summary>
		[ViewVariables(VVAccess.ReadOnly)]
		public float TimeToFullSpeed => MaxSpeed / (Acceleration - LinearDrag);

		/// <summary>
		///		Time (s) to stop from full speed.
		/// </summary>
		[ViewVariables(VVAccess.ReadOnly)]
		public float TimeToFullStop => MaxSpeed / LinearDrag;

		/// <summary>
		///		Prototype helper to set parameters.
		///		1st, 2nd, and 3rd list elements are the TimeToFullSpeed, TimeToFullStop, and MaxSpeed.
		/// </summary>
		[ViewVariables(VVAccess.ReadWrite)]
		[DataField("startStopMax")]
		public List<float> StartStopMax
		{
			get => new() { TimeToFullSpeed, TimeToFullStop, MaxSpeed };
			set
			{
				if (!value.TryGetValue(0, out var startTime) ||
					!value.TryGetValue(1, out var stopTime) ||
					!value.TryGetValue(2, out var maxSpeed))
					return;

				MaxSpeed = maxSpeed;
				Acceleration = maxSpeed * (1 / startTime + 1 / stopTime);
				LinearDrag = maxSpeed / stopTime;
			}
		}
	}
}
