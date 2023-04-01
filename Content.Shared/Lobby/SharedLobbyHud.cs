using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using System;

namespace Content.Shared.Lobby
{
	/// <summary>
	///		Sent from client to server when joining the lobby of a server.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class LobbyJoinedEvent : EntityEventArgs
	{

	}

	/// <summary>
	///		Sent from client to server when pressing the "Start Game" button in lobby.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class StartGamePressedEvent : EntityEventArgs
	{

	}

	/// <summary>
	///		Sent from client to server when pressing the "Start Mapping" button in lobby.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class StartMappingPressedEvent : EntityEventArgs
	{

	}

	/// <summary>
	///		Sent from server to client when the game has started.
	///		Raised locally on server to indicate the game has started.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class StartGameEvent : EntityEventArgs
	{

	}
}
