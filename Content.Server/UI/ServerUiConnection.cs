using Content.Shared.UI;
using Robust.Server.Player;

namespace Content.Server.UI
{
	[Access(typeof(ServerUiStateManager))]
	public sealed class ServerUiConnection
	{
		public Enum UiKey { get; }

		public IPlayerSession Player { get; }

		public UiState State = default!;

		public bool Dirty;

		public ServerUiConnection(Enum uiKey, IPlayerSession player, UiState state)
		{
			UiKey = uiKey;
			Player = player;
			State = state;
		}
	}
}
