using Content.Shared.UI;
using System;

namespace Content.Client.UI
{
	public interface IUiStateSubscriber
	{
		/// <summary>
		///		The key the server use to refer to this UI.
		/// </summary>
		public Enum UiKey { get; }

		/// <summary>
		///		Gets the state this UI should have if the server-provided state is not available.
		/// </summary>
		public UiState DefaultState { get; }

		/// <summary>
		///		Passes a state the the UI.
		/// </summary>
		public void SetState(UiState state);

		/// <summary>
		///		Notifies this UI that the server has sent a state.
		///		This is called after the new state has been set.
		/// </summary>
		public void AfterUiConnectionOpened();

		/// <summary>
		///		Notifies this UI that the server has discarded its state.
		///		This is called after the default state has been set.
		/// </summary>
		public void AfterUiConnectionClosed();
	}
}
