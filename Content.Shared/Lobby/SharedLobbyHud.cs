using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using System;
using System.Collections.Generic;

namespace Content.Shared.Lobby
{
	/// <summary>
	///		Transmits the state of the lobby  UI.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class LobbyUIStateEvent : EntityEventArgs
	{
		/// <summary>
		///		Data on each player in the lobby.
		/// </summary>
		public IEnumerable<LobbyPlayerState> PlayersInLobby { get; }
	}

	/// <summary>
	///		Data about a aprticular player in the lobby.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class LobbyPlayerState
	{
		/// <summary>
		///  Name of the player.
		/// </summary>
		public string Name { get; }

		/// <summary>
		///		If this player is ready for the game to start.
		/// </summary>
		public bool Ready { get; }
	}

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
	///		Sent from server to client when the game has started.
	///		Raised locally on server to indicate the game has started.
	/// </summary>
	[Serializable, NetSerializable]
	public sealed class StartGameEvent : EntityEventArgs
	{

	}
}
