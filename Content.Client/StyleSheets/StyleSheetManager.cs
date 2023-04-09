using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Shared.IoC;

namespace Content.Client.StyleSheets;

/// <summary>
///		Loads up a set of StyleSheets to be used by UI controls.
/// </summary>
public sealed class StyleSheetManager
{
    [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;
    [Dependency] private readonly IResourceCache _resourceCache = default!;

	public Stylesheet DefaultStyleSheet { get; private set; } = default!;
        
    public void Initialize()
    {
		DefaultStyleSheet = new DefaultStyle(_resourceCache).Stylesheet;
		_userInterfaceManager.Stylesheet = DefaultStyleSheet;
	}
}