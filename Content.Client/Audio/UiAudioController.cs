using Content.Shared.ContentCVars;
using JetBrains.Annotations;
using Robust.Client;
using Robust.Client.Audio;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Sources;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Client.Audio
{
	[UsedImplicitly]
	public sealed class UiAudioController : UIController
	{
		[Dependency] private readonly IAudioManager _audioManager = default!;
		[Dependency] private readonly IConfigurationManager _configManager = default!;
		[Dependency] private readonly IResourceCache _cache = default!;

		private IAudioSource? _clickSource;

		public override void Initialize()
		{
			base.Initialize();
			_configManager.OnValueChanged(ContentCVars.UIClickSound, SetClickSound, true);
			//_configManager.OnValueChanged(CCVars.UIHoverSound, SetHoverSound, true);
			_configManager.OnValueChanged(ContentCVars.GuiEffectsVolume, SetGuiVolume, true);
		}

		private void SetClickSound(string value)
		{
			_clickSource = null;

			if (!string.IsNullOrEmpty(value))
			{
				var resource = _cache.GetResource<AudioResource>(value);
				var source = _audioManager.CreateAudioSource(resource);

				if (source != null)
				{
					source.Volume = IoCManager.Resolve<IConfigurationManager>()
						.GetCVar(ContentCVars.GuiEffectsVolume);
					source.Global = true;
				}
				_clickSource = source;
			}
			UIManager.SetClickSound(_clickSource);
		}

		private void SetGuiVolume(float volume)
		{
			if (_clickSource is null)
			{
				return;
			}
			_clickSource.Volume = volume;
		}
	}
}
