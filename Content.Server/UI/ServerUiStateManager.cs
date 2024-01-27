using Content.Shared.UI;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Content.Server.UI
{
	public sealed class ServerUiStateManager : EntitySystem
	{
		[Dependency] private readonly IPlayerManager _players = default!;
		[Dependency] private readonly ILogManager _logMan = default!;
		private ISawmill _logger = default!;

		/// <summary>
		///		Set of connected players with their set of active ui connections.
		/// </summary>
		private readonly Dictionary<ICommonSession, ConnectionSet> _playerUiConnectionSets = new();

		private sealed class ConnectionSet : Dictionary<Enum, ServerUiConnection> { };

		/// <summary>
		///		Set of players with a dirty ui state to update, with the key of the ui to update.
		/// </summary>
		private readonly Queue<(ICommonSession player, Enum uiKey)> _stateUpdateQueue = new();

		public override void Initialize()
		{
			base.Initialize();
			_logger = _logMan.GetSawmill("uiState.server");
			_players.PlayerStatusChanged += PlayerStatusChanged;
		}

		public override void Shutdown()
		{
			base.Shutdown();
			_players.PlayerStatusChanged -= PlayerStatusChanged;
		}

		private void PlayerStatusChanged(object? sender, SessionStatusEventArgs ev)
		{
			switch (ev.NewStatus)
			{
				case SessionStatus.Connected:
					_playerUiConnectionSets.Add(ev.Session, new());
					break;
				case SessionStatus.Disconnected:
					_playerUiConnectionSets.Remove(ev.Session);
					break;
			}
		}

		public override void Update(float frameTime)
		{
			base.Update(frameTime);
			SendDirtyStates();

			void SendDirtyStates()
			{
				while (_stateUpdateQueue.TryDequeue(out var tuple))
				{
					var (player, uiKey) = tuple;

					// Check that UI and player still exist.
					if (!TryGetConnection(player, uiKey, out var ui, out _))
						continue;

					ui.Dirty = false;
					RaiseNetworkEvent(new StateUiConnectionMessage(uiKey, ui.State), player);
				}
			}
		}

		/// <summary>
		///		Sends the inital UI state to a client.
		/// </summary>
		/// <param name="uiKey">Determine which UI gets the state.</param>
		/// <param name="player">Player to send the state to.</param>
		/// <param name="state">State to send to the player. If null sends empty dummy state.</param>
		public bool TryOpenUiConnection(Enum uiKey, ICommonSession player, UiState? state = null)
		{
			state ??= new DummyUiState();

			if (!TryGetConnectionSet(player, out var connectionSet, out var errorMsg) ||
				ConnectionExists(connectionSet, uiKey, out errorMsg))
			{
				_logger.Error($"Failed to open {nameof(ConnectionSet)}:\n{errorMsg}");
				return false;
			}

			var ui = new ServerUiConnection(uiKey, state);
			connectionSet.Add(uiKey, ui);

			RaiseNetworkEvent(new OpenUiConnectionMessage(uiKey, state), player);
			return true;
		}

		/// <summary>
		///		Makes a client discard a UI state.
		/// </summary>
		/// <param name="uiKey">Determine which UI discards its state.</param>
		/// <param name="player">Player to make discard.</param>
		public bool TryCloseUiConnection(Enum uiKey, ICommonSession player)
		{
			if (!TryGetConnectionSet(player, out var connectionSet, out var errorMsg) ||
				!ConnectionExists(connectionSet, uiKey, out errorMsg))
			{
				_logger.Error($"Failed to close {nameof(ConnectionSet)}:\n{errorMsg}");

				return false;
			}

			connectionSet.Remove(uiKey);
			RaiseNetworkEvent(new CloseUiConnectionMessage(uiKey), player);
			return true;
		}

		public bool TryDirtyUiState(Enum uiKey, ICommonSession player, UiState newState)
		{
			if (!TryGetConnection(player, uiKey, out var ui, out var errorMsg))
			{
				_logger.Error($"Failed to dirty {nameof(ConnectionSet)}:\n{errorMsg}");
				return false;
			}

			ui.State = newState;
			if (!ui.Dirty)
			{
				ui.Dirty = true;
				_stateUpdateQueue.Enqueue((player, ui.UiKey));
			}
			return true;
		}

		[Pure]
		private bool TryGetConnectionSet(
			ICommonSession player,
			[NotNullWhen(true)] out ConnectionSet? connections,
			out string errorMsg)
		{
			errorMsg = $"{nameof(ConnectionSet)} not present for {player}";
			return _playerUiConnectionSets.TryGetValue(player, out connections);
		}

		[Pure]
		private static bool ConnectionExists(
			Dictionary<Enum, ServerUiConnection> connectionSet,
			Enum uiKey,
			out string errorMsg)
		{
			var present = connectionSet.ContainsKey(uiKey);
			errorMsg = $"{nameof(ServerUiConnection)} is {(present ? "already" : "not")} present for {uiKey.GetType()}";
			return present;
		}

		[Pure]
		private bool TryGetConnection(
			ICommonSession player,
			Enum uiKey,
			[NotNullWhen(true)] out ServerUiConnection? connection,
			out string errorMsg)
		{
			connection = null;
			if (!TryGetConnectionSet(player, out var connectionSet, out errorMsg))
				return false;

			errorMsg = $"{nameof(ServerUiConnection)} not present for {uiKey.GetType()}";
			return connectionSet.TryGetValue(uiKey, out connection);
		}
	}
}