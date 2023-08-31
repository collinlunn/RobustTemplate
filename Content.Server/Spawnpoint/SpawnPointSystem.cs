using Robust.Server.Player;
using Robust.Shared.Map;
using Robust.Shared.Random;
using System.Linq;
using System.Numerics;

namespace Content.Server.Spawnpoint
{
	public sealed class SpawnPointSystem : EntitySystem
	{
		[Dependency] private readonly IRobustRandom _random = default!;

		public override void Initialize()
		{
			base.Initialize();
			SubscribeLocalEvent<SpawnPlayerEvent>(OnSpawnPlayer);
			SubscribeLocalEvent<SpawnMappingPlayerEvent>(OnSpawnMappingPlayer);
		}

		private void OnSpawnPlayer(SpawnPlayerEvent message)
		{
			var spawnPoints = EntityQuery<SpawnPointComponent, TransformComponent>();

			var spawnCoords = new List<EntityCoordinates>();
			foreach ((_, var transform) in spawnPoints)
			{
				spawnCoords.Add(transform.Coordinates);
			}
			_random.Shuffle(spawnCoords);

			if (!spawnCoords.Any())
			{
				Logger.Error($"No spawn points found.");
				return;
			}

			var playerEntity = EntityManager.SpawnEntity(message.Prototype, spawnCoords.First());
			message.PlayerSession.AttachToEntity(playerEntity);
		}

		private void OnSpawnMappingPlayer(SpawnMappingPlayerEvent message)
		{
			var spawnCoords = new MapCoordinates(new Vector2(0, 0), message.MapId);
			var playerEntity = EntityManager.SpawnEntity(message.Prototype, spawnCoords);
			message.PlayerSession.AttachToEntity(playerEntity);
		}
	}

	public sealed class SpawnPlayerEvent : EntityEventArgs
	{
		public string Prototype { get; }
		public IPlayerSession PlayerSession { get; }

		public SpawnPlayerEvent(string prototype, IPlayerSession playerSession)
		{
			Prototype = prototype;
			PlayerSession = playerSession;
		}
	}

	public sealed class SpawnMappingPlayerEvent : EntityEventArgs
	{
		public string Prototype { get; }
		public MapId MapId { get; }
		public IPlayerSession PlayerSession { get; }

		public SpawnMappingPlayerEvent(string prototype, MapId mapId, IPlayerSession playerSession)
		{
			Prototype = prototype;
			MapId = mapId;
			PlayerSession = playerSession;
		}
	}
}
