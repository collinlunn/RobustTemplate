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
		private const string PanelContainerTexturePath = "panelWhiteOutlined.png";
		private readonly Color PanelContainerColor = Color.FromHex("#25252a");

		private List<StyleRule> PanelContainerRules()
		{
			var panelContainerStyleBoxTexture = GetStyleBoxTexture(PanelContainerTexturePath);
			panelContainerStyleBoxTexture.SetPatchMargin(StyleBox.Margin.All, 2);

			var panelContainersStyleBoxRule = Element<PanelContainer>()
				.Prop(PanelContainer.StylePropertyPanel, panelContainerStyleBoxTexture);

			var panelContainerColorRule = Element<PanelContainer>()
				.Prop(Control.StylePropertyModulateSelf, PanelContainerColor);

			return new()
			{
				panelContainersStyleBoxRule,
				panelContainerColorRule,
			};
		}
	}
}
