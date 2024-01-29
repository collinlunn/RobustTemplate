using Content.Client.UI.Controls;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.AczLauncherConnecting
{
	[UsedImplicitly]
	public sealed class AczLauncherConnectingController : UIController, IOnStateChanged<AczLauncherConnectingState>
	{
		[Dependency] private readonly IUserInterfaceManager _userInterface = default!;

		private IdleAnimation? _idleAnimation;

		public void OnStateEntered(AczLauncherConnectingState state)
		{
			_idleAnimation = new IdleAnimation();
			LayoutContainer.SetAnchorPreset(_idleAnimation, LayoutContainer.LayoutPreset.Center);
			_userInterface.StateRoot.AddChild(_idleAnimation);
		}

		public void OnStateExited(AczLauncherConnectingState state)
		{
			_idleAnimation?.Dispose();
		}
	}
}
