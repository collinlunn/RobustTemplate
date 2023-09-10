using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.Audio
{
	/// <summary>
	///		Helpers to play audio in the main menu since the audio entity system does not load until connecting to a server
	/// </summary>
	public sealed class MainMenuAudio
	{
		[Dependency] private readonly IResourceCache _resourceCache = default!;
		[Dependency] private readonly IClydeAudio _clyde = default!;

		private const string FileRoot = "/Audio/";

		public bool TryPlaySound(string filePath, [NotNullWhen(true)] out IClydeAudioSource? source)
		{
			source = null;

			if (!_resourceCache.TryGetResource<AudioResource>(new ResPath(FileRoot + filePath), out var audio))
				return false;

			source = _clyde.CreateAudioSource(audio);

			if (source == null) //CreateAudioSource can return null
				return false;

			source.StartPlaying();

			return true;
		}
	}
}