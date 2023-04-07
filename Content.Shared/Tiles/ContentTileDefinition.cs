﻿using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;
using System.Collections.Generic;

namespace Content.Shared.Tiles
{
	[Prototype("tile")]
	public sealed class ContentTileDefinition : IPrototype, IInheritingPrototype, ITileDefinition
	{
		#region ITileDefinition

		public ushort TileId { get; private set; }

		[IdDataField]
		public string ID { get; } = string.Empty;

		[DataField("name")]
		public string Name { get; private set; } = string.Empty;

		[DataField("sprite")]
		public ResourcePath? Sprite { get; private set; }

		[DataField("variants")]
		public byte Variants { get; set; } = 1;

		[DataField("edgeSprites")]
		public Dictionary<Direction, ResourcePath> EdgeSprites { get; } = new();

		[DataField("friction")]
		public float Friction { get; private set; } = 0.0f;

		public void AssignTileId(ushort id)
		{
			TileId = id;
		}

		#endregion

		#region IInheritingPrototype

		[ParentDataFieldAttribute(typeof(AbstractPrototypeIdArraySerializer<ContentTileDefinition>))]
		public string[]? Parents { get; private set; }

		[AbstractDataFieldAttribute, NeverPushInheritance]
		public bool Abstract { get; private set; } = false;

		#endregion
	}
}
