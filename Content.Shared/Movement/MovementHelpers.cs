using Robust.Shared.GameObjects;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Maths;
using Robust.Shared.Players;
using System;

namespace Content.Shared.Movement
{
	public sealed class MovementInputCmdHandler : InputCmdHandler
	{		
		private readonly MoveButtons _button;
		private readonly OnMoveButtonChangedDelegate MoveButtonChangedDelegate;

		public MovementInputCmdHandler(MoveButtons button, OnMoveButtonChangedDelegate moveButtonChangedDelegate)
		{
			_button = button;
			MoveButtonChangedDelegate = moveButtonChangedDelegate;
		}

		public override bool HandleCmdMessage(ICommonSession? session, InputCmdMessage message)
		{
			if (message is not FullInputCmdMessage full || session?.AttachedEntity == null) return false;

			var buttonPressed = full.State == BoundKeyState.Down;
			var args = new OnMoveButtonChangedArgs(session.AttachedEntity.Value, message.SubTick, _button, buttonPressed);
			MoveButtonChangedDelegate.Invoke(args);
			return false; //return false to avoid blocking other keybinds
		}

		public delegate void OnMoveButtonChangedDelegate(OnMoveButtonChangedArgs args);

		public sealed class OnMoveButtonChangedArgs
		{
			public readonly EntityUid Entity;
			public readonly ushort SubTick;
			public readonly MoveButtons Button;
			public readonly bool ButtonPressed;

			public OnMoveButtonChangedArgs(EntityUid entity, ushort subTick, MoveButtons button, bool buttonPressed)
			{
				Entity = entity;
				SubTick = subTick;
				Button = button;
				ButtonPressed = buttonPressed;
			}
		}
	}

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
