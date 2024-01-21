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
		private const string PanelContainerTexturePath = "panelWhiteOutlined.png";

		//designates panels that should receive color modulation, to avoid coloring panels inside other controls
		//such as coloring the panels inside sliders
		private const string ContentPanelStyleRule = "ContentPanel"; 

		private List<StyleRule> PanelContainerRules()
		{
			var panelContainerStyleBoxTexture = GetStyleBoxTexture(PanelContainerTexturePath);
			panelContainerStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);

			var panelContainersStyleBoxRule = Element<PanelContainer>()
				.Prop(PanelContainer.StylePropertyPanel, panelContainerStyleBoxTexture);

			var panelContainerColorRule = Element<PanelContainer>()
				.Class(ContentPanelStyleRule)
				.Prop(Control.StylePropertyModulateSelf, PanelColor);

			var windowPanelColorRule = Element<PanelContainer>()
				.Class(DefaultWindow.StyleClassWindowPanel)
				.Prop(Control.StylePropertyModulateSelf, PanelColor);

			var windowHeaderColorRule = Element<PanelContainer>()
				.Class(DefaultWindow.StyleClassWindowHeader)
				.Prop(Control.StylePropertyModulateSelf, PanelColor);

			return new()
			{
				panelContainersStyleBoxRule,
				panelContainerColorRule,
				windowPanelColorRule,
				windowHeaderColorRule
			};
		}
	}
}
