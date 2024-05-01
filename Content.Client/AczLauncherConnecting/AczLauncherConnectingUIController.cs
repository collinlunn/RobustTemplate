using JetBrains.Annotations;
using Robust.Client;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Network;

namespace Content.Client.AczLauncherConnecting
{
	[UsedImplicitly]
	public sealed class AczLauncherConnectingController : UIController, IOnStateChanged<AczLauncherConnectingState>
	{
		[Dependency] private readonly IUserInterfaceManager _userInterface = default!;
		[Dependency] private readonly IBaseClient _baseClient = default!;
		[Dependency] private readonly IGameController _gameController = default!;

		private AczLauncher? _gui;
		private Page _currentPage;

		public void OnStateEntered(AczLauncherConnectingState state)
		{
			_gui = new AczLauncher();
			_gui.RetryButton.OnPressed += _ => RetryConnect();
			LayoutContainer.SetAnchorPreset(_gui, LayoutContainer.LayoutPreset.HorizontalCenterWide);
			_userInterface.StateRoot.AddChild(_gui);

			SetPage(_currentPage);
			SetConnectionFailedReason(string.Empty);
			SetConnectionState(null);
		}

		public void OnStateExited(AczLauncherConnectingState state)
		{
			_gui?.Dispose();
		}
		
		public void SetPage(Page page)
		{
			_currentPage = page;

			if (_gui == null)
				return;

			_gui.RetryButton.Visible = page == Page.ConnectFailed || page == Page.Disconnected;
			_gui.LauncherStatus.Text = $"Launcher status: {page}";
			_gui.IdleAnimation.Visible = page == Page.Redialing || page == Page.Connecting;
		}

		public void SetConnectionFailedReason(string reason)
		{
			if (_gui == null)
				return;

			_gui.ConnectionFailedReason.Text = $"Connection failed reason: {reason}";
			_gui.ConnectionFailedReason.Visible = reason != string.Empty;
		}

		public void SetConnectionState(ClientConnectionState? state)
		{
			if (_gui == null)
				return;

			_gui.ConnectionStatus.Text = $"Connection status: {state}";
			_gui.ConnectionStatus.Visible = state != null;
		}

		private void RetryConnect()
		{
			if (_gameController.LaunchState.ConnectEndpoint != null)
			{
				_baseClient.ConnectToServer(_gameController.LaunchState.ConnectEndpoint);
				SetPage(Page.Connecting);
			}
		}
	}
}
