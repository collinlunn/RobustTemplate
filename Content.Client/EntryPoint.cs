using Content.Client.Admin;
using Content.Client.Input;
using Content.Client.MainMenu;
using Content.Client.StyleSheets;
using Content.Client.UI;
using Robust.Client;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

// DEVNOTE: Games that want to be on the hub can change their namespace prefix in the "manifest.yml" file.
namespace Content.Client;

public sealed class EntryPoint : GameClient
{
    [Dependency] private readonly IStateManager _stateManager = default!;
	[Dependency] private readonly IBaseClient _baseClient = default!;
	[Dependency] private readonly IInputManager _inputManager = default!;
	[Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

	public override void PreInit()
	{
		ClientContentIoC.Register();
		IoCManager.BuildGraph();
		IoCManager.InjectDependencies(this);
    }

    public override void Init()
    {
        var factory = IoCManager.Resolve<IComponentFactory>();
        var prototypes = IoCManager.Resolve<IPrototypeManager>();

        factory.DoAutoRegistrations();

        foreach (var ignoreName in IgnoredComponents.List)
        {
            factory.RegisterIgnore(ignoreName);
        }

        foreach (var ignoreName in IgnoredPrototypes.List)
        {
            prototypes.RegisterIgnore(ignoreName);
        }
        factory.GenerateNetIds();

		// DEVNOTE: This is generally where you'll be setting up the IoCManager further.

		IoCManager.Resolve<ClientAdminManager>().SetAsActiveConsoleManager();
		IoCManager.Resolve<CursorManager>().Initialize();
	}

	public override void PostInit()
    {
        base.PostInit();
		ContentContexts.SetupContexts(_inputManager.Contexts);

#if DEBUG
		//fake latency to help reveal bugs while debugging on localhost
		IoCManager.Resolve<IConfigurationManager>().OverrideDefault(CVars.NetFakeLagMin, 0.05f);
#endif
		IoCManager.Resolve<StyleSheetManager>().Initialize(); //Load a stylesheet into the IUserInterfaceManager so UI works
		_userInterfaceManager.MainViewport.Visible = false; //Viewport will be re-added via a UiSheet
		_stateManager.RequestStateChange<MainMenuState>(); //bring up the main menu
		//If run level drops to initialize after disconnecting reopen the main menu
		_baseClient.RunLevelChanged += (_, args) =>
		{
			if (args.NewLevel == ClientRunLevel.Initialize)
			{
				if (args.OldLevel == ClientRunLevel.Connected || args.OldLevel == ClientRunLevel.InGame)
				{
					_stateManager.RequestStateChange<MainMenuState>();
				}
			}
		};

		// DEVNOTE: You might want a main menu to connect to a server, or start a singleplayer game.
		// Be sure to check out StateManager for this! Below you'll find examples to start a game.

		// If you want to connect to a server...
		// client.ConnectToServer("ip-goes-here", 1212);

		// Optionally, singleplayer also works!
		// client.StartSinglePlayer();
	}

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
            
        // DEVNOTE: You might want to do a proper shutdown here.
    }

    public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
    {
        base.Update(level, frameEventArgs);
        // DEVNOTE: Game update loop goes here. Usually you'll want some independent GameTicker.
    }
}