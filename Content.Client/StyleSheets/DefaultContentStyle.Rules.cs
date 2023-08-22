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
		private const string WindowCloseButtonTexturePath = "/Textures/Interface/cross.png";
		private const string TabContainerPanelTexturePath = "/Textures/Interface/tabPanel.png";
		private const string CheckBoxUncheckedTexturePath = "/Textures/Interface/checkBoxUnchecked.png";
		private const string CheckBoxCheckedTexturePath = "/Textures/Interface/checkBoxChecked.png";

		private readonly Color ButtonColorDefault = Color.FromHex("#1a1a1a");
		private readonly Color ButtonColorPressed = Color.FromHex("#575b7f");
		private readonly Color ButtonColorHover = Color.FromHex("#242424");
		private readonly Color ButtonColorDisabled = Color.FromHex("#6a2e2e");

		private readonly Color WindowCloseButtonColorDefault = Color.FromHex("#BB88BB");
		private readonly Color WindowCloseButtonColorHover = Color.FromHex("#DD88DD");
		private readonly Color WindowCloseButtonColorPressed = Color.FromHex("#FFCCFF");

		private readonly Color TabContainerActiveTabColor = new Color(64, 64, 64);
		private readonly Color TabContainerInactiveTabColor = new Color(32, 32, 32);
		private const int TabContainerHorizontalMarginOverride = 5;

		private const int CheckBoxSeparation = 10;

		private const int FontSizeRegular = 10;
		private const int FontSizeLarge = 32;

		private const int DefaultMargin = 2;

		private List<StyleRule> FontRules()
		{
			var defaultFont = new VectorFont(_fontResource, FontSizeRegular);
			var bigFont = new VectorFont(_fontResource, FontSizeLarge);

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
			panelContainerStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, DefaultMargin);
			panelContainerStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, DefaultMargin);

			var panelContainerRule = Element<PanelContainer>()
				.Prop(PanelContainer.StylePropertyPanel, panelContainerStyleBoxTexture);

			return panelContainerRule;
		}

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

		private StyleRule TabContainerRule()
		{
			var tabContainerPanelTex = _resourceCache.GetResource<TextureResource>(TabContainerPanelTexturePath);
			var tabContainerPanel = new StyleBoxTexture { Texture = tabContainerPanelTex.Texture };
			tabContainerPanel.SetPatchMargin(StyleBox.Margin.All, DefaultMargin);

			var tabContainerBoxActive = new StyleBoxFlat { BackgroundColor = TabContainerActiveTabColor };
			tabContainerBoxActive.SetContentMarginOverride(StyleBox.Margin.Horizontal, TabContainerHorizontalMarginOverride);

			var tabContainerBoxInactive = new StyleBoxFlat { BackgroundColor = TabContainerInactiveTabColor };
			tabContainerBoxInactive.SetContentMarginOverride(StyleBox.Margin.Horizontal, TabContainerHorizontalMarginOverride);

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
			buttonStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, DefaultMargin);
			buttonStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, DefaultMargin);

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
			var checkBoxUncheckedTexture = _resourceCache.GetResource<TextureResource>(CheckBoxUncheckedTexturePath).Texture;
			var checkBoxCheckedTexture = _resourceCache.GetResource<TextureResource>(CheckBoxCheckedTexturePath).Texture;

			var uncheckedTextureRule = Element<TextureRect>()
				.Class(CheckBox.StyleClassCheckBox)
				.Prop(TextureRect.StylePropertyTexture, checkBoxUncheckedTexture);

			var checkedTextureRule = Element<TextureRect>()
				.Class(CheckBox.StyleClassCheckBoxChecked)
				.Prop(TextureRect.StylePropertyTexture, checkBoxCheckedTexture);

			var separationRule = Element<BoxContainer>()
				.Class(CheckBox.StyleClassCheckBox)
				.Prop(BoxContainer.StylePropertySeparation, CheckBoxSeparation);

			return new List<StyleRule>
			{
				uncheckedTextureRule,
				checkedTextureRule,
				separationRule,
			};
		}

		private List<StyleRule> WindowCloseButtonRules()
		{
			var textureCloseButton = _resourceCache.GetResource<TextureResource>(WindowCloseButtonTexturePath).Texture;

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
