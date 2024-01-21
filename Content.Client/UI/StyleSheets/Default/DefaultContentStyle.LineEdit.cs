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
		private const string LineEditTexturePath = "outlinedCircleWhite.png";

		private List<StyleRule> LineEditRule()
		{
			var lineEditStyleBoxTexture = GetStyleBoxTexture(LineEditTexturePath);
			lineEditStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 10);
			lineEditStyleBoxTexture.SetContentMarginOverride(StyleBox.Margin.Left, 8);


			var lineEditStyleBoxRule = Element<LineEdit>()
				.Prop(LineEdit.StylePropertyStyleBox, lineEditStyleBoxTexture);

			var lineEditColorRule = Element<LineEdit>()
				.Prop(Control.StylePropertyModulateSelf, LineEditColor);

			var lineEditNotEditableFont = Element<LineEdit>()
				.Pseudo(LineEdit.StylePseudoClassPlaceholder)
				.Prop(Label.StylePropertyFontColor, LineEditPlaceholderColor);

			var lineEditPlaceholderFont = Element<LineEdit>()
				.Class(LineEdit.StyleClassLineEditNotEditable)
				.Prop(Label.StylePropertyFontColor, LineEditDisabledColor);

			return new()
			{
				lineEditStyleBoxRule,
				lineEditColorRule,
				lineEditNotEditableFont,
				lineEditPlaceholderFont
			};
		}
	}
}
