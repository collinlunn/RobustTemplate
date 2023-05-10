using Robust.Server.Console;
using Robust.Server.Player;

namespace Content.Server.Admin
{
	public sealed class ServerAdminManager : IConGroupControllerImplementation
	{
		[Dependency] private readonly IConGroupController _conGroup = default!;

		public void SetAsActiveConsoleManager()
		{
			_conGroup.Implementation = this;
		}

		public bool CanAdminMenu(IPlayerSession session)
		{
			return IsAdmin(session);
		}

		public bool CanAdminPlace(IPlayerSession session)
		{
			return IsAdmin(session);
		}

		public bool CanAdminReloadPrototypes(IPlayerSession session)
		{
			return IsAdmin(session);
		}

		public bool CanCommand(IPlayerSession session, string cmdName)
		{
			return IsAdmin(session);
		}

		public bool CanScript(IPlayerSession session)
		{
			return IsAdmin(session);
		}

		public bool CanViewVar(IPlayerSession session)
		{
			return IsAdmin(session);
		}

		private bool IsAdmin(IPlayerSession session)
		{
			return true;
		}
	}
}
