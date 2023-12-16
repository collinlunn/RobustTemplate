using Content.Client.InGame;
using Content.Client.Lobby;
using Content.Shared.ContentCVars;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.State;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.Audio
{
	[UsedImplicitly]
	public sealed class MusicSystem : EntitySystem
	{
		[Dependency] private readonly IStateManager _stateManager = default!;
		[Dependency] private readonly SharedAudioSystem _audio = default!;
		[Dependency] private readonly IConfigurationManager _cfg = default!;

		private const string LobbyMusicPath = "/Audio/test_music.wav";
		private const string InGameMusicPath = "/Audio/test_music.wav";

		private (EntityUid Stream, AudioComponent Component)? _currentMusic = default;

		public override void Initialize()
		{
			base.Initialize();
			_stateManager.OnStateChanged += OnStateChanged;
			_cfg.OnValueChanged(ContentCVars.MusicVolume, UpdateVolume);
		}

		public override void Shutdown()
		{
			base.Shutdown();
			_stateManager.OnStateChanged -= OnStateChanged;
			_cfg.UnsubValueChanged(ContentCVars.MusicVolume, UpdateVolume);
			ClearSong();
		}

		private void OnStateChanged(StateChangedEventArgs args)
		{
			ClearSong();
			if (TryGetCurrentSong(args.NewState, out var file))
			{
				var musicParams = new AudioParams
				{
					Volume = _cfg.GetCVar(ContentCVars.MusicVolume),
					Loop = true,
				};
				_currentMusic = _audio.PlayGlobal(file, Filter.Local(), false, audioParams: musicParams);
			}
		}

		private void UpdateVolume(float newVolume)
		{
			_audio.SetVolume(_currentMusic?.Stream, newVolume, component: _currentMusic?.Component);
		}

		private void ClearSong()
		{
			_audio.Stop(_currentMusic?.Stream, _currentMusic?.Component);
			_currentMusic = null;
		}

		private bool TryGetCurrentSong(State gameState, [NotNullWhen(true)] out string? fileName)
		{
			fileName = gameState switch
			{
				InGameState => InGameMusicPath,
				LobbyState => LobbyMusicPath,
				_ => null,
			};
			return fileName != null;
		}
	}
}
