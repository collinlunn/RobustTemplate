using JetBrains.Annotations;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;
using System;

namespace Content.Shared.Lobby;

[UsedImplicitly]
public class SharedLobbySystem : EntitySystem
{

}

[Serializable, NetSerializable]
public sealed class LobbyJoinedEvent : EntityEventArgs
{

}

[Serializable, NetSerializable]
public sealed class StartGamePressedEvent : EntityEventArgs
{

}