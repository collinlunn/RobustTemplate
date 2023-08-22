using Robust.Shared.Configuration;
using Robust.Shared;

namespace Content.Shared.ContentCVars
{
	[CVarDefs]
	public sealed class ContentCVars : CVars
	{
		public static readonly CVarDef<bool> HudFpsVisible =
			CVarDef.Create("hud.fps_visible", true, CVar.CLIENTONLY | CVar.ARCHIVE);
	}
}
