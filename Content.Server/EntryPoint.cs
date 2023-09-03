using Content.Server.Admin;
using Content.Server.UI;
using Robust.Server.ServerStatus;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Network;
using Robust.Shared.Timing;

// DEVNOTE: Games that want to be on the hub can change their namespace prefix in the "manifest.yml" file.
namespace Content.Server;

public sealed class EntryPoint : GameServer
{
	[Dependency] private readonly IConfigurationManager _configMan = default!;

	public override void PreInit()
    {
        base.PreInit();
		ServerContentIoC.Register();
		IoCManager.BuildGraph();
		IoCManager.InjectDependencies(this);
	}

	public override void Init()
    {
        base.Init();

		// Configure ACZ correctly.
		var aczInfo = new DefaultMagicAczInfo("Content.Client", new[] { "Content.Client", "Content.Shared" });
		var deps = IoCManager.Resolve<IDependencyCollection>();
		var aczProvider = new DefaultMagicAczProvider(aczInfo, deps);
		IoCManager.Resolve<IStatusHost>().SetMagicAczProvider(aczProvider);

		var factory = IoCManager.Resolve<IComponentFactory>();

        factory.DoAutoRegistrations();

        foreach (var ignoreName in IgnoredComponents.List)
        {
            factory.RegisterIgnore(ignoreName);
        }
        factory.GenerateNetIds();

		// DEVNOTE: This is generally where you'll be setting up the IoCManager further.
	}

	public override void PostInit()
    {
        base.PostInit();

#if DEBUG
		_configMan.SetCVar(CVars.NetTickrate, 10);
		_configMan.SetCVar(CVars.TargetMinimumTickrate, 10);
		_configMan.SetCVar(CVars.NetPVS, false);

		_configMan.SetCVar(CVars.GridSplitting, false);
#endif
		// DEVNOTE: Can also initialize IoC stuff more here.
	}

	public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
    {
        base.Update(level, frameEventArgs);
		// DEVNOTE: Game update loop goes here. Usually you'll want some independent GameTicker.

		//switch (level)
		//{

		//}
	}
}