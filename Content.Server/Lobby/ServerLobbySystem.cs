using Content.Server.Spawnpoint;
using Content.Server.UI;
using Content.Shared.Lobby;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Timing;

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

	public override void Initialize()
    {
        base.Initialize();
        _playerManager.PlayerStatusChanged += PlayerStatusChanged;
		SubscribeNetworkEvent<StartGameButtonPressed>(OnStartGamePressed);
		SubscribeNetworkEvent<StartMappingButtonPressed>(OnStartMappingPressed);
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
				RaiseNetworkEvent(new LobbyJoinedEvent(), session);
				_uiState.OpenUiConnection(LobbyUiKey.Key, session, new LobbyUiState(1));
				break;
		}
    }

    public void OnStartGamePressed(StartGameButtonPressed message)
    {
		if (_gameStarted)
			return;
		_gameStarted = true;

		var mapId = _mapManager.CreateMap();
		_mapLoader.TryLoad(mapId, _mapToLoad, out _);

		SpawnPlayers("TestPlayer");
    }

	private void SpawnPlayers(string playerProto)
	{
		var gameStartedEvent = new GameStartedEvent();

		foreach (var playerSession in _playerManager.ServerSessions)
		{
			var spawnEvent = new SpawnPlayerEvent(playerProto, playerSession);
			RaiseLocalEvent(spawnEvent);
			RaiseNetworkEvent(gameStartedEvent, playerSession);
			_uiState.CloseUiConnection(LobbyUiKey.Key, playerSession);
		}
	}

	public void OnStartMappingPressed(StartMappingButtonPressed message)
	{
		if (_gameStarted)
			return;
		_gameStarted = true;

		var mapId = _mapManager.CreateMap();
		_mapManager.AddUninitializedMap(mapId); //set as uninitialized so map can be saved to a file correctly
		_mapLoader.TryLoad(mapId, _mapToLoad, out _);

		SpawnMappingPlayers("TestMappingPlayer", mapId);
	}

	private void SpawnMappingPlayers(string playerProto, MapId mapId)
	{
		var gameStartedEvent = new GameStartedEvent();

		foreach (var playerSession in _playerManager.ServerSessions)
		{
			var spawnEvent = new SpawnMappingPlayerEvent(playerProto, mapId, playerSession);
			RaiseLocalEvent(spawnEvent);
			RaiseNetworkEvent(gameStartedEvent, playerSession);
			_uiState.CloseUiConnection(LobbyUiKey.Key, playerSession);
		}
	}
}