using Robust.Client.Audio;
using Robust.Client.ResourceManagement;
using Robust.Shared.Audio.Sources;
using System.Diagnostics.CodeAnalysis;

namespace Content.Client.Audio
{
	public static class AudioHelpers
	{
		public static bool TryGetAudioSource(string file, [NotNullWhen(true)] out IAudioSource? source)
		{
			source = null;
			if (!string.IsNullOrEmpty(file))
			{
				var resource = IoCManager.Resolve<IResourceCache>().GetResource<AudioResource>(file);
				source = IoCManager.Resolve<IAudioManager>().CreateAudioSource(resource);
			}
			return source is not null;
		}
	}
}
