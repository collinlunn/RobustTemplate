using Content.Shared;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;

namespace Content.Client;

[UsedImplicitly]
public sealed class TestSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
            
        //SubscribeNetworkEvent<PongGameStateChangedEvent>(OnPongGameStateChanged);
        //SubscribeLocalEvent<PlayerAttachSysMessage>(OnPlayerAttached);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        //Test
    }
}