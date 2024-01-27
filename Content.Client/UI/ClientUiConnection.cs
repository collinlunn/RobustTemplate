using Content.Shared.UI;
using Robust.Shared.Utility;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Content.Client.UI
{
	[Access(typeof(ClientUiStateManager))]
	public sealed class ClientUiConnection
	{
		public readonly Enum UiKey;

		public readonly List<IUiStateSubscriber> Subscribers = new();
		
		/// <summary>
		///		Tracks the status of the server's ui connection.
		/// </summary>
		public UiConnectionStatus Status = UiConnectionStatus.NotOpen;

		/// <summary>
		///		The last state sent by the server for this key.
		/// </summary>
		public UiState State = new DefaultUiState();

		public ClientUiConnection(Enum uiKey)
		{
			UiKey = uiKey;
		}

		[Pure]
		public bool SubscriberExists(IUiStateSubscriber subscriber, out string errorMsg)
		{
			var exists = Subscribers.Contains(subscriber);
			errorMsg = $"{nameof(IUiStateSubscriber)} {subscriber.GetType()} {(exists ? "is already" : "is not")}" +
				$" subscribed to {Name()}";
			return exists;
		}

		[Pure]
		public bool CanReceiveState(out string errorMsg)
		{
			errorMsg = $"{Name()} cannot receive a {nameof(UiState)}";
			return Status == UiConnectionStatus.Open;
		}

		[Pure]
		public bool CanChangeStatusTo(UiConnectionStatus newStatus, out string errorMsg)
		{
			errorMsg = $"Cannot change {Name()} from {nameof(UiConnectionStatus)} {Status} to {newStatus}";
			var allowedNewStatuses = Status switch
			{
				UiConnectionStatus.NotOpen => new UiConnectionStatus[] { UiConnectionStatus.Open },
				UiConnectionStatus.Open => new UiConnectionStatus[] { UiConnectionStatus.Closed },
				UiConnectionStatus.Closed => new UiConnectionStatus[] { UiConnectionStatus.Open },
				_ => throw new NotImplementedException(),
			};
			return allowedNewStatuses.Contains(newStatus);
		}

		[Pure]
		private string Name()
		{
			return $"{nameof(ClientUiConnection)} (Key:{UiKey.GetType()}, Status:{Status})";
		}
	}

	public enum UiConnectionStatus
	{
		NotOpen, //a client subscribed to this but no server state has arrived
		Open, //A server state was received
		Closed, //The server has discarded the state
	}

	/// <summary>
	///		Ui state to default to when server has not provided any.
	/// </summary>
	public sealed class DefaultUiState : UiState
	{

	}
}
