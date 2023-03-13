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
        var entityUid = ev.AttachedEntity;

        if (!entityUid.Valid)
            return;

        var eye = EnsureComp<EyeComponent>(entityUid);
        eye.Current = true;
        eye.Zoom = Vector2.One;
    }
}