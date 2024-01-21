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
		private const string ButtonTexturePath = "circleWhite.png";

		private const string WindowCloseButtonTexturePath = "cross.png";

		private List<StyleRule> ButtonRules()
		{
			var buttonStyleBoxTexture = GetStyleBoxTexture(ButtonTexturePath);
			buttonStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 10);

			var buttonStyleBoxRule = Element<Button>()
				.Prop(ContainerButton.StylePropertyStyleBox, buttonStyleBoxTexture);

			var buttonDefaultColor = Element<Button>()
				.Pseudo(ContainerButton.StylePseudoClassNormal)
				.Prop(Control.StylePropertyModulateSelf, ButtonColor);

			var buttonPressedColor = Element<Button>()
				.Pseudo(ContainerButton.StylePseudoClassPressed)
				.Prop(Control.StylePropertyModulateSelf, ButtonPressedColor);

			var buttonHoverColor = Element<Button>()
				.Pseudo(ContainerButton.StylePseudoClassHover)
				.Prop(Control.StylePropertyModulateSelf, ButtonHoveredColor);

			var buttonDisabledColor = Element<Button>()
				.Pseudo(ContainerButton.StylePseudoClassDisabled)
				.Prop(Control.StylePropertyModulateSelf, ButtonDisabledColor);

			return new List<StyleRule>
			{
				buttonStyleBoxRule,
				buttonDefaultColor,
				buttonPressedColor,
				buttonHoverColor,
				buttonDisabledColor,
			};
		}

		private List<StyleRule> WindowCloseButtonRules()
		{
			var textureCloseButton = GetTexture(WindowCloseButtonTexturePath);

			var windowCloseButtonTextureRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Prop(TextureButton.StylePropertyTexture, textureCloseButton);

			var windowCloseButtonDefaultColorRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonNormalColor);

			var windowCloseButtonHoverColorRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Pseudo(TextureButton.StylePseudoClassHover)
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonHoveredColor);

			var windowCloseButtonPressedColorRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Pseudo(TextureButton.StylePseudoClassPressed)
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonPressedColor);

			return new()
			{
				windowCloseButtonTextureRule,
				windowCloseButtonDefaultColorRule,
				windowCloseButtonHoverColorRule,
				windowCloseButtonPressedColorRule,
			};
		}
	}
}
