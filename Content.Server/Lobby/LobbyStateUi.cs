using Content.Server.UI;
using Content.Shared.Lobby;
using Content.Shared.UI;

namespace Content.Server.Lobby
{
	public sealed class LobbyStateUi : ServerStateUiConnection
	{
		[Dependency] private readonly IEntityManager _entityManager = default!;

		public override UiStateMessage GetNewState()
		{
			return new LobbyStateUiState();
		}

		public override void HandleInput(UiInputMessage uiInput)
		{
			var lobby = _entityManager.EntitySysManager.GetEntitySystem<ServerLobbySystem>(); //TODO how to not get this manually?

			switch (uiInput)
			{
				case StartGameInputMessage startGame:
					lobby.OnStartGamePressed();
					break;

				case StartMappingInputMessage startMapping:
					lobby.OnStartMappingPressed();
					break;
			}
		}
	}
}
