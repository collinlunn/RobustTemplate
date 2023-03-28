using System.Globalization;
using Robust.Shared.Configuration;
using Robust.Shared;
using Robust.Shared.ContentPack;
using Robust.Shared.IoC;
using Robust.Shared.Localization;

// DEVNOTE: Games that want to be on the hub can change their namespace prefix in the "manifest.yml" file.
namespace Content.Shared;

public sealed class EntryPoint : GameShared
{
    // IoC services shared between the client and the server go here...
        
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
	}
}