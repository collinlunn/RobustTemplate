using Content.Shared.UI;
using Robust.Shared.Serialization;

namespace Content.Shared.Lobby
{
	[Serializable, NetSerializable, Virtual]
	public class LobbyUiState : UiState
	{
		public string[] ConnectedPlayers;

		public LobbyUiState(string[] connectedPlayers)
		{
			ConnectedPlayers = connectedPlayers;
		}
	}

	[Serializable, NetSerializable]
	public enum LobbyUiKey
	{
		Key
	}

	[Serializable, NetSerializable]
	public sealed class LobbyJoinedEvent : EntityEventArgs
	{

	}

	[Serializable, NetSerializable]
	public sealed class GameStartedEvent : EntityEventArgs
	{

	}

	[Serializable, NetSerializable]
	public sealed class StartGameButtonPressed : EntityEventArgs
	{

	}

	[Serializable, NetSerializable]
	public sealed class StartMappingButtonPressed : EntityEventArgs
	{

	}
}
