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
	[Dependency] private readonly IUserInterfaceManager _uiManager = default!;

	private LobbyHudController LobbyHudController => _uiManager.GetUIController<LobbyHudController>();

	public override void Initialize()
    {
        base.Initialize();
		SubscribeLocalEvent<PlayerAttachSysMessage>(OnPlayerAttached);
		SubscribeNetworkEvent<LobbyJoinedEvent>(OnLobbyJoined);
		SubscribeNetworkEvent<GameStartedEvent>(OnGameStarted);

		SubscribeNetworkEvent<OpenUiConnectionMessage>(LobbyUiConnectionOpened);
		SubscribeNetworkEvent<CloseUiConnectionMessage>(LobbyUiConnectionClosed);
		SubscribeNetworkEvent<StateUiConnectionMessage>(HandleLobbyState);
	}

	private void OnLobbyJoined(LobbyJoinedEvent message)
	{
		_stateManager.RequestStateChange<LobbyState>();
	}

	private void OnGameStarted(GameStartedEvent message)
	{
		_stateManager.RequestStateChange<InGameState>();
	}

	private void LobbyUiConnectionOpened(OpenUiConnectionMessage ev)
	{
		if (ev.State is not LobbyUiState state)
			return;

		LobbyHudController.SetState(state);
	}

	private void LobbyUiConnectionClosed(CloseUiConnectionMessage ev)
	{
		LobbyHudController.ClearState();
	}

	private void HandleLobbyState(StateUiConnectionMessage ev)
	{
		if (ev.State is not LobbyUiState state)
			return;

		LobbyHudController.SetState(state);
	}

	private void OnPlayerAttached(PlayerAttachSysMessage ev)
    {
        var entity = ev.AttachedEntity;

        if (!entity.Valid)
            return;

		entity.EnsureComponentWarn<EyeComponent>().Current = true;
    }
}