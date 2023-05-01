using Content.Shared.UI;

namespace Content.Client.UI
{
	[Access(typeof(ClientUiManager))]
	public abstract class ClientStateUi
	{
		public uint Id { get; set; } = PreInitId;

		public const uint PreInitId = 0;

		public ClientStateUi()
		{
			IoCManager.InjectDependencies(this);
		}

		public abstract void OnOpen();

		public abstract void OnClose();

		public abstract void HandleState(UiStateMessage uiState);

		public abstract void HandleEvent(UiEventMessage uiEvent);
	}
}
