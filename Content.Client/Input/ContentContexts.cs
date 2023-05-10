using Content.Shared.Input;
using Robust.Shared.Input;

namespace Content.Client.Input
{
    /// <summary>
    ///     Contains a helper function for setting up all content
    ///     contexts, and modifying existing engine ones.
    /// </summary>
    public static class ContentContexts
    {
        public static void SetupContexts(IInputContextContainer contexts)
        {
            var common = contexts.GetContext("common");

			var mapping = contexts.New("mapping", "common");
			mapping.AddFunction(ContentKeyFunctions.OpenMappingCommandWindow);
			mapping.AddFunction(ContentKeyFunctions.OpenEntitySpawnWindow);
			mapping.AddFunction(ContentKeyFunctions.OpenTileSpawnWindow);
			mapping.AddFunction(ContentKeyFunctions.OpenVV);

		}
	}
}
