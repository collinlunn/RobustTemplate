using Content.Client.InGame;
using Content.Client.UI;
using Content.Shared.Lobby;
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

	public override void Initialize()
    {
        base.Initialize();
		SubscribeLocalEvent<PlayerAttachSysMessage>(OnPlayerAttached);
		SubscribeNetworkEvent<LobbyJoinedEvent>(OnLobbyJoined);
		SubscribeNetworkEvent<GameStartedEvent>(OnGameStarted);

		SubscribeLocalEvent<UiConnectionLoadedEvent>(HandleLobbyUiLoad);
		SubscribeLocalEvent<UiConnectionUnloadedEvent>(HandleLobbyUiUnload);
		SubscribeLocalEvent<UiStateReceivedEvent>(HandleLobbyUiState);
	}

	private void OnLobbyJoined(LobbyJoinedEvent message)
	{
		_stateManager.RequestStateChange<LobbyState>();
		//set ui with placeholder state or real state if it already arrived
		//_uiManager.GetUIController<LobbyHudController>();
	}

	private void OnGameStarted(GameStartedEvent message)
	{
		_stateManager.RequestStateChange<InGameState>();
		//close ui
	}

	private void HandleLobbyUiLoad(UiConnectionLoadedEvent ev)
	{
		//set ui controller state if it exists
		//otherwise store state until it is made then apply it
	}

	private void HandleLobbyUiUnload(UiConnectionUnloadedEvent ev)
	{
		//replace ui controller state with placeholder if it still exists
	}

	private void HandleLobbyUiState(UiStateReceivedEvent ev)
	{
		//set ui state
	}

	private void OnPlayerAttached(PlayerAttachSysMessage ev)
    {
        var entity = ev.AttachedEntity;

        if (!entity.Valid)
            return;

		entity.EnsureComponentWarn<EyeComponent>().Current = true;
    }
}