using Content.Shared.UI;
using Robust.Shared.Serialization;

namespace Content.Shared.Lobby
{
	[Serializable, NetSerializable, Virtual]
	public class LobbyUiState : UiState
	{
		public readonly string[] ConnectedPlayers;
		public readonly bool GameStarted;

		public LobbyUiState(string[] connectedPlayers, bool gameStarted)
		{
			ConnectedPlayers = connectedPlayers;
			GameStarted = gameStarted;
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

	[Serializable, NetSerializable]
	public sealed class JoinGameButtonPressed : EntityEventArgs
	{

	}
}
