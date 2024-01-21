using Robust.Client.Graphics;

namespace Content.Client.UI.StyleSheets.Default
{
	public sealed partial class DefaultContentStyle
	{
		private Color GetColor(string color)
		{
			var foundColor = _uiMan.CurrentTheme.ResolveColor(color);

			if (foundColor is null)
			{
				_logger.Error($"Could not find color {color} in theme {_uiMan.CurrentTheme.ID}, resorting to fallback.");
				foundColor = Color.HotPink;
			}
			return (Color) foundColor;
		}

		private Color PanelColor => GetColor("panel");

		private Color LineEditColor => GetColor("lineEdit");
		private Color LineEditPlaceholderColor => GetColor("lineEditPlaceholder");
		private Color LineEditDisabledColor => GetColor("lineEditDisabled");

		private Color ButtonColor => GetColor("button");
		private Color ButtonHoveredColor => GetColor("buttonHovered");
		private Color ButtonPressedColor => GetColor("buttonPressed");
		private Color ButtonDisabledColor => GetColor("buttonDisabled");

		private Color WindowCloseButtonNormalColor => GetColor("windowCloseButton");
		private Color WindowCloseButtonHoveredColor => GetColor("windowCloseButtonHovered");
		private Color WindowCloseButtonPressedColor => GetColor("windowCloseButtonPressed");

		private Color SliderFillColor => GetColor("sliderFill");
		private Color SliderBackColor => GetColor("sliderBack");
		private Color SliderForeColor => GetColor("sliderFore");
		private Color SliderGrabberColor => GetColor("sliderGrabber");

		private Color WindowRootColor => GetColor("windowRoot");

	}
}
