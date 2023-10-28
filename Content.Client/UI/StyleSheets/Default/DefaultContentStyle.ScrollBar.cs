using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.StylesheetHelpers;


namespace Content.Client.UI.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		private const string ScrollBarTexturePath = "panelWhiteOutlined.png";

		private const int ScrollBarGrabberSize = 10;
		private readonly Color ScrollBarGrabberDefaultColor = Color.FromHex("#464966").WithAlpha(0.35f);
		private readonly Color ScrollBarGrabberHoverColor = Color.FromHex("#575b7f").WithAlpha(0.35f);
		private readonly Color ScrollBarGrabberGrabbedColor = Color.FromHex("#3e6c45").WithAlpha(0.35f);

		private List<StyleRule> VScrollBarRules()
		{
			var vScrollBarGrabberNormal = GetStyleBoxTexture(ScrollBarTexturePath);
			vScrollBarGrabberNormal.Modulate = ScrollBarGrabberDefaultColor;
			vScrollBarGrabberNormal.SetContentMarginOverride(StyleBox.Margin.Top | StyleBox.Margin.Left, ScrollBarGrabberSize);
			vScrollBarGrabberNormal.SetPatchMargin(StyleBox.Margin.All, 2);

			var vScrollBarGrabberHover = GetStyleBoxTexture(ScrollBarTexturePath);
			vScrollBarGrabberHover.Modulate = ScrollBarGrabberHoverColor;
			vScrollBarGrabberHover.SetContentMarginOverride(StyleBox.Margin.Top | StyleBox.Margin.Left, ScrollBarGrabberSize);
			vScrollBarGrabberHover.SetPatchMargin(StyleBox.Margin.All, 2);

			var vScrollBarGrabberGrabbed = GetStyleBoxTexture(ScrollBarTexturePath);
			vScrollBarGrabberGrabbed.Modulate = ScrollBarGrabberGrabbedColor;
			vScrollBarGrabberGrabbed.SetContentMarginOverride(StyleBox.Margin.Top | StyleBox.Margin.Left, ScrollBarGrabberSize);
			vScrollBarGrabberGrabbed.SetPatchMargin(StyleBox.Margin.All, 2);

			var grabberRule = Element<VScrollBar>()
				.Prop(ScrollBar.StylePropertyGrabber, vScrollBarGrabberNormal);
			var grabberHoverRule = Element<VScrollBar>()
				.Class(ScrollBar.StylePseudoClassHover)
				.Prop(ScrollBar.StylePropertyGrabber, vScrollBarGrabberHover);
			var grabberGrabbedRule = Element<VScrollBar>()
				.Class(ScrollBar.StylePseudoClassGrabbed)
				.Prop(ScrollBar.StylePropertyGrabber, vScrollBarGrabberGrabbed);

			return new List<StyleRule> { grabberRule, grabberHoverRule, grabberGrabbedRule };
		}

		private List<StyleRule> HScrollBarRules()
		{
			var hScrollBarGrabberNormal = GetStyleBoxTexture(ScrollBarTexturePath);
			hScrollBarGrabberNormal.Modulate = ScrollBarGrabberDefaultColor;
			hScrollBarGrabberNormal.SetContentMarginOverride(StyleBox.Margin.Top, ScrollBarGrabberSize);
			hScrollBarGrabberNormal.SetPatchMargin(StyleBox.Margin.All, 2);

			var hScrollBarGrabberHover = GetStyleBoxTexture(ScrollBarTexturePath);
			hScrollBarGrabberHover.Modulate = ScrollBarGrabberHoverColor;
			hScrollBarGrabberHover.SetContentMarginOverride(StyleBox.Margin.Top, ScrollBarGrabberSize);
			hScrollBarGrabberHover.SetPatchMargin(StyleBox.Margin.All, 2);

			var hScrollBarGrabberGrabbed = GetStyleBoxTexture(ScrollBarTexturePath);
			hScrollBarGrabberGrabbed.Modulate = ScrollBarGrabberGrabbedColor;
			hScrollBarGrabberGrabbed.SetContentMarginOverride(StyleBox.Margin.Top, ScrollBarGrabberSize);
			hScrollBarGrabberGrabbed.SetPatchMargin(StyleBox.Margin.All, 2);

			var grabberRule = Element<HScrollBar>()
				.Prop(ScrollBar.StylePropertyGrabber, hScrollBarGrabberNormal);
			var grabberHoverRule = Element<HScrollBar>()
				.Class(ScrollBar.StylePseudoClassHover)
				.Prop(ScrollBar.StylePropertyGrabber, hScrollBarGrabberHover); 
			var grabberGrabbedRule = Element<HScrollBar>()
				.Class(ScrollBar.StylePseudoClassGrabbed)
				.Prop(ScrollBar.StylePropertyGrabber, hScrollBarGrabberGrabbed);

			return new List<StyleRule> { grabberRule, grabberHoverRule, grabberGrabbedRule };
		}
	}
}
