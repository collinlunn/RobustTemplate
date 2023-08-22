using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Robust.Client.UserInterface.StylesheetHelpers;


namespace Content.Client.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		public const string StyleClasstitleFont = "TitleFont";

		private const int FontSizeRegular = 10;
		private const int FontSizeLarge = 32;

		private List<StyleRule> FontRules()
		{
			var defaultFont = new VectorFont(_fontResource, FontSizeRegular);
			var bigFont = new VectorFont(_fontResource, FontSizeLarge);

			var defaultFontRule = Element()
				.Prop(Label.StylePropertyFont, defaultFont);

			var titleFontRule = Element()
				.Class(StyleClasstitleFont)
				.Prop(Label.StylePropertyFont, bigFont);

			return new List<StyleRule>
			{
				defaultFontRule,
				titleFontRule,
			};
		}
	}
}
