using Content.Client.InGame;
using Content.Shared.Lobby;
using Content.Shared.UI;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.State;
using Robust.Client.UserInterface;

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
		SubscribeNetworkEvent<GameStartedEvent>(OnGameStarted);
	}

	private void OnLobbyJoined(LobbyJoinedEvent message)
	{
		_stateManager.RequestStateChange<LobbyState>();
	}

	private void OnGameStarted(GameStartedEvent message)
	{
		_stateManager.RequestStateChange<InGameState>();
	}

	private void OnPlayerAttached(PlayerAttachSysMessage ev)
    {
        var entity = ev.AttachedEntity;

        if (!entity.Valid)
            return;

		entity.EnsureComponentWarn<EyeComponent>().Current = true;
    }
}