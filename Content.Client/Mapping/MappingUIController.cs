using Content.Client.InGame;
using Content.Shared.Input;
using Robust.Client.Input;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Input.Binding;
using Robust.Shared.IoC;
using Robust.Shared.Utility;

namespace Content.Client.Mapping
{
	public sealed class MappingUIController : UIController, IOnStateChanged<InGameState>
	{
		[Dependency] private readonly IInputManager _input = default!;

		private DefaultWindow? _window;

		public void OnStateEntered(InGameState state)
		{
			DebugTools.Assert(_window == null);
			_window = new();

			_input.SetInputCommand(ContentKeyFunctions.OpenMappingWindow,
				InputCmdHandler.FromDelegate(_ => ToggleWindow()));
		}

		public void OnStateExited(InGameState state)
		{
			_window?.Dispose();
		}

		private void ToggleWindow()
		{
			if (_window == null)
				return;

			if (_window.IsOpen)
			{
				_window.Close();
			}
			else
			{
				_window.Open();
			}
		}
	}
}