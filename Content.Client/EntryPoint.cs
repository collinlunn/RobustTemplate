using Content.Client.UserInterface;
using Content.Client.UserInterface.States;
using Robust.Client;
using Robust.Client.Graphics;
using Robust.Client.State;
using Robust.Shared.ContentPack;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

// DEVNOTE: Games that want to be on the hub can change their namespace prefix in the "manifest.yml" file.
namespace Content.Client;

public sealed class EntryPoint : GameClient
{
    [Dependency] private readonly IStateManager _stateManager = default!;

    public override void PreInit()
    {
        IoCManager.InjectDependencies(this);
        IoCManager.Resolve<IClyde>().SetWindowTitle("Placeholder Window Title");
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

        ClientContentIoC.Register();

        IoCManager.BuildGraph();

        factory.GenerateNetIds();

        // DEVNOTE: This is generally where you'll be setting up the IoCManager further.
        
        IoCManager.Resolve<StyleSheetManager>().Initialize(); //Load a stylesheet into the IUserInterfaceManager so UI works
    }

    public override void PostInit()
    {
        base.PostInit();

        _stateManager.RequestStateChange<MainMenuState>();

        // DEVNOTE: The line below will disable lighting, so you can see in-game sprites without the need for lights
        IoCManager.Resolve<ILightManager>().Enabled = false;

        // DEVNOTE: Further setup...
        var client = IoCManager.Resolve<IBaseClient>();

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