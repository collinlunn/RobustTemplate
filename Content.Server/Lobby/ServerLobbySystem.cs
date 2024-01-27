using Content.Server.Spawnpoint;
using Content.Server.UI;
using Content.Shared.Lobby;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using System.Linq;

namespace Content.Server.Lobby;

[UsedImplicitly]
public sealed class ServerLobbySystem : SharedLobbySystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
	[Dependency] private readonly MapLoaderSystem _mapLoader = default!;
	[Dependency] private readonly ServerUiStateManager _uiState = default!;

	private const string DefaultMap = "/Maps/default_map.yml";
	private const string PlayerProto = "TestPlayer";
	private const string PlayerMappingProto = "TestMappingPlayer";

	private LobbyStatus _lobbyStatus = LobbyStatus.GameNotStarted;
	private readonly HashSet<ICommonSession> _playersInLobby = new();

	public override void Initialize()
    {
        base.Initialize();
        _playerManager.PlayerStatusChanged += PlayerStatusChanged;
		SubscribeNetworkEvent<StartGameButtonPressed>(OnStartGamePressed);
		SubscribeNetworkEvent<StartMappingButtonPressed>(OnStartMappingPressed);
		SubscribeNetworkEvent<JoinGameButtonPressed>(OnJoinGamePressed);
	}

	private void PlayerStatusChanged(object? sender, SessionStatusEventArgs args)
    {
        var session = args.Session;

        switch (args.NewStatus)
        {
            case SessionStatus.Connected:
				// Make the player actually join the game.
				// timer time must be > tick length
				Timer.Spawn(0, () => _playerManager.JoinGame(args.Session));
				break;

			case SessionStatus.InGame:
				_playersInLobby.Add(session);
				RaiseNetworkEvent(new LobbyJoinedEvent(), session);
				_uiState.TryOpenUiConnection(LobbyUiKey.Key, session, UiState());
				DirtyUi();
				break;

			case SessionStatus.Disconnected:
				_playersInLobby.Remove(session);
				DirtyUi();
				break;
		}
    }

	private void OnStartGamePressed(StartGameButtonPressed message, EntitySessionEventArgs args)
    {
		if (!GameCanStart(out var errorMsg))
		{
			Log.Error($"{args.SenderSession} cannot start game: {errorMsg}");
			return;
		}
		_lobbyStatus = LobbyStatus.GameStarted;

		var mapId = _mapManager.CreateMap();
		_mapLoader.TryLoad(mapId, DefaultMap, out _);

		_playerManager.Sessions.ForEach(SpawnPlayer);
	}

	private void OnStartMappingPressed(StartMappingButtonPressed message, EntitySessionEventArgs args)
	{
		if (!GameCanStart(out var errorMsg))
		{
			Log.Error($"{args.SenderSession} cannot start mapping: {errorMsg}");
			return;
		}
		_lobbyStatus = LobbyStatus.MappingStarted;

		var mapId = _mapManager.CreateMap();
		_mapManager.AddUninitializedMap(mapId); //set as uninitialized so map can be saved to a file correctly
		_mapLoader.TryLoad(mapId, DefaultMap, out _);

		_playerManager.Sessions.ForEach(session => SpawnPlayerMapping(session, mapId));
	}

	private void OnJoinGamePressed(JoinGameButtonPressed message, EntitySessionEventArgs args)
	{
		var player = args.SenderSession;

		if (!GameCanStart(out var errorMsg))
		{
			Log.Error($"{player} cannot join game:{errorMsg}");
			return;
		}
		SpawnPlayer(player);
	}

	private void SpawnPlayer(ICommonSession playerSession)
	{
		_playersInLobby.Remove(playerSession);

		RaiseLocalEvent(new SpawnPlayerEvent(PlayerProto, playerSession));
		RaiseNetworkEvent(new GameStartedEvent(), playerSession);
		_uiState.TryCloseUiConnection(LobbyUiKey.Key, playerSession);
		DirtyUi();
	}

	private void SpawnPlayerMapping(ICommonSession playerSession, MapId mapId)
	{
		_playersInLobby.Remove(playerSession);

		RaiseLocalEvent(new SpawnMappingPlayerEvent(PlayerMappingProto, mapId, playerSession));
		RaiseNetworkEvent(new GameStartedEvent(), playerSession);
		_uiState.TryCloseUiConnection(LobbyUiKey.Key, playerSession);
		DirtyUi();
	}

	private void DirtyUi()
	{
		var state = UiState();
		_playersInLobby.ForEach(player => _uiState.TryDirtyUiState(LobbyUiKey.Key, player, state));
	}

	[Pure]
	private bool GameCanStart(out string errorMsg)
	{
		errorMsg = $"Lobby cannot start; {nameof(ServerLobbySystem)} is not in state {LobbyStatus.GameNotStarted}";
		return _lobbyStatus == LobbyStatus.GameNotStarted;
	}

	[Pure]
	private bool GameCanJoin(out string errorMsg)
	{
		errorMsg = $"Cannot join; {nameof(ServerLobbySystem)} is still in state {LobbyStatus.GameNotStarted}";
		return _lobbyStatus == LobbyStatus.GameNotStarted;
	}


	[Pure]
	private LobbyUiState UiState()
	{
		var connectedPlayers = _playerManager.Sessions
			.Where(session => session.Status == SessionStatus.InGame)
			.Select(session => session.Name)
			.ToArray();

		var uiState = new LobbyUiState(connectedPlayers, _lobbyStatus);
		return uiState;
	}
}