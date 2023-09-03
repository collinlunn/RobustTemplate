﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.Admin
{
	public sealed class PlayerPermissions
	{
		public AdminFlags Permissions { get; set; } = AdminFlags.None;

		public bool CanSpawn()
		{
			return Permissions.HasFlag(AdminFlags.SpawnMenu);
		}

		public bool CanAdminMenu()
		{
			return Permissions.HasFlag(AdminFlags.AdminMenu);
		}

		public bool IsHost()
		{
			return Permissions.HasFlag(AdminFlags.Host);
		}
	}

	[Flags]
	public enum AdminFlags
	{
		None = 0,
		SpawnMenu = 1 << 0, //can spawn stuff in
		AdminMenu = 1 << 1, //can use admin menus (bans, etc)
		Host = 1 << 3, //can execute dangerous server-side scripts
		All = ~0,
	}
}
