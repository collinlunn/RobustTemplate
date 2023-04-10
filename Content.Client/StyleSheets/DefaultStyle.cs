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
		private VectorFont _defaultFont;

		private StyleBoxTexture _greyBoxPanel; //TODO Replace this with a greyscale one for coloring
		private StyleBoxTexture _darkBoxPanel; //TODO Replace this with a greyscale one for coloring
		private StyleBoxTexture _whiteBoxPanel;

		public DefaultStyle(IResourceCache resourceCache)
		{
			_resourceCache = resourceCache;
			_fontResource = _resourceCache.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Regular.ttf");
			_defaultFont = new VectorFont(_fontResource, 10);

			var panelGreyTex = _resourceCache.GetResource<TextureResource>("/Textures/Interface/panelGrey.png");
			_greyBoxPanel = new StyleBoxTexture { Texture = panelGreyTex };
			_greyBoxPanel.SetPatchMargin(StyleBox.Margin.All, 2);
			_greyBoxPanel.SetExpandMargin(StyleBox.Margin.All, 2);

			var panelDarkTex = _resourceCache.GetResource<TextureResource>("/Textures/Interface/panelDark.png");
			_darkBoxPanel = new StyleBoxTexture { Texture = panelDarkTex };
			_darkBoxPanel.SetPatchMargin(StyleBox.Margin.All, 2);
			_darkBoxPanel.SetExpandMargin(StyleBox.Margin.All, 2);

			var panelWhiteTex = _resourceCache.GetResource<TextureResource>("/Textures/Interface/panelWhite.png");
			_whiteBoxPanel = new StyleBoxTexture { Texture = panelWhiteTex };
			_whiteBoxPanel.SetPatchMargin(StyleBox.Margin.All, 2);
			_whiteBoxPanel.SetExpandMargin(StyleBox.Margin.All, 2);

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
				LabelGameTitleRule(),
				LineEditRule(),
			};
			styleRules.AddRange(ButtonRules());
			styleRules.AddRange(WindowCloseButtonRules());

			return styleRules;
		}
	}
}
