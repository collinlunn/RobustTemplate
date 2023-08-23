using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.StylesheetHelpers;

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

		private const string TextureRoot = "/Textures/Interface/";

		private const float DefaultPatchMargin = 2f; //Increasing shrinks the corners
		private const int DefaultExpandMargin = 0; //Increasing pushes out how far the patch expands from the stylebox edge

		private const int DefaultContentMargin = 2; //Margin inside stylebox edge
		private const int DefaultPaddingMargin = 2; //Margin outside stylebox edge

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
				WindowRootRule(),
			};
			styleRules.AddRange(FontRules());
			styleRules.AddRange(ButtonRules());
			styleRules.AddRange(CheckBoxRules());
			styleRules.AddRange(WindowCloseButtonRules());
			
			return styleRules;
		}

		private Texture GetTexture(string texturePath)
		{
			var texture = _resourceCache.GetResource<TextureResource>(TextureRoot + texturePath);
			return texture;
		}

		private StyleBox GetStyleBoxTexture(string texturePath)
		{
			var styleBoxTexture = new StyleBoxTexture 
			{ 
				Texture = GetTexture(texturePath)
			};
			SetDefaultPatchMargins(styleBoxTexture);
			SetDefaultMargins(styleBoxTexture);
			return styleBoxTexture;
		}

		private StyleBox GetStyleBoxFlat(Color backGroundColor = default, Color borderColor = default, Thickness borderThickness = default)
		{
			var styleBoxFlat = new StyleBoxFlat
			{ 
				BackgroundColor = backGroundColor,
				BorderColor = borderColor,
				BorderThickness = borderThickness,
			};
			SetDefaultMargins(styleBoxFlat);
			return styleBoxFlat;
		}

		private void SetDefaultPatchMargins(StyleBoxTexture styleBoxTexture)
		{
			styleBoxTexture.SetPatchMargin(StyleBox.Margin.All, DefaultPatchMargin);
			styleBoxTexture.SetExpandMargin(StyleBox.Margin.All, DefaultExpandMargin);
		}

		private void SetDefaultMargins(StyleBox styleBox)
		{
			styleBox.SetContentMarginOverride(StyleBox.Margin.All, DefaultContentMargin);
			styleBox.SetPadding(StyleBox.Margin.All, DefaultPaddingMargin);
		}

		#region WindowRoot

		private readonly Color WindowRootColor = Color.Black;

		private StyleRule WindowRootRule()
		{
			var windowRootRule = Element<WindowRoot>()
				.Prop(UIRoot.StylePropBackground, WindowRootColor);
			return windowRootRule;
		}

		#endregion
	}
}
