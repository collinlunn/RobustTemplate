using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Serialization;
using System;

namespace Content.Shared.PlayerMovement
{
    /// <summary>
    ///     Defined collision groups for the physics system.
    ///     Mask is what it collides with when moving. Layer is what CollisionGroup it is part of.
    /// </summary>
    [Flags, PublicAPI]
    [FlagsFor(typeof(CollisionLayer)), FlagsFor(typeof(CollisionMask))]
    public enum CollisionGroup
    {
        None = 0,
        LineOfSight = 1 << 0, // 1, layer for entities that block line of sight
        Impassable = 1 << 1, // 2, layer for any entities that can never be walked through
        Mob = 1 << 2, // 4, layer for mobs, players, etc
        Projectile = 1 << 3, // 8, layer for bullets, etc

        MapGrid = MapGridHelpers.CollisionGroup, // Map grids. This is the actual grid itself, not the walls or other entities connected to the grid.

        // 32 possible groups
        AllMask = -1,

        // Humanoids, etc.
        MobMask = Impassable | Mob,
        MobLayer = Mob,
    }
}
