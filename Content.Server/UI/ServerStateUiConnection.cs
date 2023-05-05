using Content.Shared.UI;
using Robust.Server.Player;

namespace Content.Server.UI
{
	[Access(typeof(ServerUiStateManager))]
	public abstract class ServerStateUiConnection : SharedStateUiConnection
	{
		public uint Id { get; set; } = PreInitId;

		public IPlayerSession Player { get; set; } = default!;

		public bool Dirty;

		public abstract UiStateMessage GetNewState();

		public abstract void HandleInput(UiInputMessage uiInput);
	}
}
