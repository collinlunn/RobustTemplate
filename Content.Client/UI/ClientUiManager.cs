using Content.Shared.UI;
using Robust.Shared.Network;
using Robust.Shared.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.UI
{
	public sealed class ClientUiManager
	{
		[Dependency] private readonly IClientNetManager _net = default!;
		[Dependency] private readonly IReflectionManager _refl = default!;
		[Dependency] private readonly IDynamicTypeFactory _dtf = default!;

		private readonly Dictionary<uint, ClientStateUi> _openUis = new();

		public void Initialize()
		{
			_net.RegisterNetMessage<MsgUi>(HandleUiMessage);
			_net.Disconnect += NetOnDisconnect;
		}

		public void SendUiInput(ClientStateUi ui, UiInputMessage uiInput)
		{
			SendMsgUi(ui.Id, uiInput);
		}

		private void HandleUiMessage(MsgUi message)
		{
			var id = message.Id;
			var uiMessage = message.Message;

			switch (uiMessage)
			{
				case OpenUiMessage openUi:
					OpenUi(id, openUi.OpenType);
					break;

				case CloseUiMessage:
					CloseUi(id);
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
			foreach (var openUi in _openUis)
			{
				openUi.Value.OnClose();
			}
			_openUis.Clear();
		}

		private void OpenUi(uint id, string type)
		{
			if (!_openUis.TryGetValue(id, out var openUi))
			{
				var uiType = _refl.LooseGetType(type);
				var newUi = _dtf.CreateInstance<ClientStateUi>(uiType);
				newUi.Id = id;
				newUi.OnOpen();
				_openUis.Add(id, newUi);
			}
			else
			{
				Logger.Error($"Tried to open a UI ({type}) but ID ({id}) was already in use by {openUi.GetType()}.");
			}
		}

		private void CloseUi(uint id)
		{
			if (TryGetUi(id, out var targetUi))
			{
				targetUi.OnClose();
				_openUis.Remove(id);
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

		private bool TryGetUi(uint id, [NotNullWhen(true)] out ClientStateUi? ui)
		{
			if (_openUis.TryGetValue(id, out var foundUi))
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
