using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Robust.Client.UserInterface.StylesheetHelpers;


namespace Content.Client.UI.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		public static string StyleClassHeaderFont = "Header";
		public static string StyleClassTitleFont = "Title";

		private const int FontSizeRegular = 10;
		private const int FontSizeHeader = 15;
		private const int FontSizeTitle = 32;

		private List<StyleRule> FontRules()
		{
			var defaultFont = new VectorFont(_fontResource, FontSizeRegular);
			var headerFont = new VectorFont(_fontResource, FontSizeHeader);
			var titleFont = new VectorFont(_fontResource, FontSizeTitle);

			var defaultFontRule = Element()
				.Prop(Label.StylePropertyFont, defaultFont);

			var headerFontRule = Element()
				.Class(StyleClassHeaderFont)
				.Prop(Label.StylePropertyFont, headerFont);

			var titleFontRule = Element()
				.Class(StyleClassTitleFont)
				.Prop(Label.StylePropertyFont, titleFont);

			return new List<StyleRule>
			{
				defaultFontRule,
				headerFontRule,
				titleFontRule,
			};
		}
	}
}
