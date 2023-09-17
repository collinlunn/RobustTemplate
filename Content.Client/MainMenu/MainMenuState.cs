using Content.Client.Audio;
using Content.Shared.ContentCVars;
using Robust.Client.Graphics;
using Robust.Client.State;
using Robust.Shared.Configuration;
using YamlDotNet.Core.Tokens;

namespace Content.Client.MainMenu;

public sealed class MainMenuState : State
{
	[Dependency] private readonly MainMenuAudio _audio = default!;
	[Dependency] private readonly IConfigurationManager _cfg = default!;

	private IClydeAudioSource? _menuMusic = null;

	private const string MenuMusicPath = "test_music.wav";

	protected override void Startup()
    {
		_cfg.OnValueChanged(ContentCVars.MusicVolume, UpdateTitleMusicVolume);

		var volume = _cfg.GetCVar(ContentCVars.MusicVolume);

		if (_audio.TryPlaySound(MenuMusicPath, volume, out _menuMusic))
		{
			_menuMusic.IsLooping = true;
		}
	}

    protected override void Shutdown()
    {
		_cfg.UnsubValueChanged(ContentCVars.MusicVolume, UpdateTitleMusicVolume);
		_menuMusic?.StopPlaying();
		_menuMusic = null;
	}

	private void UpdateTitleMusicVolume(float newVolume)
	{
		_menuMusic?.SetVolume(newVolume);
	}
}