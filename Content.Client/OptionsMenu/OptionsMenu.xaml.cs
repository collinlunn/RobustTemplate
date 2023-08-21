using Robust.Client.UserInterface;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.OptionsMenu
{
	public sealed class OptionsMenu : DefaultWindow
	{
		public OptionsMenu()
		{
			RobustXamlLoader.Load(this);
			IoCManager.InjectDependencies(this);
		}
	}
}
