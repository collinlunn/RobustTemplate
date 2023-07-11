using Content.Shared.UI;
using Robust.Shared.Utility;

namespace Content.Client.UI
{
	[Access(typeof(ClientUiStateManager))]
	public sealed class ClientUiConnection
	{
		public Enum UiKey { get; }

		private readonly List<IUiStateSubscriber> _subscribers = new();
		
		/// <summary>
		///		Tracks the status of the server's ui connection.
		/// </summary>
		private UiConnectionStatus Status = UiConnectionStatus.NotOpen;

		/// <summary>
		///		The last state sent by the server for this key. Null if no state is available.
		/// </summary>
		private UiState? State;

		public ClientUiConnection(Enum uiKey)
		{
			UiKey = uiKey;
		}

		public void ServerUiConnectionOpened(UiState state)
		{
			DebugTools.Assert(Status == UiConnectionStatus.NotOpen || Status == UiConnectionStatus.Closed);
			Status = UiConnectionStatus.Open;

			State = state;

			SetSubscriberStates();
		}

		public void ServerUiStateReceived(UiState state)
		{
			DebugTools.Assert(Status == UiConnectionStatus.Open);

			DebugTools.Assert(State != null);
			State = state;

			SetSubscriberStates();
		}

		public void ServerUiConnectionClosed()
		{
			DebugTools.Assert(Status == UiConnectionStatus.Open);
			Status = UiConnectionStatus.Closed;

			DebugTools.Assert(State != null);
			State = null;

			SetSubscriberStates();
		}

		public void OnNetDisconnected()
		{
			Status = UiConnectionStatus.Closed;
			State = null;
			SetSubscriberStates();
		}

		public void AddSubscriber(IUiStateSubscriber subscriber)
		{
			DebugTools.Assert(!_subscribers.Contains(subscriber));
			_subscribers.Add(subscriber);
			SetState(subscriber);
		}

		public void RemoveSubscriber(IUiStateSubscriber subscriber)
		{
			DebugTools.Assert(_subscribers.Contains(subscriber));
			_subscribers.Remove(subscriber);
		}

		private void SetState(IUiStateSubscriber subscriber)
		{
			DebugTools.Assert(_subscribers.Contains(subscriber));
			subscriber.SetState(State ?? subscriber.DefaultState, Status);
		}

		private void SetSubscriberStates()
		{
			foreach (var subscriber in _subscribers)
			{
				SetState(subscriber);
			}
		}
	}

	public enum UiConnectionStatus
	{
		NotOpen, //a client subscribed to this but no server state has arrived
		Open, //A server state was received
		Closed, //The server has discarded the state
	}
}
