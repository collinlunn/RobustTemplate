using Content.Shared.UI;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Utility;

namespace Content.Server.UI
{
	public sealed class ServerUiManager
	{
		[Dependency] private readonly IPlayerManager _players = default!;
		[Dependency] private readonly IServerNetManager _net = default!;

		private readonly Dictionary<IPlayerSession, PlayerUiData> _playerData = new();

		private sealed class PlayerUiData
		{
			public uint NextId = 1;
			public readonly Dictionary<uint, ServerStateUi> OpenUis = new();
		}

		private readonly Queue<(IPlayerSession player, uint id)> _stateUpdateQueue = new ();

		public void Initialize()
		{
			_net.RegisterNetMessage<MsgUi>(HandleUiMessage);
			_players.PlayerStatusChanged += PlayerStatusChanged;
		}

		public void OpenUi(ServerStateUi ui, IPlayerSession player)
		{
			if (ui.Id != ServerStateUi.PreInitId)
			{
				throw new ArgumentException($"Tried to open UI {ui.GetType().Name}, but it was already opened.");
			}

			var data = _playerData[player];
			var newId = data.NextId++;
			data.OpenUis.Add(newId, ui);

			ui.Id = newId;
			ui.Player = player;

			SendMsgUi(newId, new OpenUiMessage(ui.GetType().Name), player.ConnectedClient);
		}

		public void CloseUi(ServerStateUi ui)
		{
			var player = ui.Player;
			var id = ui.Id;

			_playerData[player].OpenUis.Remove(id);
			SendMsgUi(id, new CloseUiMessage(), player.ConnectedClient);
		}

		public void SendUiEvent(ServerStateUi ui, UiEventMessage uiEvent)
		{
			SendMsgUi(ui.Id, uiEvent, ui.Player.ConnectedClient);
		}

		public void DirtyUi(ServerStateUi ui)
		{
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
				if (_playerData.TryGetValue(player, out var playerData) && playerData.OpenUis.TryGetValue(id, out var ui))
				{
					ui.Dirty = false;
					var state = ui.GetNewState();
					SendMsgUi(id, state, player.ConnectedClient);
				}
			}
		}

		private void QueueStateUpdate(ServerStateUi ui)
		{
			DebugTools.Assert(ui.Id != ServerStateUi.PreInitId, "UI has not been opened yet.");
			_stateUpdateQueue.Enqueue((ui.Player, ui.Id));
		}

		private void HandleUiMessage(MsgUi message)
		{
			var id = message.Id;
			var uiMessage = message.Message;

			if (!_players.TryGetSessionByChannel(message.MsgChannel, out var player))
			{
				return;
			}
			if (!_playerData.TryGetValue(player, out var playerUiData))
			{
				return;
			}
			if (!playerUiData.OpenUis.TryGetValue(id, out var ui))
			{
				Logger.Error($"Received a UI message from {player} for ID {id} but none existed.");
				return;
			}

			switch (uiMessage)
			{
				case UiInputMessage input:
					ui.HandleInput(input);
					break;

				default:
					Logger.Error($"Received a UI message of an unhandled type: {message.GetType()}");
					break;
			}
		}

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
					foreach (var ui in playerData.OpenUis.Values)
					{
						CloseUi(ui);
					}

					_playerData.Remove(e.Session);
				}
			}
		}

		private void SendMsgUi(uint id, BaseUiMessage message, INetChannel client)
		{
			var msgUi = new MsgUi
			{
				Id = id,
				Message = message
			};
			_net.ServerSendMessage(msgUi, client);
		}
	}
}