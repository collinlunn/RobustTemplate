using Content.Client.InGame;
using Content.Shared.Lobby;
using Content.Shared.UI;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Shared.Player;

namespace Content.Client.Lobby;

[UsedImplicitly]
public sealed class ClientLobbySystem : SharedLobbySystem
{
    [Dependency] private readonly IStateManager _stateManager = default!;

	public override void Initialize()
    {
        base.Initialize();
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
}