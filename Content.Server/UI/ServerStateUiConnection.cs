using Content.Shared.UI;
using Robust.Server.Player;

namespace Content.Server.UI
{
	[Access(typeof(ServerUiStateManager))]
	public abstract class ServerStateUiConnection : SharedStateUiConnection
	{
		/// <summary>
		///		The player this ui state is for.
		/// </summary>
		public IPlayerSession Player { get; set; } = default!;

		/// <summary>
		///		If the ui state need to be resent.
		/// </summary>
		public bool Dirty;

		/// <summary>
		///		The ui state to send to the player.
		/// </summary>
		public UiState State = default!;
	}
}
