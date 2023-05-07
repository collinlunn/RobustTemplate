using Content.Shared.UI;
using Robust.Shared.Network;
using Robust.Shared.Utility;
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

		public override void Shutdown()
		{
			base.Shutdown();
			_net.Disconnect += NetOnDisconnect;
		}

		public UiState? GetUiStateOrNull(Enum uiKey)
		{
			_uiStates.TryGetValue(uiKey, out var value);
			return value;
		}

		public bool TryGetUiState(Enum uiKey, [NotNullWhen(true)] out UiState? uiState)
		{
			return _uiStates.TryGetValue(uiKey, out uiState);
		}

		private void OpenUiConnection(OpenUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;
			var state = msg.State;

			DebugTools.Assert(!_uiStates.ContainsKey(uiKey),
				$"Tried to add initial UI state for {nameof(uiKey)}, but it already had one.");

			_uiStates.Add(uiKey, state);
		}

		private void CloseUiConnection(CloseUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;

			DebugTools.Assert(_uiStates.ContainsKey(uiKey),
				$"Tried set remove state of UI with key: {nameof(uiKey)} but none existed.");

			_uiStates.Remove(uiKey);
		}

		private void HandleState(StateUiConnectionMessage msg)
		{
			var uiKey = msg.UiKey;
			var state = msg.State;

			DebugTools.Assert(_uiStates.ContainsKey(uiKey),
				$"Tried set state of UI with key: {nameof(uiKey)} but none existed.");

			_uiStates[uiKey] = state;
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
