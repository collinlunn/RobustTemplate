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
		private readonly Color ButtonColorDefault = Color.FromHex("#464966");
		private readonly Color ButtonColorHover = Color.FromHex("#575b7f");
		private readonly Color ButtonColorPressed = Color.FromHex("#3e6c45");
		private readonly Color ButtonColorDisabled = Color.FromHex("#30313c");

		private const string WindowCloseButtonTexturePath = "cross.png";
		private readonly Color WindowCloseButtonColorDefault = Color.FromHex("#4B596A");
		private readonly Color WindowCloseButtonColorHover = Color.FromHex("#7F3636");
		private readonly Color WindowCloseButtonColorPressed = Color.FromHex("#753131");

		private List<StyleRule> ButtonRules()
		{
			var buttonStyleBoxTexture = GetStyleBoxTexture(ButtonTexturePath);
			buttonStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 10);

			var buttonStyleBoxRule = Element<Button>()
				.Prop(ContainerButton.StylePropertyStyleBox, buttonStyleBoxTexture);

			var buttonDefaultColor = Element<Button>()
				.Pseudo(ContainerButton.StylePseudoClassNormal)
				.Prop(Control.StylePropertyModulateSelf, ButtonColorDefault);

			var buttonPressedColor = Element<Button>()
				.Pseudo(ContainerButton.StylePseudoClassPressed)
				.Prop(Control.StylePropertyModulateSelf, ButtonColorPressed);

			var buttonHoverColor = Element<Button>()
				.Pseudo(ContainerButton.StylePseudoClassHover)
				.Prop(Control.StylePropertyModulateSelf, ButtonColorHover);

			var buttonDisabledColor = Element<Button>()
				.Pseudo(ContainerButton.StylePseudoClassDisabled)
				.Prop(Control.StylePropertyModulateSelf, ButtonColorDisabled);

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
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonColorDefault);

			var windowCloseButtonHoverColorRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Pseudo(TextureButton.StylePseudoClassHover)
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonColorHover);

			var windowCloseButtonPressedColorRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Pseudo(TextureButton.StylePseudoClassPressed)
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonColorPressed);

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
