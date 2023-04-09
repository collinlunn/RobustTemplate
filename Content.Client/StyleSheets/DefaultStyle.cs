using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Maths;

namespace Content.Client.StyleSheets
{
	/// <summary>
	///     Creates a stylesheet for UI.
	///     This is required so text and controls show up.
	///     (Please note that this stylesheet is simple and incomplete.)
	/// </summary>
	public sealed class DefaultStyle
	{
		public Stylesheet Stylesheet { get; }

		public const string StyleClassLabelGame = "LabelGame";

		public DefaultStyle(IResourceCache resourceCache)
		{
			var fontRes = resourceCache.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Regular.ttf");
			var font = new VectorFont(fontRes, 10);
			var bigFont = new VectorFont(fontRes, 32);

			var panelTex = resourceCache.GetResource<TextureResource>("/Textures/Interface/panel.png");
			var panel = new StyleBoxTexture { Texture = panelTex };
			panel.SetPatchMargin(StyleBox.Margin.All, 2);
			panel.SetExpandMargin(StyleBox.Margin.All, 2);

			var panelDarkTex = resourceCache.GetResource<TextureResource>("/Textures/Interface/panelDark.png");
			var panelDark = new StyleBoxTexture { Texture = panelDarkTex };
			panelDark.SetPatchMargin(StyleBox.Margin.All, 2);
			panelDark.SetExpandMargin(StyleBox.Margin.All, 2);

			var tabContainerPanelTex = resourceCache.GetResource<TextureResource>("/Textures/Interface/tabPanel.png");
			var tabContainerPanel = new StyleBoxTexture
			{
				Texture = tabContainerPanelTex.Texture,
			};
			tabContainerPanel.SetPatchMargin(StyleBox.Margin.All, 2);

			var tabContainerBoxActive = new StyleBoxFlat { BackgroundColor = new Color(64, 64, 64) };
			tabContainerBoxActive.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);
			var tabContainerBoxInactive = new StyleBoxFlat { BackgroundColor = new Color(32, 32, 32) };
			tabContainerBoxInactive.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);

			var textureCloseButton = resourceCache.GetResource<TextureResource>("/Textures/Interface/cross.png").Texture;

			Stylesheet = new Stylesheet(new[]
			{
			new StyleRule(
				new SelectorElement(null, null, null, null),
				new[]
				{
					new StyleProperty("font", font),
					new StyleProperty(PanelContainer.StylePropertyPanel, panel),
					new StyleProperty(LineEdit.StylePropertyStyleBox, panelDark)
				}),
            // TabContainer
            new StyleRule(new SelectorElement(typeof(TabContainer), null, null, null),
				new[]
				{
					new StyleProperty(TabContainer.StylePropertyPanelStyleBox, tabContainerPanel),
					new StyleProperty(TabContainer.StylePropertyTabStyleBox, tabContainerBoxActive),
					new StyleProperty(TabContainer.StylePropertyTabStyleBoxInactive, tabContainerBoxInactive),
				}),
            // Window close button base texture.
            new StyleRule(
				new SelectorElement(typeof(TextureButton), new[] {DefaultWindow.StyleClassWindowCloseButton}, null,
					null),
				new[]
				{
					new StyleProperty(TextureButton.StylePropertyTexture, textureCloseButton),
					new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#BB88BB")),
				}),
            // Window close button hover.
            new StyleRule(
				new SelectorElement(typeof(TextureButton), new[] {DefaultWindow.StyleClassWindowCloseButton}, null,
					new[] {TextureButton.StylePseudoClassHover}),
				new[]
				{
					new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#DD88DD")),
				}),
            // Window close button pressed.
            new StyleRule(
				new SelectorElement(typeof(TextureButton), new[] {DefaultWindow.StyleClassWindowCloseButton}, null,
					new[] {TextureButton.StylePseudoClassPressed}),
				new[]
				{
					new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#FFCCFF")),
				}),
            // Game label.
            new StyleRule(
				new SelectorElement(typeof(Label), new [] {StyleClassLabelGame}, null, null), new []
				{
					new StyleProperty(Label.StylePropertyFont, bigFont)
				})
		});
		}
	}
}
