using Content.Shared.UI;
using Robust.Server.Player;

namespace Content.Server.UI
{
	[Access(typeof(ServerUiManager))]
	public abstract class ServerStateUi
	{
		public uint Id { get; set; } = PreInitId;

		public const uint PreInitId = 0;

		public IPlayerSession Player { get; set; } = default!;

		public bool Dirty;

		public ServerStateUi()
		{
			IoCManager.InjectDependencies(this);
		}

		public abstract UiStateMessage GetNewState();

		public abstract void HandleInput(UiInputMessage uiInput);
	}
}
