using Robust.Client;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Shared.Network;

namespace Content.Client.AczLauncherConnecting
{
	public sealed class AczLauncherConnectingState : State
	{
		[Dependency] private readonly IClientNetManager _clientNetManager = default!;
		[Dependency] private readonly IGameController _gameController = default!;
		[Dependency] private readonly IUserInterfaceManager _uiMan = default!;
		[Dependency] private readonly ILogManager _logMan = default!;

		private ISawmill _logger = default!;
		private AczLauncherConnectingController _ui = default!;

		protected override void Startup()
		{
			_logger = _logMan.GetSawmill("acz.launcher");
			_clientNetManager.ConnectFailed += OnConnectFailed;
			_clientNetManager.ClientConnectStateChanged += OnConnectStateChanged;
			_ui = _uiMan.GetUIController<AczLauncherConnectingController>();
		}

		protected override void Shutdown()
		{
			_clientNetManager.ConnectFailed -= OnConnectFailed;
			_clientNetManager.ClientConnectStateChanged -= OnConnectStateChanged;
		}

		private void OnConnectFailed(object? _, NetConnectFailArgs args)
		{
			if (args.RedialFlag && Redial())
			{
				_ui.SetPage(Page.Redialing);
			}
			else
			{
				_ui.SetPage(Page.ConnectFailed);
			}
			_ui.SetConnectionFailedReason(args.Reason);
		}

		private void OnConnectStateChanged(ClientConnectionState state)
		{
			_ui.SetConnectionState(state);
		}

		private bool Redial()
		{
			try
			{
				if (_gameController.LaunchState.Ss14Address != null)
				{
					_gameController.Redial(_gameController.LaunchState.Ss14Address);
					return true;
				}
				else
					_logger.Info("launcher-ui", $"Redial not possible, no Ss14Address");
			}
			catch (Exception ex)
			{
				_logger.Error("launcher-ui", $"Redial exception: {ex}");
			}
			return false;
		}

		public void SetConnecting()
		{
			_ui.SetPage(Page.Connecting);
		}

		public void SetDisconnected()
		{
			_ui.SetPage(Page.Disconnected);
		}
	}

	public enum Page : byte
	{
		Connecting,
		Redialing,
		ConnectFailed,
		Disconnected,
	}
}
