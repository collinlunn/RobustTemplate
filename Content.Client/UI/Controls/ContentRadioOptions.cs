using Robust.Shared.Utility;
using System.Linq;
using static Robust.Client.Input.Mouse;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Robust.Client.UserInterface.Controls
{
	public sealed class ContentRadioOptions<T> : Control
		where T : IComparable
	{
		public T? SelectedValue { get; private set; }

		public IEnumerable<BaseButton> Buttons => _radioOptions.Select(o => o.Button);
		private readonly List<RadioOption> _radioOptions = new();

		public Action<T>? OptionSelected;

		private LayoutOrientation Layout
		{
			get => _box.Orientation;
			set => _box.Orientation = value;
		}

		private BoxContainer _box;

		public ContentRadioOptions()
		{
			IoCManager.InjectDependencies(this);

			_box = new();
			AddChild(_box);
		}

		public BaseButton AddButton(string label, T value)
		{
			var button = new Button()
			{
				Text = label,
			};
			var radioOption = new RadioOption(button, value);
			_radioOptions.Add(radioOption);
			button.OnPressed += _ => SetSelected(radioOption);
			_box.AddChild(button);
			return button;
		}

		public bool TrySetValue(T value)
		{
			var matchingButtons = _radioOptions.Where(o => o.Value.Equals(value));

			//There should an available option for the value, but not multiple
			if (matchingButtons.Count() != 1)
			{
				return false;
			}
			SetSelected(matchingButtons.First());
			return true;
		}

		private void SetSelected(RadioOption option)
		{
			DebugTools.Assert(_radioOptions.Contains(option));
			ClearSelected();
			option.Button.Pressed = true;
			SelectedValue = option.Value;
			OptionSelected?.Invoke(SelectedValue);
		}

		private void ClearSelected()
		{
			foreach (var option in _radioOptions)
			{
				option.Button.Pressed = false;
			}
			SelectedValue = default;
		}

		private record RadioOption(BaseButton Button, T Value);
	}
}
