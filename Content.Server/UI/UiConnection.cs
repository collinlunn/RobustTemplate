using Content.Shared.UI;
using Robust.Server.Player;

namespace Content.Server.UI
{
	[Access(typeof(ServerUiStateManager))]
	public sealed class UiConnection
	{
		public uint Id { get; }

		public IPlayerSession Player { get; }

		public UiState State = default!;

		public bool Dirty;

		public UiConnection(uint id, IPlayerSession player, UiState state)
		{
			Id = id;
			Player = player;
			State = state;
		}

		[Access(Other = AccessPermissions.ReadWriteExecute)]
		public void Unload()
		{
			IoCManager.Resolve<ServerUiStateManager>().UnloadUi(this);
		}
	}
}
