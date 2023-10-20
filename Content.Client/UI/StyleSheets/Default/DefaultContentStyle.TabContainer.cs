﻿using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Robust.Client.UserInterface.StylesheetHelpers;

namespace Content.Client.UI.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		private const string TabContainerPanelTexturePath = "tabPanel.png";
		private readonly Color TabContainerActiveTabColor = new Color(64, 64, 64);
		private readonly Color TabContainerInactiveTabColor = new Color(32, 32, 32);

		private StyleRule TabContainerRule()
		{
			var tabContainerPanel = GetStyleBoxTexture(TabContainerPanelTexturePath);
			var tabContainerBoxActive = new StyleBoxFlat { BackgroundColor = TabContainerActiveTabColor };
			var tabContainerBoxInactive = new StyleBoxFlat { BackgroundColor = TabContainerInactiveTabColor };

			tabContainerPanel.SetPadding(StyleBox.Margin.Right, 4);
			tabContainerBoxActive.SetPadding(StyleBox.Margin.Right, 4);
			tabContainerBoxInactive.SetPadding(StyleBox.Margin.Right, 4);

			tabContainerPanel.SetPatchMargin(StyleBox.Margin.All, 2);

			var tabContainerRule = Element<TabContainer>()
				.Prop(TabContainer.StylePropertyPanelStyleBox, tabContainerPanel)
				.Prop(TabContainer.StylePropertyTabStyleBox, tabContainerBoxActive)
				.Prop(TabContainer.StylePropertyTabStyleBoxInactive, tabContainerBoxInactive);

			return tabContainerRule;
		}
	}
}