using Robust.Shared.Serialization;

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

		public LoadUiMessage(string type)
		{
			LoadType = type;
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
	///		Individual UI's will override with their own state data.
	/// </summary>
	[Serializable, NetSerializable]
	public abstract class UiStateMessage : BaseUiMessage
	{

	}

	/// <summary>
	///		Base message for transmitting an event related to the UI, sent from server -> client.
	///		Individual UI's will override with their own event data.
	/// </summary>
	[Serializable, NetSerializable]
	public abstract class UiEventMessage : BaseUiMessage
	{

	}

	/// <summary>
	///		Base message for transmitting an input related to the UI, sent from client -> server.
	///		Individual UI's will override with their own input data.
	/// </summary>
	[Serializable, NetSerializable]
	public abstract class UiInputMessage : BaseUiMessage
	{

	}
}
