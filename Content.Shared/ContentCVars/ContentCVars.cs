﻿using Robust.Shared.Configuration;
using Robust.Shared;

namespace Content.Shared.ContentCVars
{
	[CVarDefs]
	public sealed class ContentCVars : CVars
	{
		public static readonly CVarDef<bool> HudFpsVisible =
			CVarDef.Create("hud.fps_visible", true, CVar.CLIENTONLY | CVar.ARCHIVE);

		public static readonly CVarDef<bool> HudPingVisible =
			CVarDef.Create("hud.ping_visible", true, CVar.CLIENTONLY | CVar.ARCHIVE);

		/// <summary>
		///		dB level of game music.
		/// </summary>
		public static readonly CVarDef<float> MusicVolume =
			CVarDef.Create("audio.musicvolume", 0.0f, CVar.ARCHIVE | CVar.CLIENTONLY);

		/// <summary>
		///		dB level of gui sfx.
		/// </summary>
		public static readonly CVarDef<float> GuiEffectsVolume =
			CVarDef.Create("audio.guieffectsvolume", 0.0f, CVar.ARCHIVE | CVar.CLIENTONLY);

		/// <summary>
		///		dB level of ambience tracks.
		/// </summary>
		public static readonly CVarDef<float> AmbienceVolume =
			CVarDef.Create("audio.ambiencevolume", 0.0f, CVar.ARCHIVE | CVar.CLIENTONLY);

		/// <summary>
		///		Soundfile used when triggering UI click sound via UiManager
		/// </summary>
		public static readonly CVarDef<string> UIClickSound =
			CVarDef.Create("audio.click_sound", "/Audio/pop.wav", CVar.ARCHIVE | CVar.CLIENTONLY);

		/// <summary>
		///		Soundfile used when triggering UI hover sound via UiManager
		/// </summary>
		public static readonly CVarDef<string> UIHoverSound =
			CVarDef.Create("audio.hover_sound", string.Empty, CVar.ARCHIVE | CVar.CLIENTONLY);
	}
}