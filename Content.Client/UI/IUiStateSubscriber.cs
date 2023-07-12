using Content.Shared.UI;

namespace Content.Client.UI
{
	/// <summary>
	///		For ui's that want to be notified of incoming server ui states.
	/// </summary>
	public interface IUiStateSubscriber
	{
		/// <summary>
		///		The key the server use to refer to this UI.
		/// </summary>
		public Enum UiKey { get; }

		/// <summary>
		///		Passes a state to the UI
		///		The status specifies what is happening with the sever ui connection.
		/// </summary>
		public void SetState(UiState state, UiConnectionStatus status);
	}
}
