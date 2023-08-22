using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Robust.Client.UserInterface.StylesheetHelpers;


namespace Content.Client.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		private const string CheckBoxUncheckedTexturePath = "/Textures/Interface/checkBoxUnchecked.png";
		private const string CheckBoxCheckedTexturePath = "/Textures/Interface/checkBoxChecked.png";

		private const int CheckBoxSeparation = 10;

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
	}
}
