using Content.Shared.UI;

namespace Content.Client.UI
{
	[Access(typeof(ClientUiStateManager))]
	public abstract class ClientStateUiConnection : SharedStateUiConnection
	{
		public abstract void OnLoad();

		public abstract void OnUnload();

		public abstract void HandleState(UiStateMessage uiState);
	}
}
