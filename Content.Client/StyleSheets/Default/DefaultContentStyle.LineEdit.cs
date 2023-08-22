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
			var lineEditTexture = _resourceCache.GetResource<TextureResource>(LineEditTexturePath);
			var lineEditStyleBoxTexture = new StyleBoxTexture { Texture = lineEditTexture };
			lineEditStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, DefaultMargin);
			lineEditStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, DefaultMargin);

			var lineEditRule = Element<LineEdit>()
				.Prop(LineEdit.StylePropertyStyleBox, lineEditStyleBoxTexture);

			return lineEditRule;
		}
	}
}
