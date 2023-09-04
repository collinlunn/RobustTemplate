using Robust.Shared.Console;

namespace Content.Shared.Admin
{
	/// <summary>
	///		Placeholder way to specify what admin permissions are needed to run console commands.
	/// </summary>
	public static class CommandPermissions
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
			Logger.Debug($"No permission for ConsoleCommand {cmdName}, defaulting to {nameof(AdminFlags.Host)}");
			return AdminFlags.Host; //if not found, default to Host perms
		}

		private static readonly Dictionary<AdminFlags, HashSet<string>> _commandPerms = new Dictionary<AdminFlags, HashSet<string>>
		{
			{ AdminFlags.None, new HashSet<string> {
				"echo",
				"disconnect",
				"help",
				"list",
				"quit",
				"hardquit",
				"svbind",
				"bind",
				"exec",
				"cls",
				"vram",
				"monitor",
				"setmonitor",
				"keyinfo",
				"setclipboard",
				"getclipboard",
				"net_graph",
				"net_watchent",
				"devwindow",
				"fill",
				"dumpentities",
				"\">\"",
				"gcf",
				"gc",
				"gc_mode",
				"resetent",
				"resetallents",
				"cvar",
				"midipanic",
				"replay_recording_start",
				"replay_recording_stop",
				"replay_recording_stats",
				"replay_play",
				"replay_pause",
				"replay_toggle",
				"replay_skip",
				"replay_set_time",
				"replay_stop",
				"replay_load",
			} }
		};
	}
}