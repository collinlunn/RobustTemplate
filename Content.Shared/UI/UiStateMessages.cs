using Robust.Shared.Serialization;

namespace Content.Shared.UI
{
	[Serializable, NetSerializable]
	public abstract class BaseUiConnectionMessage : EntityEventArgs
	{
		/// <summary>
		///		The key is that client and server systems used to identify which ui states are for them.
		///		Different UIs will make different keys.
		/// </summary>
		public Enum UiKey { get; }

		public BaseUiConnectionMessage(Enum uiKey)
		{
			UiKey = uiKey;
		}
	}

	/// <summary>
	///		For sending the initial state of a ui to a client, sent from server -> client.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class OpenUiConnectionMessage : BaseUiConnectionMessage
	{
		public UiState State { get; }

		public OpenUiConnectionMessage(Enum uiKey, UiState state) : base(uiKey)
		{
			State = state;
		}
	}

	/// <summary>
	///		For deleting a UI state on a client, sent from server -> client.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class CloseUiConnectionMessage : BaseUiConnectionMessage
	{
		public CloseUiConnectionMessage(Enum uiKey) : base(uiKey)
		{

		}
	}

	/// <summary>
	///		For transmitting UI state, sent from server -> client.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class StateUiConnectionMessage : BaseUiConnectionMessage
	{
		public UiState State { get; }

		public StateUiConnectionMessage(Enum uiKey, UiState state) : base(uiKey)
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
