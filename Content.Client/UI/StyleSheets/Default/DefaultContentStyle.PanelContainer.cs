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
		private const string PanelContainerTexturePath = "panelGrey.png";

		private StyleRule PanelContainerRule()
		{
			var panelContainerStyleBoxTexture = GetStyleBoxTexture(PanelContainerTexturePath);
			panelContainerStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);

			var panelContainerRule = Element<PanelContainer>()
				.Prop(PanelContainer.StylePropertyPanel, panelContainerStyleBoxTexture);

			return panelContainerRule;
		}
	}
}