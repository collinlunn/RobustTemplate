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
	///		For opening a UI on a client, sent from server -> client.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class OpenUiMessage : BaseUiMessage
	{
		/// <summary>
		///		What type of UI should be opened?
		/// </summary>
		public string OpenType { get; }

		public OpenUiMessage(string type)
		{
			OpenType = type;
		}
	}

	/// <summary>
	///		For closing a UI on a client, sent from server -> client.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class CloseUiMessage : BaseUiMessage
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
