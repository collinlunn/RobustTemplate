using Content.Shared.Admin;
using JetBrains.Annotations;
using Robust.Client.Console;

namespace Content.Client.Admin
{
	[UsedImplicitly]
	public sealed class ClientAdminManager : SharedAdminManager, IClientConGroupImplementation
	{
		[Dependency] private readonly IClientConGroupController _conGroup = default!;

		//I have to include this because it's in an interface, but since it's used it throws a warning
#pragma warning disable 67
		public event Action? ConGroupUpdated;
#pragma warning restore 67

		/// <summary>
		///		The admin permissions of this client. Null if server has not notified client of its permissions yet.
		/// </summary>
		private PlayerPermissions? PlayerPermissions;

		public IReadOnlyDictionary<string, AdminFlags>? ConsolePermissions => _consolePermissions;
		private Dictionary<string, AdminFlags>? _consolePermissions;

		public override void Initialize()
		{
			base.Initialize();

			_conGroup.Implementation = this;
			SubscribeNetworkEvent<UpdatePlayerPermissionsEvent>(args =>
			{ 
				PlayerPermissions = args.PlayerPermissions;
				_consolePermissions = args.CommandPermissions;
			});
		}

		public bool CanAdminMenu()
		{
			return TryGetPlayerPermissions()?.CanAdminMenu() ?? false;
		}

		public bool CanAdminPlace()
		{
			return TryGetPlayerPermissions()?.CanSpawn() ?? false;
		}

		public bool CanScript()
		{
			return TryGetPlayerPermissions()?.IsHost() ?? false;
		}

		public bool CanCommand(string cmdName)
		{
			if (_consolePermissions == null)
			{
				Log.Debug($"Tried to run {cmdName}, but no permissions from server yet.");
				return false;
			}
			if (!_consolePermissions.TryGetValue(cmdName, out var cmdFlag))
			{
				Log.Error($"Could not find permissions for {cmdName}");
				return false;
			}
			return TryGetPlayerPermissions()?.Permissions.HasFlag(cmdFlag) ?? false;
		}

		public bool CanViewVar()
		{
			return CanCommand("vv");
		}

		private PlayerPermissions? TryGetPlayerPermissions()
		{
			if (PlayerPermissions == null)
				Log.Debug("Tried to get admin permissions but it has not been received yet");

			return PlayerPermissions;
		}
	}
}
