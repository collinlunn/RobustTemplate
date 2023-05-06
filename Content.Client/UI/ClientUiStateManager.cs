using Content.Shared.UI;
using Robust.Shared.Network;

namespace Content.Client.UI
{
	public sealed class ClientUiStateManager
	{
		[Dependency] private readonly IClientNetManager _net = default!;

		private readonly Dictionary<uint, UiState> _uiStates = new();

		public void Initialize()
		{
			_net.RegisterNetMessage<MsgUiState>(HandleUiMessage);
			_net.Disconnect += NetOnDisconnect;
		}

		private void HandleUiMessage(MsgUiState message)
		{
			var id = message.Id;
			var uiMessage = message.Message;

			switch (uiMessage)
			{
				case LoadUiMessage loadUi:
					LoadUi(id, loadUi.State);
					break;

				case UnloadUiMessage:
					UnloadUi(id);
					break;

				case UiStateMessage uiState:
					HandleState(id, uiState.State);
					break;

				default:
					Logger.Error($"Received a UI message of an unhandled type: {message.GetType()}");
					break;
			}
		}

		private void NetOnDisconnect(object? sender, NetDisconnectedArgs e)
		{
			foreach (var tuple in _uiStates)
			{
				IoCManager.Resolve<EntityManager>().EventBus.RaiseEvent(EventSource.Local, new UiConnectionUnloadedEvent());
			}
			_uiStates.Clear();
		}

		private void LoadUi(uint id, UiState initialState)
		{
			if (!_uiStates.TryGetValue(id, out var loadedUi))
			{
				_uiStates.Add(id, initialState);
				IoCManager.Resolve<EntityManager>().EventBus.RaiseEvent(EventSource.Local, new UiConnectionLoadedEvent(initialState));
			}
			else
			{
				Logger.Error($"Tried to load a UI ({initialState.GetType()}) but ID ({id}) was already in use by {loadedUi.GetType()}.");
			}
		}

		private void UnloadUi(uint id)
		{
			if (IdHasConnection(id))
			{
				IoCManager.Resolve<EntityManager>().EventBus.RaiseEvent(EventSource.Local, new UiConnectionUnloadedEvent());
				_uiStates.Remove(id);
			}
		}

		private void HandleState(uint id, UiState uiState)
		{
			if (IdHasConnection(id))
			{
				_uiStates[id] = uiState;
				IoCManager.Resolve<EntityManager>().EventBus.RaiseEvent(EventSource.Local, new UiStateReceivedEvent(uiState));
			}
		}

		private bool IdHasConnection(uint id)
		{
			var hasConnection = _uiStates.ContainsKey(id);

			if (!hasConnection)
			{
				Logger.Error($"Tried to get UI at ID: {id} but none existed.");
			}

			return hasConnection;
		}

		//private void SendMsgUi(uint id, BaseUiMessage message)
		//{
		//	var msgUi = new MsgUi
		//	{
		//		Id = id,
		//		Message = message
		//	};
		//	_net.ClientSendMessage(msgUi);
		//}
	}
}
