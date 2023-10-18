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
		private const string LineEditTexturePath = "panelDark.png";
		private StyleRule LineEditRule()
		{
			var lineEditStyleBoxTexture = GetStyleBoxTexture(LineEditTexturePath);
			lineEditStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);
			lineEditStyleBoxTexture.SetContentMarginOverride(StyleBox.Margin.Left, 5);

			var lineEditRule = Element<LineEdit>()
				.Prop(LineEdit.StylePropertyStyleBox, lineEditStyleBoxTexture);

			return lineEditRule;
		}
	}
}
