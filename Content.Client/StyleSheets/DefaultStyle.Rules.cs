using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Robust.Client.UserInterface.StylesheetHelpers;


namespace Content.Client.StyleSheets
{
	public partial class DefaultContentStyle
	{
		public const string StyleClassLabelMainMenu = "LabelGameTitle";

		private const string PanelContainerTexturePath = "/Textures/Interface/panelGrey.png";
		private const string LineEditTexturePath = "/Textures/Interface/panelDark.png";
		private const string ButtonTexturePath = "/Textures/Interface/panelWhite.png";
		private const string TabContainerPanelTexturePath = "/Textures/Interface/tabPanel.png";
		private const string CheckBoxUncheckedTexturePath = "/Textures/Interface/checkBoxChecked.png";
		private const string CheckBoxCheckedTexturePath = "/Textures/Interface/checkBoxUnchecked.png";

		private readonly Color ButtonColorDefault = Color.FromHex("#1a1a1a");
		private readonly Color ButtonColorPressed = Color.FromHex("#575b7f");
		private readonly Color ButtonColorHover = Color.FromHex("#242424");
		private readonly Color ButtonColorDisabled = Color.FromHex("#6a2e2e");

		private List<StyleRule> FontRules()
		{
			var defaultFont = new VectorFont(_fontResource, 10);
			var bigFont = new VectorFont(_fontResource, 32);

			var defaultFontRule = Element()
				.Prop(Label.StylePropertyFont, defaultFont);

			var titleFontRule = Element()
				.Class(StyleClassLabelMainMenu)
				.Prop(Label.StylePropertyFont, bigFont);

			return new List<StyleRule>
			{
				defaultFontRule,
				titleFontRule,
			};
		}

		private StyleRule PanelContainerRule()
		{
			var panelContainerTexture = _resourceCache.GetResource<TextureResource>(PanelContainerTexturePath);
			var panelContainerStyleBoxTexture = new StyleBoxTexture { Texture = panelContainerTexture };
			panelContainerStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);
			panelContainerStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, 2);

			var panelContainerRule = Element<PanelContainer>()
				.Prop(PanelContainer.StylePropertyPanel, panelContainerStyleBoxTexture);

			return panelContainerRule;
		}

		private StyleRule LineEditRule()
		{
			var lineEditTexture = _resourceCache.GetResource<TextureResource>(LineEditTexturePath);
			var lineEditStyleBoxTexture = new StyleBoxTexture { Texture = lineEditTexture };
			lineEditStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);
			lineEditStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, 2);

			var lineEditRule = Element<LineEdit>()
				.Prop(LineEdit.StylePropertyStyleBox, lineEditStyleBoxTexture);

			return lineEditRule;
		}

		private StyleRule TabContainerRule()
		{
			var tabContainerPanelTex = _resourceCache.GetResource<TextureResource>(TabContainerPanelTexturePath);
			var tabContainerPanel = new StyleBoxTexture { Texture = tabContainerPanelTex.Texture };
			tabContainerPanel.SetPatchMargin(StyleBox.Margin.All, 2);

			var tabContainerBoxActive = new StyleBoxFlat { BackgroundColor = new Color(64, 64, 64) };
			tabContainerBoxActive.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);

			var tabContainerBoxInactive = new StyleBoxFlat { BackgroundColor = new Color(32, 32, 32) };
			tabContainerBoxInactive.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);

			var tabContainerRule = Element<TabContainer>()
				.Prop(TabContainer.StylePropertyPanelStyleBox, tabContainerPanel)
				.Prop(TabContainer.StylePropertyTabStyleBox, tabContainerBoxActive)
				.Prop(TabContainer.StylePropertyTabStyleBoxInactive, tabContainerBoxInactive);

			return tabContainerRule;
		}

		private List<StyleRule> ButtonRules()
		{
			var buttonTexture = _resourceCache.GetResource<TextureResource>(ButtonTexturePath);
			var buttonStyleBoxTexture = new StyleBoxTexture { Texture = buttonTexture };
			buttonStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);
			buttonStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, 2);

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

		private List<StyleRule> CheckBoxRules()
		{
			var checkBoxUncheckedTexture = _resourceCache.GetResource<TextureResource>(CheckBoxUncheckedTexturePath);
			var checkBoxCheckedTexture = _resourceCache.GetResource<TextureResource>(CheckBoxCheckedTexturePath);
	
			var rule1 = Element<TextureRect>()
				.Class(CheckBox.StyleClassCheckBox)
				.Prop(TextureRect.StylePropertyTexture, checkBoxUncheckedTexture);

			var rule2 = Element<TextureRect>()
				.Class(CheckBox.StyleClassCheckBoxChecked)
				.Prop(TextureRect.StylePropertyTexture, checkBoxCheckedTexture);

			return new List<StyleRule>
			{
				rule1,
				rule2,
			};
		}

		private List<StyleRule> WindowCloseButtonRules()
		{
			var textureCloseButton = _resourceCache.GetResource<TextureResource>("/Textures/Interface/cross.png").Texture;

			var windowCloseButtonTextureRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Prop(TextureButton.StylePropertyTexture, textureCloseButton)
				.Prop(Control.StylePropertyModulateSelf, Color.FromHex("#BB88BB"));

			var windowCloseButtonHoverRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Pseudo(TextureButton.StylePseudoClassHover)
				.Prop(Control.StylePropertyModulateSelf, Color.FromHex("#DD88DD"));

			var windowCloseButtonPressedRule = Element<TextureButton>()
				.Class(DefaultWindow.StyleClassWindowCloseButton)
				.Pseudo(TextureButton.StylePseudoClassPressed)
				.Prop(Control.StylePropertyModulateSelf, Color.FromHex("#FFCCFF"));

			return new List<StyleRule>()
			{
				windowCloseButtonTextureRule,
				windowCloseButtonHoverRule,
				windowCloseButtonPressedRule
			};
		}
	}
}
