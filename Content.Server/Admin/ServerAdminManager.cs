using Content.Shared.Admin;
using JetBrains.Annotations;
using Robust.Server.Console;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Players;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Errors;
using Robust.Shared.Utility;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Content.Server.Admin
{
	[UsedImplicitly]
	public sealed class ServerAdminManager : EntitySystem, IConGroupControllerImplementation
	{
		[Dependency] private readonly IConGroupController _conGroup = default!;
		[Dependency] private readonly IPlayerManager _playerManager = default!;
		[Dependency] private readonly IServerConsoleHost _consoleHost = default!;

		/// <summary>
		///		The set of admins along with their permissions.
		/// </summary>
		private readonly Dictionary<IPlayerSession, PlayerPermissions> _playerPermissions = new();

		public override void Initialize()
		{
			base.Initialize();

			_conGroup.Implementation = this;
			_playerManager.PlayerStatusChanged += PlayerStatusChanged;
		}

		#region IConGroupControllerImplementation

		public bool CanAdminMenu(IPlayerSession session)
		{
			if (TryGetCachedPlayerPermissions(session, out var player))
			{
				return player.CanAdminMenu();
			}
			return false;
		}

		public bool CanAdminPlace(IPlayerSession session)
		{
			if (TryGetCachedPlayerPermissions(session, out var player))
			{
				return player.CanSpawn();
			}
			return false;
		}

		public bool CanAdminReloadPrototypes(IPlayerSession session)
		{
			if (TryGetCachedPlayerPermissions(session, out var player))
			{
				return player.IsHost();
			}
			return false;
		}

		public bool CanScript(IPlayerSession session)
		{
			if (TryGetCachedPlayerPermissions(session, out var player))
			{
				return player.IsHost();
			}
			return false;
		}

		public bool CanCommand(IPlayerSession session, string cmdName)
		{
			return CanUseCommand(session, cmdName);
		}

		public bool CanViewVar(IPlayerSession session)
		{
			return CanUseCommand(session, "vv");
		}

		public bool CheckInvokable(CommandSpec command, ICommonSession? user, out IConError? error)
		{
			//TODO what is this for
			if (user is null)
			{
				error = null;
				return true; // Server console.
			}

			error = new NoPermissionError(command);
			return false;
		}

		#endregion

		private bool CanUseCommand(IPlayerSession session, string cmdName)
		{
			var commandPerm = CommandPermissions.CheckPermissions(cmdName);
			if (TryGetCachedPlayerPermissions(session, out var playerPerms))
			{
				return playerPerms.Permissions.HasFlag(commandPerm); 
			}
			return false;
		}

		private void PlayerStatusChanged(object? sender, SessionStatusEventArgs args)
		{
			var newStatus = args.NewStatus;
			var session = args.Session;

			if (newStatus == SessionStatus.Connected)
			{
				UpdatePlayerPermissions(session);
			}
			else if (newStatus == SessionStatus.Disconnected)
			{
				_playerPermissions.Remove(session);
			}

			void UpdatePlayerPermissions(IPlayerSession session)
			{
				var playerPerms = GetPermissions(session);
				_playerPermissions.Add(session, playerPerms);
				RaiseNetworkEvent(new UpdatePlayerPermissionsEvent(playerPerms));
			}
			PlayerPermissions GetPermissions(IPlayerSession session)
			{
				var adminFlags = AdminFlags.None;

				if (IsLocalHost(session))
				{
					adminFlags = AdminFlags.Host;
				}
				//TODO: Check database for admin data here
				return new PlayerPermissions { Permissions = adminFlags };
			}
			bool IsLocalHost(IPlayerSession player)
			{
				var ep = player.ConnectedClient.RemoteEndPoint;
				var addr = ep.Address;
				if (addr.IsIPv4MappedToIPv6)
				{
					addr = addr.MapToIPv4();
				}
				return Equals(addr, System.Net.IPAddress.Loopback) || Equals(addr, System.Net.IPAddress.IPv6Loopback);
			}
		}

		private bool TryGetCachedPlayerPermissions(IPlayerSession session, [NotNullWhen(true)] out PlayerPermissions? player)
		{
			player = _playerPermissions.GetValueOrDefault(session);
			if (player == null)
			{
				Logger.Error($"Could not find cached permissions of {session}.");
				return false;
			}
			return true;
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
