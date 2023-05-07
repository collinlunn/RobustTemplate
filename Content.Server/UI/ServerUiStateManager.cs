using Content.Shared.UI;
using Robust.Server.Player;
using Robust.Shared.Enums;

namespace Content.Server.UI
{
	public sealed class ServerUiStateManager : EntitySystem
	{
		[Dependency] private readonly IPlayerManager _players = default!;

		/// <summary>
		///		Set of connected players with their set of active ui connections.
		/// </summary>
		private readonly Dictionary<IPlayerSession, PlayerUiConnections> _playerUiConnections = new();

		private sealed class PlayerUiConnections
		{
			public readonly Dictionary<Enum, UiConnection> UiConnections = new();
		}

		/// <summary>
		///		Set of players with a dirty ui state to update, with the key of the ui to update.
		/// </summary>
		private readonly Queue<(IPlayerSession player, Enum uiKey)> _stateUpdateQueue = new();

		public override void Initialize()
		{
			base.Initialize();
			_players.PlayerStatusChanged += PlayerStatusChanged;
		}

		public override void Update(float frameTime)
		{
			base.Update(frameTime);
			SendDirtyStates();
		}

		public UiConnection OpenUiConnection(Enum uiKey, IPlayerSession player, UiState? state = null)
		{
			if (state == null)
			{
				Logger.Debug($"Loaded UI {nameof(uiKey)} with dummy UI state.");
				state = new DummyUiState();
			}

			var data = _playerUiConnections[player];
			var ui = new UiConnection(uiKey, player, state);
			data.UiConnections.Add(uiKey, ui);

			RaiseNetworkEvent(new OpenUiConnectionMessage(uiKey, state), player);
			return ui;
		}

		public void CloseUiConnection(Enum uiKey, IPlayerSession player)
		{
			_playerUiConnections[player].UiConnections.Remove(uiKey);
			RaiseNetworkEvent(new CloseUiConnectionMessage(uiKey), player);
		}

		public void DirtyUiConnection(UiConnection ui, UiState newState)
		{
			ui.State = newState;
			if (!ui.Dirty)
			{
				ui.Dirty = true;
				_stateUpdateQueue.Enqueue((ui.Player, ui.UiKey));
			}
		}

		private void SendDirtyStates()
		{
			while (_stateUpdateQueue.TryDequeue(out var tuple))
			{
				var (player, uiKey) = tuple;

				// Check that UI and player still exist.
				if (!_playerUiConnections.TryGetValue(player, out var playerData))
				{
					continue;
				}
				if (!playerData.UiConnections.TryGetValue(uiKey, out var ui))
				{
					continue;
				}

				ui.Dirty = false;
				RaiseNetworkEvent(new StateUiConnectionMessage(uiKey, ui.State), player);
			}
		}

		private void PlayerStatusChanged(object? sender, SessionStatusEventArgs ev)
		{
			switch (ev.NewStatus)
			{
				case SessionStatus.Connected:
					_playerUiConnections.Add(ev.Session, new PlayerUiConnections());
					break;
				case SessionStatus.Disconnected:
					_playerUiConnections.Remove(ev.Session);
					break;
			}
		}
	}
}