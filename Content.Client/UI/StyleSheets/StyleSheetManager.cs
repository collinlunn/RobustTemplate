using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Shared.IoC;
using Robust.Client.UserInterface.Stylesheets;
using Content.Client.UI.StyleSheets.Default;

namespace Content.Client.UI.StyleSheets;

/// <summary>
///		Loads up a set of StyleSheets to be used by UI controls.
/// </summary>
public sealed class StyleSheetManager
{
    [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;
    [Dependency] private readonly IResourceCache _resourceCache = default!;

	public Stylesheet DefaultContentStyleSheet { get; private set; } = default!;
        
    public void Initialize()
    {
		_userInterfaceManager.SetDefaultTheme("DefaultContentTheme");
		DefaultContentStyleSheet = new DefaultContentStyle(_resourceCache).Stylesheet;
		_userInterfaceManager.Stylesheet = DefaultContentStyleSheet;
	}
}