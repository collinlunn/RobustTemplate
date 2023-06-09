using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using System.Collections.Generic;

namespace Content.Client.StyleSheets
{
	/// <summary>
	///     Creates a stylesheet for UI.
	///     This is required so text and controls show up.
	///     (Please note that this stylesheet is simple and incomplete.)
	/// </summary>
	public sealed partial class DefaultStyle
	{
		public Stylesheet Stylesheet { get; }

		private IResourceCache _resourceCache;

		private FontResource _fontResource;

		private const string DefaultFontResourcePath = "/Fonts/NotoSans/NotoSans-Regular.ttf";

		public DefaultStyle(IResourceCache resourceCache)
		{
			_resourceCache = resourceCache;
			_fontResource = _resourceCache.GetResource<FontResource>(DefaultFontResourcePath);

			var rules = GetStyleRules();
			Stylesheet = new Stylesheet(rules);
		}

		private IReadOnlyList<StyleRule> GetStyleRules()
		{
			var styleRules = new List<StyleRule>
			{
				DefaultFontRule(),
				PanelContainerRule(),
				TabContainerRule(),
				GameTitleFontRule(),
				LineEditRule(),
			};
			styleRules.AddRange(ButtonRules());
			styleRules.AddRange(WindowCloseButtonRules());

			return styleRules;
		}
	}
}
