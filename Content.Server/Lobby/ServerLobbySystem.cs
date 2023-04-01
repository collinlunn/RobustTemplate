using Content.Shared.Lobby;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Timing;
using System.Collections.Generic;

namespace Content.Server.Lobby;

[UsedImplicitly]
public sealed class ServerLobbySystem : SharedLobbySystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
	[Dependency] private readonly MapLoaderSystem _mapLoader = default!;

	private string _mapToLoad = "/Maps/test_map.yml";


	public override void Initialize()
    {
        base.Initialize();

        _playerManager.PlayerStatusChanged += PlayerStatusChanged;
        SubscribeNetworkEvent<StartGamePressedEvent>(OnStartGamePressed);
		SubscribeNetworkEvent<StartMappingPressedEvent>(OnStartMappingPressed);

	}

	public override void Update(float frameTime)
    {
        base.Update(frameTime);
    }

    private void PlayerStatusChanged(object? sender, SessionStatusEventArgs args)
    {
        var session = args.Session;

        switch (args.NewStatus)
        {
            case SessionStatus.Connected:
            {
                // Make the player actually join the game.
                // timer time must be > tick length
                Timer.Spawn(0, args.Session.JoinGame);
                break;
            }
            case SessionStatus.InGame:
            {
                RaiseNetworkEvent(new LobbyJoinedEvent(), session.ConnectedClient);
                break;
            }
        }
    }

    private void OnStartGamePressed(StartGamePressedEvent ev)
    {
        var startGameEvent = new StartGameEvent();
        RaiseLocalEvent(startGameEvent);

		var mapId = _mapManager.CreateMap();
		_mapLoader.TryLoad(mapId, _mapToLoad, out _);

		var spawnVector = new Vector2i(0, 0);
        var spawnCoord = new MapCoordinates(spawnVector, mapId);

        foreach (var playerSession in _playerManager.ServerSessions)
        {
            RaiseNetworkEvent(startGameEvent, playerSession);
            var playerEntity = EntityManager.SpawnEntity("TestPlayer", spawnCoord);
            playerSession.AttachToEntity(playerEntity);
        }
    }

	private void OnStartMappingPressed(StartMappingPressedEvent ev)
	{
		var startGameEvent = new StartGameEvent();
		RaiseLocalEvent(startGameEvent);

		var mapId = _mapManager.CreateMap();
		_mapManager.AddUninitializedMap(mapId); //set as uninitialized so map can be saved to a file correctly
		_mapLoader.TryLoad(mapId, _mapToLoad, out _);

		var spawnVector = new Vector2i(0, 0);
		var spawnCoord = new MapCoordinates(spawnVector, mapId);

		foreach (var playerSession in _playerManager.ServerSessions)
		{
			RaiseNetworkEvent(startGameEvent, playerSession);
			var playerEntity = EntityManager.SpawnEntity("TestMappingPlayer", spawnCoord);
			playerSession.AttachToEntity(playerEntity);
		}
	}

	private void SendLobbyHudState()
	{
		var state = GenerateLobbyHudState();
		foreach (var playerSession in _playerManager.ServerSessions)
		{
			RaiseNetworkEvent(state, playerSession);
		}
	}

	private LobbyUIStateEvent GenerateLobbyHudState()
	{
		var playerStates = new List<LobbyPlayerState>();
		foreach (var playerSession in _playerManager.ServerSessions)
		{
			var userName = playerSession.Data.UserName;
			var ready = false; //TODO
			var playerState = new LobbyPlayerState(userName, ready);
			playerStates.Add(playerState);
		}
		var uiStateEvent = new LobbyUIStateEvent(playerStates);
		return uiStateEvent;
	}
}