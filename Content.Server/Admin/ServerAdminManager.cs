using Robust.Server.Console;
using Robust.Server.Player;
using Robust.Shared.Players;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Errors;
using Robust.Shared.Utility;
using System.Diagnostics;

namespace Content.Server.Admin
{
	public sealed class ServerAdminManager : IConGroupControllerImplementation
	{
		[Dependency] private readonly IConGroupController _conGroup = default!;

		/// <summary>
		///		Are we currently in developer debug mode?
		/// </summary>
		public bool DevMode = false;

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
			return DevMode || IsLocalHost(session);
		}

		private static bool IsLocalHost(IPlayerSession player)
		{
			var ep = player.ConnectedClient.RemoteEndPoint;
			var addr = ep.Address;
			if (addr.IsIPv4MappedToIPv6)
			{
				addr = addr.MapToIPv4();
			}

			return Equals(addr, System.Net.IPAddress.Loopback) || Equals(addr, System.Net.IPAddress.IPv6Loopback);
		}

		public bool CheckInvokable(CommandSpec command, ICommonSession? user, out IConError? error)
		{
			if (user is null)
			{
				error = null;
				return true; // Server console.
			}

			error = new NoPermissionError(command);
			return false;
		}

		public record struct NoPermissionError(CommandSpec Command) : IConError
		{
			public FormattedMessage DescribeInner()
			{
				return FormattedMessage.FromMarkup($"You do not have permission to execute {Command.FullName()}");
			}

			public string? Expression { get; set; }
			public Vector2i? IssueSpan { get; set; }
			public StackTrace? Trace { get; set; }
		}
	}
}
