using Content.Shared.ContentCVars;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;
using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Client.OptionsMenu
{
	[GenerateTypedNameReferences]
	public sealed partial class AudioTab : Control
	{
		[Dependency] private readonly IConfigurationManager _cfg = default!;

		public AudioTab()
		{
			RobustXamlLoader.Load(this);
			IoCManager.InjectDependencies(this);

			ApplyButton.OnPressed += _ => ApplyPressed();

			MasterVolumeSlider.Value = _cfg.GetCVar(CVars.AudioMasterVolume) * 100;
			TitleMusicVolumeSlider.Value = _cfg.GetCVar(ContentCVars.TitleMusicVolume) * 100;

			CurrentMasterVolumeLabel.Text = $"{MasterVolumeSlider.Value}%";
			CurrentTitleMusicVolumeLabel.Text = $"{TitleMusicVolumeSlider.Value}%";

			MasterVolumeSlider.OnValueChanged += range => { CurrentMasterVolumeLabel.Text = $"{range.Value}%"; };
			TitleMusicVolumeSlider.OnValueChanged += range => { CurrentTitleMusicVolumeLabel.Text = $"{range.Value}%"; };
		}

		private void ApplyPressed()
		{
			_cfg.SetCVar(CVars.AudioMasterVolume, MasterVolumeSlider.Value / 100);
			_cfg.SetCVar(ContentCVars.TitleMusicVolume, TitleMusicVolumeSlider.Value / 100);
		}
	}
}
