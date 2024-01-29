using Content.Shared.Admin;
using JetBrains.Annotations;
using Robust.Server.Console;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Errors;
using Robust.Shared.Utility;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Content.Server.Admin
{
	[UsedImplicitly]
	public sealed class ServerAdminManager : SharedAdminManager, IConGroupControllerImplementation
	{
		[Dependency] private readonly IConGroupController _conGroup = default!;
		[Dependency] private readonly IPlayerManager _playerManager = default!;
		[Dependency] private readonly ToolshedManager _toolshed = default!;

		public const string ClientCommandPermPath = "/Commands/clientCommandPerms.yml";
		public const string ServerCommandPermPath = "/Commands/serverCommandPerms.yml";
		public const string ToolboxCommandPermPath = "/Commands/toolboxCommandPerms.yml";

		public IReadOnlyDictionary<string, AdminFlags> ServerConsolePermissions => _serverConsolePermissions;
		private Dictionary<string, AdminFlags> _serverConsolePermissions = new();
		public IReadOnlyDictionary<string, AdminFlags> ClientConsolePermissions => _clientConsolePermissions;
		private Dictionary<string, AdminFlags> _clientConsolePermissions = new();
		public IReadOnlyDictionary<string, AdminFlags> ToolboxPermissions => _toolboxPermissions;
		private Dictionary<string, AdminFlags> _toolboxPermissions = new();

		private readonly Dictionary<ICommonSession, PlayerPermissions> _playerPermissions = new();

		public override void Initialize()
		{
			base.Initialize();

			_conGroup.Implementation = this;
			_toolshed.ActivePermissionController = this;
			_playerManager.PlayerStatusChanged += PlayerStatusChanged;

			_clientConsolePermissions = TryLoadPermissions(ClientCommandPermPath);
			_serverConsolePermissions = TryLoadPermissions(ServerCommandPermPath);
			_toolboxPermissions = TryLoadPermissions(ToolboxCommandPermPath);	
		}

		#region IConGroupControllerImplementation

		public bool CanAdminMenu(ICommonSession session)
		{
			if (TryGetCachedPlayerPermissions(session, out var player))
			{
				return player.CanAdminMenu();
			}
			return false;
		}

		public bool CanAdminPlace(ICommonSession session)
		{
			if (TryGetCachedPlayerPermissions(session, out var player))
			{
				return player.CanSpawn();
			}
			return false;
		}

		public bool CanAdminReloadPrototypes(ICommonSession session)
		{
			if (TryGetCachedPlayerPermissions(session, out var player))
			{
				return player.IsHost();
			}
			return false;
		}

		public bool CanScript(ICommonSession session)
		{
			if (TryGetCachedPlayerPermissions(session, out var player))
			{
				return player.IsHost();
			}
			return false;
		}

		public bool CanCommand(ICommonSession session, string cmdName)
		{
			return CanUseCommand(session, cmdName);
		}

		public bool CheckInvokable(CommandSpec command, ICommonSession? user, out IConError? error)
		{
			if (user is null)
			{
				error = null;
				return true; //Server console.
			}
			
			if (!TryGetCachedPlayerPermissions((ICommonSession) user, out var playerPerms))
			{
				error = new NoPermissionError(command);
				return false; //player permissions not found
			}

			var commandName = command.FullName();

			if (!_toolboxPermissions.TryGetValue(commandName, out var cmdFlag))
			{
				error = new NoPermissionError(command);
				Log.Error($"Could not find permissions for {commandName}");
				return false;
			}

			if (playerPerms.Permissions.HasFlag(cmdFlag))
			{
				error = null;
				return true; //player has perms
			}
			
			error = new NoPermissionError(command);
			return false; //player has insufficient perms
		}

		#endregion

		private bool CanUseCommand(ICommonSession session, string cmdName)
		{
			if (!TryGetCachedPlayerPermissions(session, out var playerPerms))
			{
				return false;
			}
			//unfortunately it appears this has to be hardcoded
			// "vv" is checked on the server for client ability ot use VV, but is not actually a real command on the server
			//so it will command loader tests if in YAML permissions
			if (cmdName == "vv")
			{
				return playerPerms.Permissions.HasFlag(AdminFlags.Debug);
			}
			if (!_serverConsolePermissions.TryGetValue(cmdName, out var cmdFlag))
			{
				Log.Error($"Could not find permissions for {cmdName}");
				return false;
			}
			return playerPerms.Permissions.HasFlag(cmdFlag);
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

			void UpdatePlayerPermissions(ICommonSession session)
			{
				var playerPerms = GetPermissions(session);
				_playerPermissions.Add(session, playerPerms);
				RaiseNetworkEvent(new UpdatePlayerPermissionsEvent(playerPerms, _clientConsolePermissions));
			}
			PlayerPermissions GetPermissions(ICommonSession session)
			{
				var adminFlags = AdminFlags.None;

				if (IsLocalHost(session))
				{
					adminFlags = AdminFlags.All;
				}
				//TODO: Check database for admin data here
				return new PlayerPermissions { Permissions = adminFlags };
			}
			bool IsLocalHost(ICommonSession player)
			{
				var ep = player.Channel.RemoteEndPoint;
				var addr = ep.Address;
				if (addr.IsIPv4MappedToIPv6)
				{
					addr = addr.MapToIPv4();
				}
				return Equals(addr, System.Net.IPAddress.Loopback) || Equals(addr, System.Net.IPAddress.IPv6Loopback);
			}
		}

		private bool TryGetCachedPlayerPermissions(ICommonSession session, [NotNullWhen(true)] out PlayerPermissions? player)
		{
			player = _playerPermissions.GetValueOrDefault(session);
			if (player == null)
			{
				Log.Error($"Could not find cached permissions of {session}.");
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
