using Content.Shared.Test;
using JetBrains.Annotations;

namespace Content.Client.Test
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
