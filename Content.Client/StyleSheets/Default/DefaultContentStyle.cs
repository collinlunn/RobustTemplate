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
				SliderRule(),
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

		private StyleBoxTexture GetStyleBoxTexture(string texturePath)
		{
			var styleBoxTexture = new StyleBoxTexture 
			{ 
				Texture = GetTexture(texturePath)
			};
			return styleBoxTexture;
		}

		private StyleBoxFlat GetStyleBoxFlat(Color backGroundColor = default, Color borderColor = default, Thickness borderThickness = default)
		{
			var styleBoxFlat = new StyleBoxFlat
			{ 
				BackgroundColor = backGroundColor,
				BorderColor = borderColor,
				BorderThickness = borderThickness,
			};
			return styleBoxFlat;
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

		#region Slider

		private const string SliderFillTexturePath = "sliderFill.png";
		private const string SliderOutlineTexturePath = "sliderOutline.png";
		private const string SliderGrabberTexturePath = "sliderGrabber.png";

		private readonly Color SliderFillColor = Color.FromHex("#2b8f2f");
		private readonly Color SliderBackColor = Color.FromHex("#8f2b2b");
		private readonly Color SliderForeColor = Color.FromHex("#010000");

		private StyleRule SliderRule()
		{
			var sliderFillBox = GetStyleBoxTexture(SliderFillTexturePath);
			sliderFillBox.Modulate = SliderFillColor;

			var sliderBackBox = GetStyleBoxTexture(SliderFillTexturePath);
			sliderBackBox.Modulate = SliderBackColor;

			var sliderForeBox = GetStyleBoxTexture(SliderOutlineTexturePath);
			sliderForeBox.Modulate = SliderForeColor;

			var sliderGrabBox = GetStyleBoxTexture(SliderGrabberTexturePath);

			sliderFillBox.SetPatchMargin(StyleBox.Margin.All, 12);
			sliderBackBox.SetPatchMargin(StyleBox.Margin.All, 12);
			sliderForeBox.SetPatchMargin(StyleBox.Margin.All, 12);
			sliderGrabBox.SetPatchMargin(StyleBox.Margin.All, 12);

			var sliderRule = Element<Slider>()
				.Prop(Slider.StylePropertyBackground, sliderBackBox)
				.Prop(Slider.StylePropertyForeground, sliderForeBox)
				.Prop(Slider.StylePropertyGrabber, sliderGrabBox)
				.Prop(Slider.StylePropertyFill, sliderFillBox);

			return sliderRule;
		}

		#endregion
	}
}
