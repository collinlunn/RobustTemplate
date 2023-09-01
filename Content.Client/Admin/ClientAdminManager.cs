using Robust.Client.Console;

namespace Content.Client.Admin
{
	public sealed class ClientAdminManager : IClientConGroupImplementation
	{
		[Dependency] private readonly IClientConGroupController _conGroup = default!;

		//I have to include this because it's in an interface, but since it's used it throws a warning
#pragma warning disable 67
		public event Action? ConGroupUpdated;
#pragma warning restore 67

		/// <summary>
		///		Are we currently in developer debug mode?
		/// </summary>
		public bool DevMode = false;

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
			return DevMode;
		}
	}
}
