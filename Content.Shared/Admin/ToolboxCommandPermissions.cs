using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.Admin
{
	/// <summary>
	///		Placeholder way to specify what admin permissions are needed to run console commands.
	/// </summary>
	public static class ToolboxCommandPermissions
	{
		public static AdminFlags CheckPermissions(string cmdName)
		{
			foreach (var (perms, commands) in _commandPerms)
			{
				if (commands.Contains(cmdName))
				{
					return perms;
				}
			}
			Logger.Debug($"No permission for ToolboxCommand {cmdName}, defaulting to {nameof(AdminFlags.Host)}");
			return AdminFlags.Host; //if not found, default to Host perms
		}

		private static readonly Dictionary<AdminFlags, HashSet<string>> _commandPerms = new Dictionary<AdminFlags, HashSet<string>>();
	}
}
