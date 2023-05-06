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
	///		For sending the initial state of a ui to a client.
	///		This is a seperate type of message from regularly sending the state to catch
	///		bugs where a state is sent before "loading" the ui.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class LoadUiMessage : BaseUiMessage
	{
		/// <summary>
		///		The initial value of the state of this UI.
		/// </summary>
		public UiState State { get; }

		public LoadUiMessage(UiState state)
		{
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
	[Serializable, NetSerializable]
	public abstract class UiState
	{

	}

	/// <summary>
	///		A dummy state used by ui connections that haven't specified a state when loading.
	///		For debugging purposes.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class DummyUiState : UiState
	{

	}
}
