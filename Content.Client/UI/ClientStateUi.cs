using Content.Shared.UI;

namespace Content.Client.UI
{
	[Access(typeof(ClientUiStateManager))]
	public abstract class ClientStateUi
	{
		public uint Id { get; set; } = PreInitId;

		public const uint PreInitId = 0;

		public ClientStateUi()
		{
			IoCManager.InjectDependencies(this);
		}

		public abstract void OnLoad();

		public abstract void OnUnload();

		public abstract void HandleState(UiStateMessage uiState);

		public abstract void HandleEvent(UiEventMessage uiEvent);
	}
}
