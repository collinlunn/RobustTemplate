using Content.Shared.Test;
using JetBrains.Annotations;
using Robust.Shared.GameObjects;
using Robust.Shared.Maths;

namespace Content.Server.Test
{
    [UsedImplicitly]
    public sealed class TestSystem : SharedTestSystem
    {
        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            var testComponents = EntityQuery<TestComponent>();
            foreach (var comp in testComponents)
            {
                //TEST
            }
        }
    }
}
