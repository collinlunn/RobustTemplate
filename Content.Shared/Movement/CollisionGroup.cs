using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Serialization;
using System;

namespace Content.Shared.Movement
{
	/// <summary>
	///     Defined collision groups for the physics system.
	///     Mask is what it collides with when moving. (Bitmask of the layers this component collides with.)
	///     Layer is what CollisionGroup it is part of. (Bitmask of the collision layers the component is a part of.)
	/// </summary>
	[Flags, PublicAPI]
    [FlagsFor(typeof(CollisionLayer)), FlagsFor(typeof(CollisionMask))]
    public enum CollisionGroup
    {
        None = 0,
        LineOfSight = 1 << 0, // 1, blocks line of sight
        Impassable = 1 << 1, // 2, blocks walking
        Mob = 1 << 2, // 4, mobs, players, etc
        Projectile = 1 << 3, // 8, bullets, etc

        MapGrid = MapGridHelpers.CollisionGroup, // Map grids. This is the actual grid itself, not the walls or other entities connected to the grid.

        // 32 possible groups
        AllMask = -1,

        // Humanoids, etc.
        MobMask = Impassable,
        MobLayer = Mob,

		// Humanoids, etc.
		//MobMask = Impassable | HighImpassable | MidImpassable | LowImpassable,
		//MobLayer = Opaque | BulletImpassable,

		// Airlocks, windoors, firelocks
		//GlassAirlockLayer = HighImpassable | MidImpassable | BulletImpassable | InteractImpassable,
	}
}
