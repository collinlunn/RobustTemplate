using Content.Shared.UI;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.UI
{
	public sealed class ServerUiStateManager : EntitySystem
	{
		[Dependency] private readonly IPlayerManager _players = default!;

		/// <summary>
		///		Set of connected players with their set of active ui connections.
		/// </summary>
		private readonly Dictionary<ICommonSession, PlayerUiConnectionSet> _playerUiConnectionSets = new();

		private sealed class PlayerUiConnectionSet
		{
			public readonly Dictionary<Enum, ServerUiConnection> UiConnections = new();
		}

		/// <summary>
		///		Set of players with a dirty ui state to update, with the key of the ui to update.
		/// </summary>
		private readonly Queue<(ICommonSession player, Enum uiKey)> _stateUpdateQueue = new();

		public override void Initialize()
		{
			base.Initialize();
			_players.PlayerStatusChanged += PlayerStatusChanged;
		}

		public override void Shutdown()
		{
			base.Shutdown();
			_players.PlayerStatusChanged -= PlayerStatusChanged;
		}

		public override void Update(float frameTime)
		{
			base.Update(frameTime);
			SendDirtyStates();
		}

		/// <summary>
		///		Sends the inital UI state to a client.
		/// </summary>
		/// <param name="uiKey">Determine which UI gets the state.</param>
		/// <param name="player">Player to send the state to.</param>
		/// <param name="state">State to send to the player. If null sends empty dummy state.</param>
		public void OpenUiConnection(Enum uiKey, ICommonSession player, UiState? state = null)
		{
			if (state == null)
			{
				Log.Debug($"Loaded UI {nameof(uiKey)} with dummy UI state.");
				state = new DummyUiState();
			}
			DebugTools.Assert(_playerUiConnectionSets.ContainsKey(player),
				$"Tried to open UI connection for {player} but they were not in list of players.");

			var connectionSet = _playerUiConnectionSets[player].UiConnections;

			DebugTools.Assert(!connectionSet.ContainsKey(uiKey),
				$"Tried to open UI connection for {player} but {nameof(uiKey)} was already in use.");

			var ui = new ServerUiConnection(uiKey, state);
			connectionSet.Add(uiKey, ui);

			RaiseNetworkEvent(new OpenUiConnectionMessage(uiKey, state), player);
		}

		/// <summary>
		///		Makes a client discard a UI state.
		/// </summary>
		/// <param name="uiKey">Determine which UI discards its state.</param>
		/// <param name="player">Player to make discard.</param>
		public void CloseUiConnection(Enum uiKey, ICommonSession player)
		{
			DebugTools.Assert(_playerUiConnectionSets.ContainsKey(player),
				$"Tried to close UI connection for {player} but they were not in list of players.");

			var uiConnections = _playerUiConnectionSets[player].UiConnections;

			DebugTools.Assert(uiConnections.ContainsKey(uiKey),
				$"Tried to close UI connection for {player} but but none existed for {nameof(uiKey)}.");

			uiConnections.Remove(uiKey);
			RaiseNetworkEvent(new CloseUiConnectionMessage(uiKey), player);
		}

		public void DirtyUiState(Enum uiKey, ICommonSession player, UiState newState)
		{
			DebugTools.Assert(_playerUiConnectionSets.ContainsKey(player),
				$"Tried to dirty UI state  for {player} but they were not in list of players.");

			var uiConnections = _playerUiConnectionSets[player].UiConnections;

			DebugTools.Assert(uiConnections.ContainsKey(uiKey),
				$"Tried to dirty UI state for {player} but but none existed for {nameof(uiKey)}.");

			var ui = uiConnections[uiKey];
			ui.State = newState;
			if (!ui.Dirty)
			{
				ui.Dirty = true;
				_stateUpdateQueue.Enqueue((player, ui.UiKey));
			}
		}

		/// <summary>
		///		Sends states to all clients with a dirty UI state.
		/// </summary>
		private void SendDirtyStates()
		{
			while (_stateUpdateQueue.TryDequeue(out var tuple))
			{
				var (player, uiKey) = tuple;

				// Check that UI and player still exist.
				if (!_playerUiConnectionSets.TryGetValue(player, out var playerData) ||
					!playerData.UiConnections.TryGetValue(uiKey, out var ui))
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
					_playerUiConnectionSets.Add(ev.Session, new PlayerUiConnectionSet());
					break;
				case SessionStatus.Disconnected:
					_playerUiConnectionSets.Remove(ev.Session);
					break;
			}
		}
	}
}