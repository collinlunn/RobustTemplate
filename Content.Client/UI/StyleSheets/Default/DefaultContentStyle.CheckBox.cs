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
		private const string CheckBoxUncheckedTexturePath = "checkBoxUnchecked.png";
		private const string CheckBoxCheckedTexturePath = "checkBoxChecked.png";
		private Color CheckBoxColorDefault => ButtonNormalPallete;
		private Color CheckBoxColorPressed => ButtonPressedPallete;
		private Color CheckBoxColorDisabled => ButtonDisabledPallete;
		private const int CheckBoxSeparation = 10;

		private List<StyleRule> CheckBoxRules()
		{
			var checkBoxUncheckedTexture = GetTexture(CheckBoxUncheckedTexturePath);
			var checkBoxCheckedTexture = GetTexture(CheckBoxCheckedTexturePath);

			var uncheckedTextureRule = Element<TextureRect>()
				.Class(CheckBox.StyleClassCheckBox)
				.Prop(TextureRect.StylePropertyTexture, checkBoxUncheckedTexture);

			var checkedTextureRule = Element<TextureRect>()
				.Class(CheckBox.StyleClassCheckBoxChecked)
				.Prop(TextureRect.StylePropertyTexture, checkBoxCheckedTexture);

			var separationRule = Element<BoxContainer>()
				.Class(CheckBox.StyleClassCheckBox)
				.Prop(BoxContainer.StylePropertySeparation, CheckBoxSeparation);
			
			var colorNormal = 
				Child()
					.Parent(
						Child()
							.Parent(Element<CheckBox>().Pseudo(ContainerButton.StylePseudoClassNormal))
							.Child(Element<BoxContainer>()))
					.Child(Element<TextureRect>())
				.Prop(Control.StylePropertyModulateSelf, CheckBoxColorDefault);

			var colorHover =
				Child()
					.Parent(
						Child()
							.Parent(Element<CheckBox>().Pseudo(ContainerButton.StylePseudoClassHover))
							.Child(Element<BoxContainer>()))
					.Child(Element<TextureRect>())
				.Prop(Control.StylePropertyModulateSelf, CheckBoxColorDefault);

			var colorPressed =
				Child()
					.Parent(
						Child()
							.Parent(Element<CheckBox>().Pseudo(ContainerButton.StylePseudoClassPressed))
							.Child(Element<BoxContainer>()))
					.Child(Element<TextureRect>())
				.Prop(Control.StylePropertyModulateSelf, CheckBoxColorPressed);

			var colorDisabled =
				Child()
					.Parent(
						Child()
							.Parent(Element<CheckBox>().Pseudo(ContainerButton.StylePseudoClassDisabled))
							.Child(Element<BoxContainer>()))
					.Child(Element<TextureRect>())
				.Prop(Control.StylePropertyModulateSelf, CheckBoxColorDisabled);

			return new List<StyleRule>
			{
				uncheckedTextureRule,
				checkedTextureRule,
				separationRule,
				colorNormal,
				colorHover,
				colorPressed,
				colorDisabled,
			};
		}
	}
}
