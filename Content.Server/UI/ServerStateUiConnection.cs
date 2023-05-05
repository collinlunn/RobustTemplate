using Content.Shared.UI;
using Robust.Server.Player;

namespace Content.Server.UI
{
	[Access(typeof(ServerUiStateManager))]
	public abstract class ServerStateUiConnection
	{
		[Dependency] ServerUiStateManager _uiMan = default!;

		public uint Id { get; set; } = PreInitId;

		public const uint PreInitId = 0;

		public IPlayerSession Player { get; set; } = default!;

		public bool Dirty;

		public ServerStateUiConnection()
		{
			IoCManager.InjectDependencies(this);
		}

		[Access(Other = AccessPermissions.ReadWriteExecute)]
		public void Load(IPlayerSession player)
		{
			_uiMan.LoadUi(this, player);
			MarkDirty();
		}

		[Access(Other = AccessPermissions.ReadWriteExecute)]
		public void Unload()
		{
			_uiMan.UnloadUi(this);
		}

		[Access(Other = AccessPermissions.ReadWriteExecute)]
		public void SendUiEvent(UiEventMessage uiEvent)
		{
			_uiMan.SendUiEvent(this, uiEvent);
		}

		[Access(Other = AccessPermissions.ReadWriteExecute)]
		public void MarkDirty()
		{
			if (!Dirty)
			{
				Dirty = true;
				_uiMan.QueueStateUpdate(this);
			}
		}

		public abstract UiStateMessage GetNewState();

		public abstract void HandleInput(UiInputMessage uiInput);
	}
}
