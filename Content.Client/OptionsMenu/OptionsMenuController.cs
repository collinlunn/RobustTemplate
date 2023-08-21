using Robust.Client.UserInterface.Controllers;

namespace Content.Client.OptionsMenu
{
	public sealed class OptionsMenuController : UIController
	{
		private OptionsMenu _optionsWindow = default!;

		public override void Initialize()
		{
			_optionsWindow = UIManager.CreateWindow<OptionsMenu>();
		}

		public void ToggleWindow()
		{
			if (_optionsWindow.IsOpen)
			{
				_optionsWindow.Close();
			}
			else
			{
				_optionsWindow.OpenCentered();
			}
		}
	}
}
