using Content.Shared.UI;
using Robust.Shared.Network;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.UI
{
	public sealed class ClientUiStateManager : EntitySystem
	{
		[Dependency] private readonly IClientNetManager _net = default!;

		private readonly Dictionary<Enum, ClientUiConnection> _uiConnections = new();

		public override void Initialize()
		{
			base.Initialize();
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

		public void AddSubscriber(IUiStateSubscriber subscriber)
		{
			var uiKey = subscriber.UiKey;

			if (_uiConnections.TryGetValue(uiKey, out var connection))
			{
				connection.Subscribers.Add(subscriber);
				//send current state? notify of open?
			}
			else
			{
				var newConnection = new ClientUiConnection(uiKey);
				_uiConnections.Add(uiKey, newConnection);
				newConnection.Subscribers.Add(subscriber);
				//send placeholder state?
			}
		}

		private void OpenUiConnection(OpenUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;
			var state = msg.State;

			if (_uiConnections.TryGetValue(uiKey, out var connection))
			{
				connection.ServerUiConnectionOpened(state);
			}
			else
			{
				var newConnection = new ClientUiConnection(uiKey);
				_uiConnections.Add(uiKey, newConnection);
				newConnection.ServerUiConnectionOpened(state);
			}
		}

		private void CloseUiConnection(CloseUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;

			UiConnection(uiKey).ServerUiConnectionClosed();

			var keyFound = _uiConnections.Remove(uiKey);
			DebugTools.Assert(keyFound, $"Tried to remove connection for {nameof(uiKey)}, but none existed.");
		}

		private void HandleState(StateUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;
			var state = msg.State;

			UiConnection(uiKey).ServerUiStateReceived(state);
		}

		private void NetOnDisconnect(object? sender, NetDisconnectedArgs e)
		{
			foreach (var connection in _uiConnections.Values)
			{
				connection.ServerUiConnectionClosed();
			}
			_uiConnections.Clear();
		}

		private ClientUiConnection UiConnection(Enum uiKey)
		{
			DebugTools.Assert(_uiConnections.ContainsKey(uiKey),
				$"Tried to get UI connection with key: {nameof(uiKey)} but none existed.");

			return _uiConnections[uiKey];
		}
	}
}
