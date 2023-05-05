using Content.Shared.UI;
using Robust.Shared.Network;
using Robust.Shared.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.UI
{
	public sealed class ClientUiStateManager
	{
		[Dependency] private readonly IClientNetManager _net = default!;
		[Dependency] private readonly IReflectionManager _refl = default!;
		[Dependency] private readonly IDynamicTypeFactory _dtf = default!;

		private readonly Dictionary<uint, ClientStateUiConnection> _loadedUis = new();

		public void Initialize()
		{
			_net.RegisterNetMessage<MsgUi>(HandleUiMessage);
			_net.Disconnect += NetOnDisconnect;
		}

		[Access(typeof(ClientStateUiConnection))]
		public void SendUiInput(ClientStateUiConnection ui, UiInputMessage uiInput)
		{
			SendMsgUi(ui.Id, uiInput);
		}

		private void HandleUiMessage(MsgUi message)
		{
			var id = message.Id;
			var uiMessage = message.Message;

			switch (uiMessage)
			{
				case LoadUiMessage loadUi:
					LoadUi(id, loadUi.LoadType);
					break;

				case UnloadUiMessage:
					UnloadUi(id);
					break;

				case UiStateMessage uiState:
					HandleState(id, uiState);
					break;

				case UiEventMessage uiEvent:
					HandleEvent(id, uiEvent);
					break;

				default:
					Logger.Error($"Received a UI message of an unhandled type: {message.GetType()}");
					break;
			}
		}

		private void NetOnDisconnect(object? sender, NetDisconnectedArgs e)
		{
			foreach (var loadedUi in _loadedUis)
			{
				loadedUi.Value.OnUnload();
			}
			_loadedUis.Clear();
		}

		private void LoadUi(uint id, string type)
		{
			if (!_loadedUis.TryGetValue(id, out var loadedUi))
			{
				var uiType = _refl.LooseGetType(type);
				var newUi = _dtf.CreateInstance<ClientStateUiConnection>(uiType);
				newUi.Id = id;
				newUi.OnLoad();
				_loadedUis.Add(id, newUi);
			}
			else
			{
				Logger.Error($"Tried to load a UI ({type}) but ID ({id}) was already in use by {loadedUi.GetType()}.");
			}
		}

		private void UnloadUi(uint id)
		{
			if (TryGetUi(id, out var targetUi))
			{
				targetUi.OnUnload();
				_loadedUis.Remove(id);
			}
		}

		private void HandleState(uint id, UiStateMessage uiState)
		{
			if (TryGetUi(id, out var targetUi))
			{
				targetUi.HandleState(uiState);
			}
		}

		private void HandleEvent(uint id, UiEventMessage uiState)
		{
			if (TryGetUi(id, out var targetUi))
			{
				targetUi.HandleEvent(uiState);
			}
		}

		private bool TryGetUi(uint id, [NotNullWhen(true)] out ClientStateUiConnection? ui)
		{
			if (_loadedUis.TryGetValue(id, out var foundUi))
			{
				ui = foundUi;
				return true;
			}
			else
			{
				Logger.Error($"Tried to get UI at ID: {id} but none existed.");
				ui = null;
				return false;
			}
		}

		private void SendMsgUi(uint id, BaseUiMessage message)
		{
			var msgUi = new MsgUi
			{
				Id = id,
				Message = message
			};
			_net.ClientSendMessage(msgUi);
		}
	}
}
