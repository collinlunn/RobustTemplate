using Content.Shared.UI;

namespace Content.Client.UI
{
	[Access(typeof(ClientUiStateManager))]
	public abstract class ClientStateUiConnection : SharedStateUiConnection
	{
		[Dependency] private readonly ClientUiStateManager _uiMan = default!;

		public uint Id { get; set; } = PreInitId;


		public void SendUiInput(UiInputMessage uiInput)
		{
			_uiMan.SendUiInput(this, uiInput);
		}

		public abstract void OnLoad();

		public abstract void OnUnload();

		public abstract void HandleState(UiStateMessage uiState);

		public abstract void HandleEvent(UiEventMessage uiEvent);
	}
}
