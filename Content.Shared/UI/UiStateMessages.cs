﻿using Robust.Shared.Serialization;

namespace Content.Shared.UI
{
	/// <summary>
	///		Base message for all UI communication.
	/// </summary>
	[Serializable, NetSerializable]
	public abstract class BaseUiMessage
	{

	}

	/// <summary>
	///		For loading a UI on a client, sent from server -> client.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class LoadUiMessage : BaseUiMessage
	{
		/// <summary>
		///		What type of UI should be opened?
		/// </summary>
		public string LoadType { get; }

		/// <summary>
		///		The initial value of the state of this UI.
		/// </summary>
		public UiState State { get; }

		public LoadUiMessage(string type, UiState state)
		{
			LoadType = type;
			State = state;
		}
	}

	/// <summary>
	///		For unloading a UI on a client, sent from server -> client.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class UnloadUiMessage : BaseUiMessage
	{

	}

	/// <summary>
	///		Base message for transmitting UI state, sent from server -> client.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class UiStateMessage : BaseUiMessage
	{
		public UiState State { get; }

		public UiStateMessage(UiState state)
		{
			State = state;
		}
	}

	/// <summary>
	///		A Ui state. Each ui will override with own version.
	/// </summary>
	public abstract class UiState
	{

	}
}
