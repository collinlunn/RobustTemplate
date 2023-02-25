using JetBrains.Annotations;
using Robust.Shared.GameObjects;

namespace Content.Shared.Test
{
    [UsedImplicitly]
    public abstract class SharedTestSystem : EntitySystem
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
