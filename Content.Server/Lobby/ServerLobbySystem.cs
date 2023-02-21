using Content.Shared.Lobby;
using JetBrains.Annotations;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.IoC;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Lobby;

[UsedImplicitly]
public sealed class ServerLobbySystem : SharedLobbySystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;

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

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
    }
}