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

		protected override IEnumerable<string> ConsolePermPaths => new string[]
		{
			ClientCommandPermPath,
		};

		public override void Initialize()
		{
			base.Initialize();

			_conGroup.Implementation = this;
			SubscribeNetworkEvent<UpdatePlayerPermissionsEvent>(args =>
			{ 
				PlayerPermissions = args.PlayerPermissions;
			});
		}

		public bool CanAdminMenu()
		{
			return PlayerPermissions?.CanAdminMenu() ?? false;
		}

		public bool CanAdminPlace()
		{
			return PlayerPermissions?.CanSpawn() ?? false;
		}

		public bool CanScript()
		{
			return PlayerPermissions?.IsHost() ?? false;
		}

		public bool CanCommand(string cmdName)
		{
			if (!ConsolePermissions.TryGetValue(cmdName, out var cmdFlag))
			{
				Log.Error($"Could not find permissions for {cmdName}");
				return false;
			}
			return PlayerPermissions?.Permissions.HasFlag(cmdFlag) ?? false;
		}

		public bool CanViewVar()
		{
			return CanCommand("vv");
		}
	}
}
