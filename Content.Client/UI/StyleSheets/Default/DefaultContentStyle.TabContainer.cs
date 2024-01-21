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
		private const string TabContainerPanelTexturePath = "panelWhiteOutlined.png";
		private readonly Color TabContainerColor = Color.FromHex("#25252a");

		private StyleRule TabContainerRule()
		{
			var tabContainerPanel = GetStyleBoxTexture(TabContainerPanelTexturePath);
			tabContainerPanel.Modulate = TabContainerColor;
			tabContainerPanel.SetPatchMargin(StyleBox.Margin.All, 2);
			tabContainerPanel.SetPadding(StyleBox.Margin.Right, 4);

			var tabContainerBoxActive = GetStyleBoxTexture(TabContainerPanelTexturePath);
			tabContainerBoxActive.Modulate = ButtonHoveredColor;
			tabContainerBoxActive.SetPatchMargin(StyleBox.Margin.All, 2);
			tabContainerBoxActive.SetPadding(StyleBox.Margin.Right, 4);

			var tabContainerBoxInactive = GetStyleBoxTexture(TabContainerPanelTexturePath);
			tabContainerBoxInactive.Modulate = ButtonColor;
			tabContainerBoxInactive.SetPatchMargin(StyleBox.Margin.All, 2);
			tabContainerBoxInactive.SetPadding(StyleBox.Margin.Right, 4);

			var tabContainerRule = Element<TabContainer>()
				.Prop(TabContainer.StylePropertyPanelStyleBox, tabContainerPanel)
				.Prop(TabContainer.StylePropertyTabStyleBox, tabContainerBoxActive)
				.Prop(TabContainer.StylePropertyTabStyleBoxInactive, tabContainerBoxInactive);

			return tabContainerRule;
		}
	}
}
