using Content.Shared.UI;
using Robust.Shared.Network;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.UI
{
	public sealed class ClientUiStateManager : EntitySystem
	{
		[Dependency] private readonly IClientNetManager _net = default!;

		private readonly Dictionary<Enum, UiState> _uiStates = new();

		public override void Initialize()
		{
			base.Initialize();
			SubscribeNetworkEvent<OpenUiConnectionMessage>(OpenUiConnection);
			SubscribeNetworkEvent<CloseUiConnectionMessage>(CloseUiConnection);
			SubscribeNetworkEvent<StateUiConnectionMessage>(HandleState);
			_net.Disconnect += NetOnDisconnect;
		}

		public bool HasUiState(Enum uiKey)
		{
			return _uiStates.ContainsKey(uiKey);
		}

		public bool TryGetUiState(Enum uiKey, [NotNullWhen(true)] out UiState? uiState)
		{
			return _uiStates.TryGetValue(uiKey, out uiState);
		}

		private void OpenUiConnection(OpenUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;
			var state = msg.State;

			if (!TryGetUiState(uiKey, out var loadedUi))
			{
				_uiStates.Add(uiKey, state);
			}
			else
			{
				Logger.Error($"Tried to add initial UI state with key: ({nameof(uiKey)})," +
					$"but it already contained {nameof(loadedUi)}.");
			}
		}

		private void CloseUiConnection(CloseUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;

			if (!_uiStates.Remove(uiKey))
			{
				Logger.Error($"Tried set remove state of UI with key: {nameof(uiKey)} but none existed.");
			}
		}

		private void HandleState(StateUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;
			var state = msg.State;

			if (_uiStates.ContainsKey(uiKey))
			{
				_uiStates[uiKey] = state;
			}
			else
			{
				Logger.Error($"Tried set state of UI with key: {nameof(uiKey)} but none existed.");
			}
		}

		private void NetOnDisconnect(object? sender, NetDisconnectedArgs e)
		{
			foreach (var uiKey in _uiStates.Keys)
			{
				//TODO?
			}
			_uiStates.Clear();
		}
	}
}
