using Robust.Client.Console;

namespace Content.Client.Admin
{
	public sealed class ClientAdminManager : IClientConGroupImplementation
	{
		[Dependency] private readonly IClientConGroupController _conGroup = default!;

		public event Action? ConGroupUpdated;

		public void SetAsActiveConsoleManager()
		{
			_conGroup.Implementation = this;
		}

		public bool CanAdminMenu()
		{
			return IsAdmin();
		}

		public bool CanAdminPlace()
		{
			return IsAdmin();
		}

		public bool CanCommand(string cmdName)
		{
			return IsAdmin();
		}

		public bool CanScript()
		{
			return IsAdmin();
		}

		public bool CanViewVar()
		{
			return IsAdmin();
		}

		private bool IsAdmin()
		{
			return true;
		}
	}
}
