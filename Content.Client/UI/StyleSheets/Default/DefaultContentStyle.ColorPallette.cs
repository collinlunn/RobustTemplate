using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.UI.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		private readonly Color PanelPallete = Color.FromHex("#25252a");

		private readonly Color LineEditPallete = Color.FromHex("#24242b");
		private readonly Color LineEditPlaceholderPallete = Color.FromHex("#c4c4c4");
		private readonly Color LineEditDisabledPallette = Color.Gray;

		private readonly Color ButtonNormalPallete = Color.FromHex("#464966");
		private readonly Color ButtonHoverPallete = Color.FromHex("#575b7f");
		private readonly Color ButtonPressedPallete = Color.FromHex("#3e6c45");
		private readonly Color ButtonDisabledPallete = Color.FromHex("#30313c");

		private readonly Color WindowCloseButtonNormalPallete = Color.FromHex("#4B596A");
		private readonly Color WindowCloseButtonHoverPallete = Color.FromHex("#7F3636");
		private readonly Color WindowCloseButtonPressedPallete = Color.FromHex("#753131");
	}
}
