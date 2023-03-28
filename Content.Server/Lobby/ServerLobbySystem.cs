using Content.Shared.Lobby;
using JetBrains.Annotations;
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

    private MapId _map = MapId.Nullspace;

    public override void Initialize()
    {
        base.Initialize();

        _playerManager.PlayerStatusChanged += PlayerStatusChanged;
        SubscribeNetworkEvent<StartGamePressedEvent>(OnStartGamePressed);
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
        //TODO Check if game should be allowed to start
        //TODO Start the game
        var startGameEvent = new StartGameEvent();
        RaiseLocalEvent(startGameEvent);

        _map = _mapManager.CreateMap();
        //_mapManager.SetMapPaused(_map, true);

        var spawnVector = new Vector2i(0, 0);
        var spawnCoord = new MapCoordinates(spawnVector, _map);

        foreach (var playerSession in _playerManager.ServerSessions)
        {
            RaiseNetworkEvent(startGameEvent, playerSession);
            var playerEntity = EntityManager.SpawnEntity("TestPlayer", spawnCoord);
            playerSession.AttachToEntity(playerEntity);
        }

        EntityManager.SpawnEntity("TestEntity", spawnCoord);
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