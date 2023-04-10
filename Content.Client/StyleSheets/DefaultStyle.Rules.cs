using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Maths;
using System.Collections.Generic;

namespace Content.Client.StyleSheets
{
	public partial class DefaultStyle
	{
		public const string StyleClassLabelMainMenu = "LabelGameTitle";

		private const string PanelContainerTexturePath = "/Textures/Interface/panelGrey.png";
		private const string LineEditTexturePath = "/Textures/Interface/panelDark.png";
		private const string ButtonTexturePath = "/Textures/Interface/panelWhite.png";
		private const string TabContainerPanelTexturePath = "/Textures/Interface/tabPanel.png";

		private readonly Color ButtonColorDefault = Color.FromHex("#1a1a1a");
		private readonly Color ButtonColorPressed = Color.FromHex("#575b7f");
		private readonly Color ButtonColorHover = Color.FromHex("#242424");

		private StyleRule DefaultFontRule()
		{
			var defaultFont = new VectorFont(_fontResource, 10);

			var defaultFontRule = new StyleRule(
				new SelectorElement(null, null, null, null),
				new[]
				{
					new StyleProperty("font", defaultFont),
				});

			return defaultFontRule;
		}

		private StyleRule GameTitleFontRule()
		{
			var bigFont = new VectorFont(_fontResource, 32);

			var gameLabelRule = new StyleRule(
				new SelectorElement(typeof(Label), new[] { StyleClassLabelMainMenu }, null, null),
				new[]
				{
					new StyleProperty(Label.StylePropertyFont, bigFont)
				});

			return gameLabelRule;
		}

		private StyleRule PanelContainerRule()
		{
			var panelContainerTexture = _resourceCache.GetResource<TextureResource>(PanelContainerTexturePath);
			var panelContainerStyleBoxTexture = new StyleBoxTexture { Texture = panelContainerTexture };
			panelContainerStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);
			panelContainerStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, 2);

			var panelContainerRule = new StyleRule(
				new SelectorElement(typeof(PanelContainer), null, null, null),
				new[]
				{
					new StyleProperty(PanelContainer.StylePropertyPanel, panelContainerStyleBoxTexture),
				});

			return panelContainerRule;
		}

		private StyleRule LineEditRule()
		{
			var lineEditTexture = _resourceCache.GetResource<TextureResource>(LineEditTexturePath);
			var lineEditStyleBoxTexture = new StyleBoxTexture { Texture = lineEditTexture };
			lineEditStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);
			lineEditStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, 2);

			var lineEditRule = new StyleRule(
				new SelectorElement(typeof(LineEdit), null, null, null),
				new[]
				{
					new StyleProperty(LineEdit.StylePropertyStyleBox, lineEditStyleBoxTexture)
				});

			return lineEditRule;
		}

		private StyleRule TabContainerRule()
		{
			var tabContainerPanelTex = _resourceCache.GetResource<TextureResource>(TabContainerPanelTexturePath);
			var tabContainerPanel = new StyleBoxTexture
			{
				Texture = tabContainerPanelTex.Texture,
			};
			tabContainerPanel.SetPatchMargin(StyleBox.Margin.All, 2);

			var tabContainerBoxActive = new StyleBoxFlat { BackgroundColor = new Color(64, 64, 64) };
			tabContainerBoxActive.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);

			var tabContainerBoxInactive = new StyleBoxFlat { BackgroundColor = new Color(32, 32, 32) };
			tabContainerBoxInactive.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);

			var tabContainerRule = new StyleRule(
				new SelectorElement(typeof(TabContainer), null, null, null),
				new[]
				{
					new StyleProperty(TabContainer.StylePropertyPanelStyleBox, tabContainerPanel),
					new StyleProperty(TabContainer.StylePropertyTabStyleBox, tabContainerBoxActive),
					new StyleProperty(TabContainer.StylePropertyTabStyleBoxInactive, tabContainerBoxInactive),
				});

			return tabContainerRule;
		}

		private List<StyleRule> ButtonRules()
		{
			var buttonTexture = _resourceCache.GetResource<TextureResource>(ButtonTexturePath);
			var buttonStyleBoxTexture = new StyleBoxTexture { Texture = buttonTexture };
			buttonStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);
			buttonStyleBoxTexture.SetExpandMargin(StyleBox.Margin.All, 2);

			var buttonStyleBoxRule = new StyleRule(
				new SelectorElement(typeof(Button), null, null, null),
				new[]
				{
					new StyleProperty(Button.StylePropertyStyleBox, buttonStyleBoxTexture)
				});
			var buttonDefaultColor = new StyleRule(
				new SelectorElement(typeof(Button), null, null, new[] { Button.StylePseudoClassNormal }),
				new[]
				{
					new StyleProperty(Button.StylePropertyModulateSelf, ButtonColorDefault),
				});
			var buttonPressedColor = new StyleRule(
				new SelectorElement(typeof(Button), null, null, new[] { Button.StylePseudoClassPressed }),
				new[]
				{
					new StyleProperty(Button.StylePropertyModulateSelf, ButtonColorPressed),
				});
			var buttonHoverColor = new StyleRule(
				new SelectorElement(typeof(Button), null, null, new[] { Button.StylePseudoClassHover }),
				new[]
				{
					new StyleProperty(Button.StylePropertyModulateSelf, ButtonColorHover),
				});

			return new List<StyleRule>
			{
				buttonStyleBoxRule,
				buttonDefaultColor,
				buttonPressedColor,
				buttonHoverColor,
			};
		}

		private List<StyleRule> WindowCloseButtonRules()
		{
			var textureCloseButton = _resourceCache.GetResource<TextureResource>("/Textures/Interface/cross.png").Texture;

			var windowCloseButtonTextureRule = new StyleRule(
				new SelectorElement(
					typeof(TextureButton),
					new[] { DefaultWindow.StyleClassWindowCloseButton },
					null,
					null),
				new[]
				{
					new StyleProperty(TextureButton.StylePropertyTexture, textureCloseButton),
					new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#BB88BB")),
				});
			var windowCloseButtonHoverRule = new StyleRule(
				new SelectorElement(
					typeof(TextureButton),
					new[] { DefaultWindow.StyleClassWindowCloseButton },
					null,
					new[] { TextureButton.StylePseudoClassHover }),
				new[]
				{
					new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#DD88DD")),
				});
			var windowCloseButtonPressedRule = new StyleRule(
				new SelectorElement(
					typeof(TextureButton),
					new[] { DefaultWindow.StyleClassWindowCloseButton },
					null,
					new[] { TextureButton.StylePseudoClassPressed }),
				new[]
				{
					new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#FFCCFF")),
				});

			return new List<StyleRule>()
			{
				windowCloseButtonTextureRule,
				windowCloseButtonHoverRule,
				windowCloseButtonPressedRule
			};
		}
	}
}
