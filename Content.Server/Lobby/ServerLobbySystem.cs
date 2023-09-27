using Content.Server.Spawnpoint;
using Content.Server.UI;
using Content.Shared.Lobby;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Map;
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

	private string _mapToLoad = "/Maps/default_map.yml";
	private bool _gameStarted = false;

	private const string PlayerProto = "TestPlayer";
	private const string PlayerMappingProto = "TestMappingPlayer";

	private HashSet<IPlayerSession> _playersInLobby = new();

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
				Timer.Spawn(0, args.Session.JoinGame);
				break;

			case SessionStatus.InGame:
				_playersInLobby.Add(session);
				RaiseNetworkEvent(new LobbyJoinedEvent(), session);
				_uiState.OpenUiConnection(LobbyUiKey.Key, session, UiState());
				DirtyUi();
				break;

			case SessionStatus.Disconnected:
				if (_playersInLobby.Contains(session))
				{
					_playersInLobby.Remove(session);
				}
				DirtyUi();
				break;
		}
    }

	[Pure]
	private LobbyUiState UiState()
	{
		var connectedPlayers = _playerManager.ServerSessions
			.Where(session => session.Status == SessionStatus.InGame)
			.Select(session => session.Name)
			.ToArray();

		var uiState = new LobbyUiState(connectedPlayers, _gameStarted);
		return uiState;
	}

	private void DirtyUi()
	{
		var uiState = UiState();
		foreach (var player in _playersInLobby)
		{
			_uiState.DirtyUiState(LobbyUiKey.Key, player, uiState);
		}
	}

	private void OnStartGamePressed(StartGameButtonPressed message)
    {
		if (_gameStarted)
		{
			Log.Debug("Client tried to start game, but it has already started.");
			return;
		}
		_gameStarted = true;

		var mapId = _mapManager.CreateMap();
		_mapLoader.TryLoad(mapId, _mapToLoad, out _);

		foreach (var playerSession in _playerManager.ServerSessions)
		{
			SpawnPlayer(playerSession);
		}
	}

	private void OnStartMappingPressed(StartMappingButtonPressed message)
	{
		if (_gameStarted)
		{
			Log.Debug("Client tried to start mapping, but it has already started.");
			return;
		}
		_gameStarted = true;

		var mapId = _mapManager.CreateMap();
		_mapManager.AddUninitializedMap(mapId); //set as uninitialized so map can be saved to a file correctly
		_mapLoader.TryLoad(mapId, _mapToLoad, out _);

		foreach (var playerSession in _playerManager.ServerSessions)
		{
			SpawnPlayerMapping(playerSession, mapId);
		}
	}

	private void OnJoinGamePressed(JoinGameButtonPressed message, EntitySessionEventArgs args)
	{
		if (!_gameStarted)
		{
			Log.Debug("Client tried to join game, but it has not started yet.");
			return;
		}

		if (args.SenderSession is not IPlayerSession session) //TODO: this cast is hacky, how to do properly? Can I replace w/ CommonSession?
		{
			Log.Error("Received wrong type of session in OnJoinGamePressed");
			return;
		}

		SpawnPlayer(session);
	}

	private void SpawnPlayer(IPlayerSession playerSession)
	{
		var spawnEvent = new SpawnPlayerEvent(PlayerProto, playerSession);
		RaiseLocalEvent(spawnEvent);
		PlayerSpawned(playerSession);
	}

	private void SpawnPlayerMapping(IPlayerSession playerSession, MapId mapId)
	{
		var spawnEvent = new SpawnMappingPlayerEvent(PlayerMappingProto, mapId, playerSession);
		RaiseLocalEvent(spawnEvent);
		PlayerSpawned(playerSession);
	}

	private void PlayerSpawned(IPlayerSession playerSession)
	{
		_playersInLobby.Remove(playerSession);

		var gameStartedEvent = new GameStartedEvent();
		RaiseNetworkEvent(gameStartedEvent, playerSession);

		_uiState.CloseUiConnection(LobbyUiKey.Key, playerSession);
		DirtyUi();
	}
}