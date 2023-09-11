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
		_cfg.OnValueChanged(ContentCVars.TitleMusicVolume, UpdateTitleMusicVolume);

		var volume = _cfg.GetCVar(ContentCVars.TitleMusicVolume);

		if (_audio.TryPlaySound(MenuMusicPath, out _menuMusic))
		{
			_menuMusic.SetVolumeDirect(volume);
			_menuMusic.IsLooping = true;
		}
	}

    protected override void Shutdown()
    {
		_cfg.UnsubValueChanged(ContentCVars.TitleMusicVolume, UpdateTitleMusicVolume);
		_menuMusic?.StopPlaying();
		_menuMusic = null;
	}

	private void UpdateTitleMusicVolume(float newVolume)
	{
		_menuMusic?.SetVolumeDirect(newVolume);
	}
}