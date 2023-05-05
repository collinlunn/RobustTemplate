using Content.Server.UI;
using Content.Shared.Lobby;
using Content.Shared.UI;

namespace Content.Server.Lobby
{
	public sealed class LobbyStateUi : ServerStateUiConnection
	{
		[Dependency] private readonly IEntityManager _entityManager = default!;

		public override UiState GetNewState()
		{
			return new LobbyUiState();
		}
	}
}
