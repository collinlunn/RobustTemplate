using Content.Client.Audio;
using Robust.Client.Graphics;
using Robust.Client.State;

namespace Content.Client.MainMenu;

public sealed class MainMenuState : State
{
	[Dependency] private readonly MainMenuAudio _audio = default!;

	private IClydeAudioSource? _menuMusic = null;

	private const string MenuMusicPath = "test_music.wav";

	protected override void Startup()
    {
		if (_audio.TryPlaySound(MenuMusicPath, out _menuMusic))
		{
			_menuMusic.IsLooping = true;
		}
	}

    protected override void Shutdown()
    {
		_menuMusic?.StopPlaying();
		_menuMusic = null;
	}
}