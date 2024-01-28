using Content.Shared.UI;
using Robust.Shared.Network;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using static Robust.Shared.GameObjects.BoundUserInterfaceRegisterExt;

namespace Content.Client.UI
{
	public sealed class ClientUiStateManager : EntitySystem
	{
		[Dependency] private readonly IClientNetManager _net = default!;
		[Dependency] private readonly ILogManager _logMan = default!;
		private ISawmill _logger = default!;

		private readonly Dictionary<Enum, ClientUiConnection> _uiConnections = new();

		public override void Initialize()
		{
			base.Initialize();
			_logger = _logMan.GetSawmill("uiState.client");
			SubscribeNetworkEvent<OpenUiConnectionMessage>(OpenUiConnection);
			SubscribeNetworkEvent<CloseUiConnectionMessage>(CloseUiConnection);
			SubscribeNetworkEvent<StateUiConnectionMessage>(HandleState);
			_net.Disconnect += NetOnDisconnect;
		}

		public override void Shutdown()
		{
			base.Shutdown();
			_net.Disconnect -= NetOnDisconnect;
		}

		public bool TryAddSubscriber(IUiStateSubscriber subscriber)
		{
			var uiKey = subscriber.UiKey;

			if (!TryGetConnection(uiKey, out var connection, out _))
			{
				connection = new ClientUiConnection(uiKey);
				_uiConnections.Add(uiKey, connection);
			}
			if (connection.SubscriberExists(subscriber, out var errorMsg))
			{
				_logger.Error($"Failed to add subscriber {subscriber.GetType()}:\n{errorMsg}");
				return false;
			}
			connection.Subscribers.Add(subscriber);
			subscriber.SetState(connection.State, connection.Status);
			return true;
		}

		public bool TryRemoveSubscriber(IUiStateSubscriber subscriber)
		{
			var uiKey = subscriber.UiKey;

			if (!TryGetConnection(uiKey, out var connection, out var errorMsg) ||
				!connection.SubscriberExists(subscriber, out errorMsg))
			{
				_logger.Error($"Failed to remove subscriber {subscriber.GetType()}:\n{errorMsg}");
				return false;
			}
			connection.Subscribers.Remove(subscriber);
			return true;
		}

		private void OpenUiConnection(OpenUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;
			var state = msg.State;

			if (!TryGetConnection(uiKey, out var connection, out _))
			{
				connection = new ClientUiConnection(uiKey);
				_uiConnections.Add(uiKey, connection);
			}
			if (!connection.CanChangeStatusTo(UiConnectionStatus.Open, out var errorMsg))
			{
				_logger.Error($"Failed to open {nameof(ClientUiConnection)} for {uiKey.GetType()}:\n{errorMsg}");
			}
			connection.Status = UiConnectionStatus.Open;
			connection.State = state;
			SetSubscriberStates(connection);
		}

		private void CloseUiConnection(CloseUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;

			if (!TryGetConnection(uiKey, out var connection, out var errorMsg) ||
				!connection.CanChangeStatusTo(UiConnectionStatus.Closed, out errorMsg))
			{
				_logger.Error($"Failed to close {nameof(ClientUiConnection)} for {uiKey.GetType()}:\n{errorMsg}");
				return;
			}
			connection.Status = UiConnectionStatus.Closed;
			connection.State = new DefaultUiState();
			SetSubscriberStates(connection);
		}

		private void HandleState(StateUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;
			var state = msg.State;

			if (!TryGetConnection(uiKey, out var connection, out var errorMsg) ||
				!connection.CanReceiveState(out errorMsg))
			{
				_logger.Error($"Failed to receive {nameof(UiState)} for {uiKey.GetType()}:\n{errorMsg}");
				return;
			}
			connection.State = state;
			SetSubscriberStates(connection);
		}

		private void NetOnDisconnect(object? sender, NetDisconnectedArgs e)
		{
			foreach (var connection in _uiConnections.Values)
			{
				connection.Status = UiConnectionStatus.Closed;
				connection.State = new DefaultUiState();
				SetSubscriberStates(connection);
			}
		}

		[Pure]
		private bool TryGetConnection(Enum uiKey, [NotNullWhen(true)] out ClientUiConnection? connection, out string errorMsg)
		{
			errorMsg = $"{nameof(ClientUiConnection)} for {uiKey.GetType()} not present";
			return _uiConnections.TryGetValue(uiKey, out connection);
		}

		private void SetSubscriberStates(ClientUiConnection connection)
		{
			connection.Subscribers.ForEach(sub => sub.SetState(connection.State, connection.Status));
		}
	}
}
