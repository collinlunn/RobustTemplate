using Content.Shared.UI;
using Robust.Shared.Utility;

namespace Content.Client.UI
{
	[Access(typeof(ClientUiStateManager))]
	public sealed class ClientUiConnection
	{
		public Enum UiKey { get; }

		public readonly List<IUiStateSubscriber> Subscribers = new();
		
		/// <summary>
		///		Tracks the status of the server's connection.
		///		For error-catching.
		/// </summary>
		private ServerConnectionStatus Status = ServerConnectionStatus.NotOpen;

		/// <summary>
		///		The last state sent by the server for this key. Null if no state is available.
		///		For error-catching.
		/// </summary>
		private UiState? LatestState;

		public ClientUiConnection(Enum uiKey)
		{
			UiKey = uiKey;
		}

		public void ServerUiConnectionOpened(UiState state)
		{
			//We are either opening or reopening the connection.
			DebugTools.Assert(Status == ServerConnectionStatus.NotOpen || Status == ServerConnectionStatus.Closed);
			LatestState = state;

			Status = ServerConnectionStatus.Open;

			foreach (var subscriber in Subscribers)
			{
				subscriber.SetState(state);
				subscriber.AfterUiConnectionOpened();
			}
		}

		public void ServerUiStateReceived(UiState state)
		{
			//Should already be open and have a state from the opening
			DebugTools.Assert(Status == ServerConnectionStatus.Open);
			DebugTools.Assert(LatestState != null);
			LatestState = state;

			foreach (var subscriber in Subscribers)
			{
				subscriber.SetState(state);
			}
		}

		public void ServerUiConnectionClosed()
		{
			//Should only be able to close if we are already open
			DebugTools.Assert(Status == ServerConnectionStatus.Open);
			DebugTools.Assert(LatestState != null);
			LatestState = null;

			Status = ServerConnectionStatus.Closed;

			foreach (var subscriber in Subscribers)
			{
				subscriber.SetState(subscriber.DefaultState);
				subscriber.AfterUiConnectionClosed();
			}
		}

		private enum ServerConnectionStatus
		{
			NotOpen, //Either this connection was just constructed, or a client opened this connecton pending a server state.
			Open, //A server state was received
			Closed, //The server has discarded the state
		}
	}
}
