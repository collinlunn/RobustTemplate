using Content.Shared.UI;
using Robust.Shared.Serialization;

namespace Content.Shared.Lobby
{
	[Serializable, NetSerializable]
	public sealed class LobbyStateUiState : UiStateMessage
	{

	}

	[Serializable, NetSerializable]
	public sealed class StartGameInputMessage : UiInputMessage
	{

	}

	[Serializable, NetSerializable]
	public sealed class StartMappingInputMessage : UiInputMessage
	{

	}

	[Serializable, NetSerializable]
	public sealed class LobbyJoinedEvent : EntityEventArgs
	{

	}

	[Serializable, NetSerializable]
	public sealed class GameStartedEvent : EntityEventArgs
	{

	}

}
