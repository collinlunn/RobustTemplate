using JetBrains.Annotations;
using Robust.Client;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.AuthLib;
using Robust.Shared.Network;
using Robust.Shared;
using Robust.Client.UserInterface;
using Robust.Shared.Configuration;
using System.Text.RegularExpressions;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using Content.Client.OptionsMenu;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Physics;

namespace Content.Client.MainMenu
{
	[UsedImplicitly]
	public sealed class MainMenuUIController : UIController, IOnStateChanged<MainMenuState>
	{
		[Dependency] private readonly IBaseClient _client = default!;
		[Dependency] private readonly IClientNetManager _netManager = default!;
		[Dependency] private readonly IGameController _gameController = default!;
		[Dependency] private readonly IConfigurationManager _cfgManager = default!;
		[Dependency] private readonly IUserInterfaceManager _userInterface = default!;

		// ReSharper disable once InconsistentNaming
		private static readonly Regex IPv6Regex = new(@"\[(.*:.*:.*)](?::(\d+))?");
		private const string LocalHost = "127.0.0.1";

		private MainMenuHud? _mainMenu;

		public void OnStateEntered(MainMenuState state)
		{
			_mainMenu = new MainMenuHud();
			_mainMenu.UsernameLineEdit.Text = _cfgManager.GetCVar(CVars.PlayerName);

			_mainMenu.ConnectButton.OnPressed += _ => OnConnectPressed(_mainMenu.AddressLineEdit.Text ?? string.Empty);
			_mainMenu.ConnectToLocalHostButton.OnPressed += _ => OnConnectPressed(LocalHost);
			_mainMenu.OptionsButton.OnPressed += _ =>
			{
				_userInterface.GetUIController<OptionsMenuController>().ToggleWindow();
			};
			_mainMenu.QuitButton.OnPressed += _ =>
			{
				_gameController.Shutdown("Client pressed quit button.");
			};
			_netManager.ConnectFailed += OnConnectFailed;

			LayoutContainer.SetAnchorPreset(_mainMenu, LayoutContainer.LayoutPreset.HorizontalCenterWide);
			_userInterface.StateRoot.AddChild(_mainMenu);
		}

		public void OnStateExited(MainMenuState state)
		{
			_netManager.ConnectFailed -= OnConnectFailed;
			_mainMenu?.Dispose();
		}

		private void OnConnectFailed(object? sender, NetConnectFailArgs e)
		{
			SetIdleAnimation(false);
			_userInterface.Popup($"Disconnected: {e.Reason}", "Disconnected");
		}

		private void OnConnectPressed(string address)
		{
			if (!_gameController.LaunchState.FromLauncher)
			{
				var userName = _mainMenu?.UsernameLineEdit.Text ?? string.Empty;

				if (!UsernameHelpers.IsNameValid(userName, out var usernameReason))
				{
					_userInterface.Popup($"Invalid username:\n{usernameReason.ToText()}", "Invalid Username");
					return;
				}

				_cfgManager.SetCVar(CVars.PlayerName, userName);
				_cfgManager.SaveToFile();
			}

			if (!TryParseAddress(address, out var ip, out var port, out var connectReason))
			{
				_userInterface.Popup($"Invalid address:\n{connectReason}", "Invalid Address");
				return;
			}

			SetIdleAnimation(true);

			try
			{
				_client.ConnectToServer(ip, port);
			}
			catch (ArgumentException e)
			{
				_userInterface.Popup($"Unable to connect: {e.Message}", "Connection Error");
			}
		}

		private void SetIdleAnimation(bool connecting)
		{
			if (_mainMenu is null)
				return;

			_mainMenu.IdleAnimationBox.Visible = connecting;

			_mainMenu.UsernameLineEdit.Editable = !connecting; 
			_mainMenu.AddressLineEdit.Editable = !connecting;

			_mainMenu.ConnectToLocalHostButton.Disabled = connecting;
			_mainMenu.ConnectButton.Disabled = connecting;
		}

		private bool TryParseAddress(string address, out string ip, out ushort port, out string reason)
		{
			var match6 = IPv6Regex.Match(address);
			reason = "";
			if (match6 != Match.Empty)
			{
				ip = match6.Groups[1].Value;
				if (!match6.Groups[2].Success)
				{
					port = _client.DefaultPort;
				}
				else if (!ushort.TryParse(match6.Groups[2].Value, out port))
				{
					reason = "Not a valid port.";
					return false;
				}

				return true;
			}

			// See if the IP includes a port.
			var split = address.Split(':');
			ip = address;
			port = _client.DefaultPort;
			if (split.Length > 2)
			{
				reason = "Not a valid Address.";
				return false;
			}

			// IP:port format.
			if (split.Length == 2)
			{
				ip = split[0];
				if (!ushort.TryParse(split[1], out port))
				{
					reason = "Not a valid port.";
					return false;
				}
			}

			return true;
		}
	}
}
