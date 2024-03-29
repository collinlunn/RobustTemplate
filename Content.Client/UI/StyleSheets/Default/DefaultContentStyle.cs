using Content.Client.UI.Controls;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.StylesheetHelpers;

namespace Content.Client.UI.StyleSheets.Default
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

		[Dependency] private readonly IUserInterfaceManager _uiMan = default!;
		[Dependency] private readonly ILogManager _logMan = default!;

		private readonly ISawmill _logger;

		/* Margin Notes:
		 * 
		 * StyleBox:
		 * SetContentMargin: Margin inside stylebox edge
		 * SetPadding: Margin outside stylebox edge
		 *
		 * StyleBoxTexture:
		 * SetPatchMargin: Sets lines used to split image into patches
		 * SetExpandMargin: How close image expands to edge
		 */

		public DefaultContentStyle(IResourceCache resourceCache)
		{
			IoCManager.InjectDependencies(this);

			_resourceCache = resourceCache;
			_fontResource = _resourceCache.GetResource<FontResource>(DefaultFontResourcePath);
			_logger = _logMan.GetSawmill("stylesheet.default");

			var rules = GetStyleRules();
			Stylesheet = new Stylesheet(rules);
		}

		private IReadOnlyList<StyleRule> GetStyleRules()
		{
			var styleRules = new List<StyleRule>
			{
				TabContainerRule(),
				WindowRootRule(),
				SliderRule(),
			};
			styleRules.AddRange(FontRules());
			styleRules.AddRange(ButtonRules());
			styleRules.AddRange(CheckBoxRules());
			styleRules.AddRange(WindowCloseButtonRules());
			styleRules.AddRange(VScrollBarRules());
			styleRules.AddRange(HScrollBarRules());
			styleRules.AddRange(LineEditRule());
			styleRules.AddRange(PanelContainerRules());

			return styleRules;
		}

		private Texture GetTexture(string texturePath)
		{
			return _uiMan.CurrentTheme.ResolveTexture(texturePath);
		}

		private StyleBoxTexture GetStyleBoxTexture(string texturePath)
		{
			var styleBoxTexture = new StyleBoxTexture 
			{ 
				Texture = GetTexture(texturePath)
			};
			return styleBoxTexture;
		}

		#region WindowRoot

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

		private StyleRule SliderRule()
		{
			var sliderFillBox = GetStyleBoxTexture(SliderFillTexturePath);
			sliderFillBox.Modulate = SliderFillColor;

			var sliderBackBox = GetStyleBoxTexture(SliderFillTexturePath);
			sliderBackBox.Modulate = SliderBackColor;

			var sliderForeBox = GetStyleBoxTexture(SliderOutlineTexturePath);
			sliderForeBox.Modulate = SliderForeColor;

			var sliderGrabBox = GetStyleBoxTexture(SliderGrabberTexturePath);
			sliderGrabBox.Modulate = SliderGrabberColor;

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
