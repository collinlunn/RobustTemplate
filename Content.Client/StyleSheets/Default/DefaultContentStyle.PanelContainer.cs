﻿using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Robust.Client.UserInterface.StylesheetHelpers;


namespace Content.Client.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		private const string PanelContainerTexturePath = "/Textures/Interface/panelGrey.png";

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
	}
}
