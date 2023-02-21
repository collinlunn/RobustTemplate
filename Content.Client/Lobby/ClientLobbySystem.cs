using Content.Client.InGame;
using Content.Shared.Lobby;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.State;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;

namespace Content.Client.Lobby;

[UsedImplicitly]
public sealed class ClientLobbySystem : SharedLobbySystem
{
    [Dependency] private readonly IStateManager _stateManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PlayerAttachSysMessage>(OnPlayerAttached);
        SubscribeNetworkEvent<LobbyJoinedEvent>(OnLobbyJoined);
        SubscribeNetworkEvent<StartGameEvent>(OnStartGame);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
    }

    private void OnLobbyJoined(LobbyJoinedEvent ev)
    {
        _stateManager.RequestStateChange<LobbyState>();
    }

    private void OnStartGame(StartGameEvent ev)
    {
        _stateManager.RequestStateChange<InGameState>();
    }

    private void OnPlayerAttached(PlayerAttachSysMessage ev)
    {
        if (!ev.AttachedEntity.Valid)
            return;

        var camera = EntityManager.SpawnEntity(null, new MapCoordinates(new Vector2i(20, 10) / 2f, Transform(ev.AttachedEntity).MapID));
        var eye = EnsureComp<EyeComponent>(camera);
        eye.Current = true;
        eye.Zoom = Vector2.One;
    }
}