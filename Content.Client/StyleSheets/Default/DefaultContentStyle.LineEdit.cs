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
		private const string LineEditTexturePath = "/Textures/Interface/panelDark.png";
		private StyleRule LineEditRule()
		{
			var lineEditStyleBoxTexture = GetStyleBoxTexture(LineEditTexturePath);

			var lineEditRule = Element<LineEdit>()
				.Prop(LineEdit.StylePropertyStyleBox, lineEditStyleBoxTexture);

			return lineEditRule;
		}
	}
}
