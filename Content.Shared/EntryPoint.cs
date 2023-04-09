using System.Globalization;
using Robust.Shared.Configuration;
using Robust.Shared;
using Robust.Shared.ContentPack;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Map;
using System.Collections.Generic;
using Robust.Shared.Prototypes;
using System;
using Content.Shared.Tiles;

// DEVNOTE: Games that want to be on the hub can change their namespace prefix in the "manifest.yml" file.
namespace Content.Shared;

public sealed class EntryPoint : GameShared
{
	// IoC services shared between the client and the server go here...
	[Dependency] private readonly IPrototypeManager _prototypeManager = default!;
	[Dependency] private readonly ITileDefinitionManager _tileDefinitionManager = default!;

	// See line 23. Controls the default game culture and language.
	// Robust calls this culture, but you might find it more fitting to call it the game
	// language. Robust doesn't support changing this mid-game. Load your config file early
	// if you want that.
	private const string Culture = "en-US";

    public override void PreInit()
    {
        IoCManager.InjectDependencies(this);

        // Default to en-US.
        // DEVNOTE: If you want your game to be multiregional at runtime, you'll need to 
        // do something more complicated here.
        IoCManager.Resolve<ILocalizationManager>().LoadCulture(new CultureInfo(Culture));
        // TODO: Document what else you might want to put here
    }

    public override void Init()
    {
        // TODO: Document what you put here
    }

    public override void PostInit()
    {
        base.PostInit();

#if DEBUG
		//test settings with simulated latency
		var configMan = IoCManager.Resolve<IConfigurationManager>();
		configMan.SetCVar(CVars.NetFakeLagMin, 0.1f);
		configMan.SetCVar(CVars.NetTickrate, 10);
		configMan.SetCVar(CVars.TargetMinimumTickrate, 10);
#endif
		// DEVNOTE: You might want to put special init handlers for, say, tiles here.
		// TODO: Document what else you might want to put here
		InitTiles();
	}

	private void InitTiles()
	{
		//Registers the dedicated "BlankTile" as the 0th TileId, as that Id is used to be "no tile"
		//Yes this is awful, but I don't want to make engine changes RN
		var blankTileDef = _prototypeManager.Index<ContentTileDefinition>(ContentTileDefinition.BlankTileId);
		_tileDefinitionManager.Register(blankTileDef);

		//Get all the other tiles	
		var prototypeList = new List<ITileDefinition>();
		foreach (var tileDef in _prototypeManager.EnumeratePrototypes<ContentTileDefinition>())
		{
			//Skip the BlankTile prototype because we special-cased that. I hate this.
			if (tileDef.ID == ContentTileDefinition.BlankTileId)
				continue;

			prototypeList.Add(tileDef);
		}

		// Sort ordinal to ensure it's consistent client and server.
		// So that tile IDs match up.
		prototypeList.Sort((a, b) => string.Compare(a.ID, b.ID, StringComparison.Ordinal));

		foreach (var tileDef in prototypeList)
		{
			_tileDefinitionManager.Register(tileDef);
		}

		_tileDefinitionManager.Initialize();


		_prototypeManager.PrototypesReloaded += _ =>
		{
			// Need to re-allocate tiledefs due to how prototype reloads work
			foreach (var def in _prototypeManager.EnumeratePrototypes<ContentTileDefinition>())
			{
				def.AssignTileId(_tileDefinitionManager[def.ID].TileId);
			}
		};
	}
}