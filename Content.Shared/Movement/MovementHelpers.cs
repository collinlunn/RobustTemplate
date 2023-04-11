using Robust.Shared.Maths;
using System;

namespace Content.Shared.Movement
{
	[Flags]
	public enum MoveButtons : byte
	{
		None = 0,
		Up = 1 << 0,
		Down = 1 << 1,
		Left = 1 << 2,
		Right = 1 << 3,
	}

	public static class MoveButtonHelpers
	{
		/// <summary>
		///		Returns a unit vector in the direction the mapping player is moving.
		/// </summary>
		public static Vector2 ToVelocityDir(this MoveButtons buttons)
		{
			var velocity = new Vector2(0, 0);

			if (buttons.HasFlag(MoveButtons.Up))
				velocity += new Vector2(0, 1);

			if (buttons.HasFlag(MoveButtons.Down))
				velocity += new Vector2(0, -1);

			if (buttons.HasFlag(MoveButtons.Left))
				velocity += new Vector2(-1, 0);

			if (buttons.HasFlag(MoveButtons.Right))
				velocity += new Vector2(1, 0);

			if (velocity.Length != 0)
				return velocity.Normalized; //convert to unit vector because this is just for direction
			else
				return velocity; //length 0 vector cannot be normalized so just return the zero vector
		}
	}
}
