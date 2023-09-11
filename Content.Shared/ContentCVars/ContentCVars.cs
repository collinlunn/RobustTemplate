using Robust.Shared.Configuration;
using Robust.Shared;

namespace Content.Shared.ContentCVars
{
	[CVarDefs]
	public sealed class ContentCVars : CVars
	{
		public static readonly CVarDef<bool> HudFpsVisible =
			CVarDef.Create("hud.fps_visible", true, CVar.CLIENTONLY | CVar.ARCHIVE);

		public static readonly CVarDef<float> MusicVolume =
			CVarDef.Create("audio.musicvolume", 1.0f, CVar.ARCHIVE | CVar.CLIENTONLY);

		public static readonly CVarDef<float> GuiEffectsVolume =
			CVarDef.Create("audio.guieffectsvolume", 1.0f, CVar.ARCHIVE | CVar.CLIENTONLY);

		public static readonly CVarDef<float> GameEffectsVolume =
			CVarDef.Create("audio.gameeffectsvolume", 1.0f, CVar.ARCHIVE | CVar.CLIENTONLY);

		public static readonly CVarDef<float> AmbienceVolume =
			CVarDef.Create("audio.ambiencevolume", 1.0f, CVar.ARCHIVE | CVar.CLIENTONLY);
	}
}