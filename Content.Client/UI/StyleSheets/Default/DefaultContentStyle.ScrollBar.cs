using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.StylesheetHelpers;


namespace Content.Client.UI.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		private const int ScrollBarGrabberSize = 10;
		private readonly Color ScrollBarGrabberDefaultColor = Color.Gray.WithAlpha(0.35f);
		private readonly Color ScrollBarGrabberHoverColor = new Color(140, 140, 140).WithAlpha(0.35f);
		private readonly Color ScrollBarGrabberGrabbedColor = new Color(160, 160, 160).WithAlpha(0.35f);

		private List<StyleRule> VScrollBarRules()
		{
			var vScrollBarGrabberNormal = new StyleBoxFlat
			{
				BackgroundColor = ScrollBarGrabberDefaultColor,
				ContentMarginLeftOverride = ScrollBarGrabberSize,
				ContentMarginTopOverride = ScrollBarGrabberSize
			};
			var vScrollBarGrabberHover = new StyleBoxFlat
			{
				BackgroundColor = ScrollBarGrabberHoverColor,
				ContentMarginLeftOverride = ScrollBarGrabberSize,
				ContentMarginTopOverride = ScrollBarGrabberSize
			};
			var vScrollBarGrabberGrabbed = new StyleBoxFlat
			{
				BackgroundColor = ScrollBarGrabberGrabbedColor,
				ContentMarginLeftOverride = ScrollBarGrabberSize,
				ContentMarginTopOverride = ScrollBarGrabberSize
			};

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
			var hScrollBarGrabberNormal = new StyleBoxFlat
			{
				BackgroundColor = ScrollBarGrabberDefaultColor,
				ContentMarginTopOverride = ScrollBarGrabberSize
			};
			var hScrollBarGrabberHover = new StyleBoxFlat
			{
				BackgroundColor = ScrollBarGrabberHoverColor,
				ContentMarginTopOverride = ScrollBarGrabberSize
			};
			var hScrollBarGrabberGrabbed = new StyleBoxFlat
			{
				BackgroundColor = ScrollBarGrabberGrabbedColor,
				ContentMarginTopOverride = ScrollBarGrabberSize
			};

			var grabberRule = Element<HScrollBar>()
				.Prop(ScrollBar.StylePropertyGrabber, hScrollBarGrabberNormal);
			var grabberHoverRule = Element<HScrollBar>()
				.Class(ScrollBar.StylePseudoClassHover)
				.Prop(ScrollBar.StylePropertyGrabber, hScrollBarGrabberNormal); 
			var grabberGrabbedRule = Element<HScrollBar>()
				.Class(ScrollBar.StylePseudoClassGrabbed)
				.Prop(ScrollBar.StylePropertyGrabber, hScrollBarGrabberNormal);

			return new List<StyleRule> { grabberRule, grabberHoverRule, grabberGrabbedRule };
		}
	}
}
