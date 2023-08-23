using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using System.Collections.Generic;

namespace Content.Client.StyleSheets.Default
{
	/// <summary>
	///     Creates a stylesheet for UI.
	///     This is required so text and controls show up.
	///     (Please note that this stylesheet is simple and incomplete.)
	/// </summary>
	public sealed partial class DefaultContentStyle
	{
		public Stylesheet Stylesheet { get; }

		private IResourceCache _resourceCache;

		private FontResource _fontResource;

		private const string DefaultFontResourcePath = "/Fonts/NotoSans/NotoSans-Regular.ttf";

		private const int DefaultMargin = 2;

		public DefaultContentStyle(IResourceCache resourceCache)
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
				PanelContainerRule(),
				TabContainerRule(),
				LineEditRule(),
			};
			styleRules.AddRange(FontRules());
			styleRules.AddRange(ButtonRules());
			styleRules.AddRange(CheckBoxRules());
			styleRules.AddRange(WindowCloseButtonRules());
			
			return styleRules;
		}

		private Texture GetTexture(string texturePath)
		{
			var texture = _resourceCache.GetResource<TextureResource>(texturePath);
			return texture;
		}

		private StyleBoxTexture GetStyleBoxTexture(string texturePath)
		{
			var styleBoxTexture = new StyleBoxTexture 
			{ 
				Texture = GetTexture(texturePath)
			};
			styleBoxTexture.SetPatchMargin(StyleBox.Margin.All, DefaultMargin);
			styleBoxTexture.SetExpandMargin(StyleBox.Margin.All, DefaultMargin);

			return styleBoxTexture;
		}
	}
}
