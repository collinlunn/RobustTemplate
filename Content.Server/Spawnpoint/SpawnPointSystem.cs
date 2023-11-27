using Robust.Server.Player;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Random;
using System.Linq;
using System.Numerics;

namespace Content.Server.Spawnpoint
{
	public sealed class SpawnPointSystem : EntitySystem
	{
		[Dependency] private readonly IRobustRandom _random = default!;
		[Dependency] private readonly IPlayerManager _playerManager = default!;

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
				Log.Error($"No spawn points found.");
				return;
			}

			var playerEntity = EntityManager.SpawnEntity(message.Prototype, spawnCoords.First());
			
			if (!_playerManager.SetAttachedEntity(message.Session, playerEntity))
			{
				Log.Error($"Player {playerEntity} could not attach when spawning");
			}
		}

		private void OnSpawnMappingPlayer(SpawnMappingPlayerEvent message)
		{
			var spawnCoords = new MapCoordinates(new Vector2(0, 0), message.MapId);
			var playerEntity = EntityManager.SpawnEntity(message.Prototype, spawnCoords);

			if (!_playerManager.SetAttachedEntity(message.Session, playerEntity))
			{
				Log.Error($"Player {playerEntity} could not attach when spawning");
			}
		}
	}

	public sealed class SpawnPlayerEvent : EntityEventArgs
	{
		public string Prototype { get; }
		public ICommonSession Session { get; }

		public SpawnPlayerEvent(string prototype, ICommonSession session)
		{
			Prototype = prototype;
			Session = session;
		}
	}

	public sealed class SpawnMappingPlayerEvent : EntityEventArgs
	{
		public string Prototype { get; }
		public MapId MapId { get; }
		public ICommonSession Session { get; }

		public SpawnMappingPlayerEvent(string prototype, MapId mapId, ICommonSession session)
		{
			Prototype = prototype;
			MapId = mapId;
			Session = session;
		}
	}
}
