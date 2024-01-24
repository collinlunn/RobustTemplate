using Content.Client.InGame;
using Content.Client.Lobby;
using Content.Client.MainMenu;
using Content.Shared.ContentCVars;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.State;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.Audio.Sources;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.Audio
{
	public sealed class MusicManager
	{
		[Dependency] private readonly IStateManager _stateManager = default!;
		[Dependency] private readonly IConfigurationManager _cfg = default!;

		private IAudioSource? _musicSource;

		private const string MainMenuMusicPath = "/Audio/test_music.wav";
		private const string LobbyMusicPath = "/Audio/test_music.wav";
		private const string InGameMusicPath = "/Audio/test_music.wav";

		public void Initialize()
		{
			_stateManager.OnStateChanged += UpdateSong;
			_cfg.OnValueChanged(ContentCVars.MusicVolume, UpdateVolume);
		}

		private void UpdateSong(StateChangedEventArgs args)
		{
			_musicSource?.Dispose();
			_musicSource = null;
			if (TryGetCurrentSong(args.NewState, out var file) && AudioHelpers.TryGetAudioSource(file, out var source))
			{
				source.Global = true;
				source.Volume = _cfg.GetCVar(ContentCVars.MusicVolume);
				source.Looping = true;
				_musicSource = source;
				_musicSource.StartPlaying();
			}
		}

		private void UpdateVolume(float newVolume)
		{
			if (_musicSource is not null)
				_musicSource.Volume = newVolume;
		}

		private bool TryGetCurrentSong(State gameState, [NotNullWhen(true)] out string? fileName)
		{
			fileName = gameState switch
			{
				MainMenuState => MainMenuMusicPath,
				InGameState => InGameMusicPath,
				LobbyState => LobbyMusicPath,
				_ => null,
			};
			return fileName != null;
		}
	}
}
