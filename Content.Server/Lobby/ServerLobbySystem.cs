using Content.Shared.Lobby;
using JetBrains.Annotations;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Timing;

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
        foreach (var playerSession in _playerManager.ServerSessions)
        {
            RaiseNetworkEvent(startGameEvent, playerSession);

            // Empty entity
            var spawnVector = new Vector2i(20, 10) / 2f;
            var spawnCoord = new MapCoordinates(spawnVector, _map);
            var entity = EntityManager.SpawnEntity(null, spawnCoord);
            playerSession.AttachToEntity(entity);
        }
    }
}