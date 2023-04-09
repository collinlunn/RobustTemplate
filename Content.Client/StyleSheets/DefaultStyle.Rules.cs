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

		private StyleRule DefaultFontRule()
		{
			var defaultFontRule = new StyleRule(
				new SelectorElement(null, null, null, null),
				new[]
				{
					new StyleProperty("font", _defaultFont),
				});

			return defaultFontRule;
		}

		private StyleRule PanelContainerRule()
		{
			var panelContainerRule = new StyleRule(
				new SelectorElement(typeof(PanelContainer), null, null, null),
				new[]
				{
					new StyleProperty(PanelContainer.StylePropertyPanel, _lightBoxPanel),
				});

			return panelContainerRule;
		}

		private StyleRule LineEditRule()
		{
			var lineEditRule = new StyleRule(
				new SelectorElement(typeof(LineEdit), null, null, null),
				new[]
				{
					new StyleProperty(LineEdit.StylePropertyStyleBox, _darkBoxPanel)
				});

			return lineEditRule;
		}

		private StyleRule TabContainerRule()
		{
			var tabContainerPanelTex = _resourceCache.GetResource<TextureResource>("/Textures/Interface/tabPanel.png");
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

		private StyleRule LabelGameTitleRule()
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
	}
}
