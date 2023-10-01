using Content.Shared.UI;

namespace Content.Server.UI
{
	[Access(typeof(ServerUiStateManager))]
	public sealed class ServerUiConnection
	{
		public Enum UiKey { get; }

		public UiState State = default!;

		public bool Dirty;

		public ServerUiConnection(Enum uiKey, UiState state)
		{
			UiKey = uiKey;
			State = state;
		}
	}
}
