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
		private const string ButtonTexturePath = "panelWhite.png";
		private readonly Color ButtonColorDefault = Color.FromHex("#464966");
		private readonly Color ButtonColorHover = Color.FromHex("#575b7f");
		private readonly Color ButtonColorPressed = Color.FromHex("#3e6c45");
		private readonly Color ButtonColorDisabled = Color.FromHex("#30313c");

		private const string WindowCloseButtonTexturePath = "cross.png";
		private readonly Color WindowCloseButtonColorDefault = Color.FromHex("#BB88BB");
		private readonly Color WindowCloseButtonColorHover = Color.FromHex("#DD88DD");
		private readonly Color WindowCloseButtonColorPressed = Color.FromHex("#FFCCFF");

		private List<StyleRule> ButtonRules()
		{
			var buttonStyleBoxTexture = GetStyleBoxTexture(ButtonTexturePath);
			buttonStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);

			var buttonStyleBoxRule = Element<Button>()
				.Prop(Button.StylePropertyStyleBox, buttonStyleBoxTexture);

			var buttonDefaultColor = Element<Button>()
				.Pseudo(Button.StylePseudoClassNormal)
				.Prop(Button.StylePropertyModulateSelf, ButtonColorDefault);

			var buttonPressedColor = Element<Button>()
				.Pseudo(Button.StylePseudoClassPressed)
				.Prop(Button.StylePropertyModulateSelf, ButtonColorPressed);

			var buttonHoverColor = Element<Button>()
				.Pseudo(Button.StylePseudoClassHover)
				.Prop(Button.StylePropertyModulateSelf, ButtonColorHover);

			var buttonDisabledColor = Element<Button>()
				.Pseudo(Button.StylePseudoClassDisabled)
				.Prop(Button.StylePropertyModulateSelf, ButtonColorDisabled);

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
				.Prop(TextureButton.StylePropertyTexture, textureCloseButton)
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonColorDefault);

			var windowCloseButtonHoverRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Pseudo(TextureButton.StylePseudoClassHover)
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonColorHover);

			var windowCloseButtonPressedRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Pseudo(TextureButton.StylePseudoClassPressed)
				.Prop(Control.StylePropertyModulateSelf, WindowCloseButtonColorPressed);

			return new List<StyleRule>()
			{
				windowCloseButtonTextureRule,
				windowCloseButtonHoverRule,
				windowCloseButtonPressedRule
			};
		}
	}
}
