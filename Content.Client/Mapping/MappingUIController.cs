using Content.Client.InGame;
using Content.Shared.Input;
using Robust.Client.Input;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controllers.Implementations;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Input.Binding;
using Robust.Shared.IoC;
using Robust.Shared.Utility;

namespace Content.Client.Mapping
{
	public sealed class MappingUIController : UIController, IOnStateChanged<InGameState>
	{
		[Dependency] private readonly IInputManager _input = default!;

		private EntitySpawningUIController EntitySpawningController => UIManager.GetUIController<EntitySpawningUIController>();
		private TileSpawningUIController TileSpawningController => UIManager.GetUIController<TileSpawningUIController>();

		private MappingCommandWindow? _window;

		public void OnStateEntered(InGameState state)
		{
			DebugTools.Assert(_window == null);
			_window = UIManager.CreateWindow<MappingCommandWindow>();

			_input.SetInputCommand(ContentKeyFunctions.OpenEntitySpawnWindow,
				InputCmdHandler.FromDelegate(_ => EntitySpawningController.ToggleWindow()));
			_input.SetInputCommand(ContentKeyFunctions.OpenTileSpawnWindow,
				InputCmdHandler.FromDelegate(_ => TileSpawningController.ToggleWindow()));
			_input.SetInputCommand(ContentKeyFunctions.OpenMappingCommandWindow,
				InputCmdHandler.FromDelegate(_ => ToggleMappingCommandWindow()));
		}

		public void OnStateExited(InGameState state)
		{
			_window?.Dispose();
			_window = null;
		}

		private void ToggleMappingCommandWindow()
		{
			if (_window == null)
				return;

			if (_window.IsOpen)
				_window.Close();
			else
				_window.Open();
		}
	}
}