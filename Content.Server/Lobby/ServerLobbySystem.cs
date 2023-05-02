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
	[Dependency] private readonly ServerUiManager _uiManager = default!;

	private string _mapToLoad = "/Maps/default_map.yml";
	private readonly List<LobbyStateUi> _lobbyUis = new();

	public override void Initialize()
    {
        base.Initialize();
        _playerManager.PlayerStatusChanged += PlayerStatusChanged;
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
				var ui = new LobbyStateUi();
				ui.Open(session);
				_lobbyUis.Add(ui);
				break;
		}
    }

	[Access(typeof(LobbyStateUi))]
    public void OnStartGamePressed()
    {
        var gameStartedEvent = new GameStartedEvent();
        RaiseLocalEvent(gameStartedEvent);

		var mapId = _mapManager.CreateMap();
		_mapLoader.TryLoad(mapId, _mapToLoad, out _);

		var spawnVector = new Vector2i(0, 0);
        var spawnCoord = new MapCoordinates(spawnVector, mapId);
		SpawnPlayers(gameStartedEvent, "TestPlayer", spawnCoord);

    }

	[Access(typeof(LobbyStateUi))]
	public void OnStartMappingPressed()
	{
		var gameStartedEvent = new GameStartedEvent();
		RaiseLocalEvent(gameStartedEvent);

		var mapId = _mapManager.CreateMap();
		_mapManager.AddUninitializedMap(mapId); //set as uninitialized so map can be saved to a file correctly
		_mapLoader.TryLoad(mapId, _mapToLoad, out _);

		var spawnVector = new Vector2i(0, 0);
		var spawnCoord = new MapCoordinates(spawnVector, mapId);
		SpawnPlayers(gameStartedEvent, "TestMappingPlayer", spawnCoord);
	}

	private void SpawnPlayers(EntityEventArgs startEvent, string playerProto, MapCoordinates spawnCoords)
	{
		foreach (var playerSession in _playerManager.ServerSessions)
		{
			RaiseNetworkEvent(startEvent, playerSession);
			var playerEntity = EntityManager.SpawnEntity(playerProto, spawnCoords);
			playerSession.AttachToEntity(playerEntity);
		}
		foreach (var ui in _lobbyUis)
		{
			_uiManager.CloseUi(ui);
		}
		_lobbyUis.Clear();
	}
}