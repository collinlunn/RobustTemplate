using Robust.Server.Console;
using Robust.Server.Player;
using Robust.Shared.IoC;

namespace Content.Server.Admin
{
	public interface IAdminConsoleManager
	{
		void SetAsActiveConsoleManager();
	}

	/// <summary>
	///		Placeholder console manager that approves everything.
	/// </summary>
	public sealed class AdminConsoleManager : IAdminConsoleManager, IConGroupControllerImplementation
	{
		[Dependency] private readonly IConGroupController _conGroup = default!;

		void IAdminConsoleManager.SetAsActiveConsoleManager()
		{
			_conGroup.Implementation = this;
		}

		public bool CanAdminMenu(IPlayerSession session)
		{
			return true;
		}

		public bool CanAdminPlace(IPlayerSession session)
		{
			return true;
		}

		public bool CanAdminReloadPrototypes(IPlayerSession session)
		{
			return true;
		}

		public bool CanCommand(IPlayerSession session, string cmdName)
		{
			return true;
		}

		public bool CanScript(IPlayerSession session)
		{
			return true;
		}

		public bool CanViewVar(IPlayerSession session)
		{
			return true;
		}
	}
}
