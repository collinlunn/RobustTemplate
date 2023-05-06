using Content.Shared.UI;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Network;

namespace Content.Server.UI
{
	public sealed class ServerUiStateManager
	{
		[Dependency] private readonly IPlayerManager _players = default!;
		[Dependency] private readonly IServerNetManager _net = default!;

		/// <summary>
		///		Set of connected players with their set of active ui connections.
		/// </summary>
		private readonly Dictionary<IPlayerSession, PlayerUiData> _playerData = new();

		private sealed class PlayerUiData
		{
			public uint NextId = 1;
			public readonly Dictionary<uint, UiConnection> UiConnections = new();
		}

		/// <summary>
		///		Set of players with a dirty ui state to update, with the id of the ui to update.
		/// </summary>
		private readonly Queue<(IPlayerSession player, uint id)> _stateUpdateQueue = new ();

		public void Initialize()
		{
			_net.RegisterNetMessage<MsgUiState>(); //HandleUiMessage
			_players.PlayerStatusChanged += PlayerStatusChanged;
		}

		public UiConnection LoadUi(IPlayerSession player, UiState? state = null)
		{
			var initialState = state ?? new DummyUiState();

			var data = _playerData[player];
			var newId = data.NextId++;
			var ui = new UiConnection(newId, player, initialState);
			data.UiConnections.Add(newId, ui);

			SendMsgUi(ui, new LoadUiMessage(initialState));
			return ui;
		}

		public void UnloadUi(UiConnection ui)
		{
			_playerData[ui.Player].UiConnections.Remove(ui.Id);
			SendMsgUi(ui, new UnloadUiMessage());
		}

		public void DirtyUi(UiConnection ui, UiState newState)
		{
			ui.State = newState;
			if (!ui.Dirty)
			{
				ui.Dirty = true;
				QueueStateUpdate(ui);
			}
		}

		public void SendDirtyStates()
		{
			while (_stateUpdateQueue.TryDequeue(out var tuple))
			{
				var (player, id) = tuple;

				// Check that UI and player still exist.
				if (_playerData.TryGetValue(player, out var playerData) && playerData.UiConnections.TryGetValue(id, out var ui))
				{
					ui.Dirty = false;
					SendMsgUi(ui, new UiStateMessage(ui.State));
				}
			}
		}

		private void QueueStateUpdate(UiConnection ui)
		{
			_stateUpdateQueue.Enqueue((ui.Player, ui.Id));
		}

		//private void HandleUiMessage(MsgUi message)
		//{
		//	var id = message.Id;
		//	var uiMessage = message.Message;

		//	if (!_players.TryGetSessionByChannel(message.MsgChannel, out var player))
		//	{
		//		return;
		//	}
		//	if (!_playerData.TryGetValue(player, out var playerUiData))
		//	{
		//		return;
		//	}
		//	if (!playerUiData.LoadedUis.TryGetValue(id, out var ui))
		//	{
		//		Logger.Error($"Received a UI message from {player} for ID {id} but none existed.");
		//		return;
		//	}

		//	//Placeholder
		//}

		private void PlayerStatusChanged(object? sender, SessionStatusEventArgs e)
		{
			if (e.NewStatus == SessionStatus.Connected)
			{
				_playerData.Add(e.Session, new PlayerUiData());
			}
			else if (e.NewStatus == SessionStatus.Disconnected)
			{
				if (_playerData.TryGetValue(e.Session, out var playerData))
				{
					foreach (var ui in playerData.UiConnections.Values)
					{
						UnloadUi(ui);
					}

					_playerData.Remove(e.Session);
				}
			}
		}

		private void SendMsgUi(UiConnection ui, BaseUiStateMessage message)
		{
			var msgUi = new MsgUiState
			{
				Id = ui.Id,
				Message = message
			};
			_net.ServerSendMessage(msgUi, ui.Player.ConnectedClient);
		}
	}
}