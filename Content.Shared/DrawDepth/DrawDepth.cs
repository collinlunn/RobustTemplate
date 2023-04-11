using Robust.Shared.Serialization;
using DrawDepthTag = Robust.Shared.GameObjects.DrawDepth;

namespace Content.Shared.DrawDepth
{
    [ConstantsFor(typeof(DrawDepthTag))]
    public enum DrawDepth
    {
		/// <summary>
		///		Carpet, floor tiles, etc.
		/// </summary>
		Floors = DrawDepthTag.Default - 2,

		/// <summary>
		///		Walls in buildings.
		/// </summary>
		Walls = DrawDepthTag.Default - 1,

		/// <summary>
		///		Player-interactable objects.
		/// </summary>
		Objects = DrawDepthTag.Default,

		/// <summary>
		///		Any characters and players.
		/// </summary>
        Mobs = DrawDepthTag.Default + 1,

        /// <summary>
        ///     Explosions, fires, etc.
        /// </summary>
        Effects = DrawDepthTag.Default + 2,

		/// <summary>
		///		UI-like objects that should render at the top.
		/// </summary>
        Overlays = DrawDepthTag.Default + 3,
    }
}
