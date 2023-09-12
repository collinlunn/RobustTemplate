using Content.Shared.ContentCVars;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Configuration;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.Audio
{
	/// <summary>
	///		Helpers to play audio in the main menu since the audio entity system does not load until connecting to a server.
	/// </summary>
	public sealed class MainMenuAudio
	{
		[Dependency] private readonly IResourceCache _resourceCache = default!;
		[Dependency] private readonly IClydeAudio _clyde = default!;
		[Dependency] private readonly ILogManager _logMan = default!;
		[Dependency] private readonly IConfigurationManager _cfg = default!;

		private ISawmill _sawmill = default!;

		private const string FileRoot = "/Audio/";
		private const string SawmillName = "mmaudio";


		public void Initialize()
		{
			_sawmill = _logMan.GetSawmill(SawmillName);
		}

		public bool TryPlaySound(string filePath, float gain, [NotNullWhen(true)] out IClydeAudioSource? source)
		{
			source = null;

			if (!_resourceCache.TryGetResource<AudioResource>(new ResPath(FileRoot + filePath), out var audio))
			{
				_sawmill.Error($"Could not find audio file \"{filePath}\"");
				return false;
			}

			source = _clyde.CreateAudioSource(audio);

			if (source == null)
			{
				_sawmill.Error($"{nameof(IClydeAudioSource)} could not be made from \"{filePath}\"");
				return false;
			}

			source.SetVolumeDirect(gain);
			source.StartPlaying();
			return true;
		}

		public void AddButtonSound(string filePath, params Button[] buttons)
		{
			foreach (var button in buttons)
			{
				button.OnPressed += _ =>
				{
					var volume = _cfg.GetCVar(ContentCVars.GuiEffectsVolume);
					TryPlaySound(filePath, volume, out var stream);
				};
			}
		}
	}
}