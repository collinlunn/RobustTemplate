using Content.Client.Audio;
using Content.Shared.ContentCVars;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared;
using Robust.Shared.Configuration;
using System;
using Range = Robust.Client.UserInterface.Controls.Range;

namespace Content.Client.OptionsMenu
{
	[GenerateTypedNameReferences]
	public sealed partial class AudioTab : Control
	{
		[Dependency] private readonly IConfigurationManager _cfg = default!;
		[Dependency] private readonly MainMenuAudio _audio = default!;

		public AudioTab()
		{
			RobustXamlLoader.Load(this);
			IoCManager.InjectDependencies(this);

			ApplyButton.OnPressed += _ => ApplyPressed();
			_audio.AddButtonSound("pop.wav", ApplyButton);

			MasterVolumeSlider.Value = _cfg.GetCVar(CVars.AudioMasterVolume) * 100;
			MusicVolumeSlider.Value = DBToLV100(_cfg.GetCVar(ContentCVars.MusicVolume));
			GuiEffectsVolumeSlider.Value = DBToLV100(_cfg.GetCVar(ContentCVars.GuiEffectsVolume));
			GameEffectsVolumeSlider.Value = DBToLV100(_cfg.GetCVar(ContentCVars.GameEffectsVolume));
			AmbienceVolumeSlider.Value = DBToLV100(_cfg.GetCVar(ContentCVars.AmbienceVolume));

			CurrentMasterVolumeLabel.Text = $"{MasterVolumeSlider.Value}%";
			CurrentMusicVolumeLabel.Text = $"{MusicVolumeSlider.Value}%";
			CurrentGuiEffectsVolumeLabel.Text = $"{GuiEffectsVolumeSlider.Value}%";
			CurrentGameEffectsVolumeLabel.Text = $"{GameEffectsVolumeSlider.Value}%";
			CurrentAmbienceVolumeLabel.Text = $"{AmbienceVolumeSlider.Value}%";

			MasterVolumeSlider.OnValueChanged += range => { CurrentMasterVolumeLabel.Text = $"{range.Value}%"; };
			MusicVolumeSlider.OnValueChanged += range => { CurrentMusicVolumeLabel.Text = $"{range.Value}%"; };
			GuiEffectsVolumeSlider.OnValueChanged += range => { CurrentGuiEffectsVolumeLabel.Text = $"{range.Value}%"; };
			GameEffectsVolumeSlider.OnValueChanged += range => { CurrentGameEffectsVolumeLabel.Text = $"{range.Value}%"; };
			AmbienceVolumeSlider.OnValueChanged += range => { CurrentAmbienceVolumeLabel.Text = $"{range.Value}%"; };

		}

		private void ApplyPressed()
		{
			_cfg.SetCVar(CVars.AudioMasterVolume, MasterVolumeSlider.Value / 100);
			_cfg.SetCVar(ContentCVars.MusicVolume, LV100ToDB(MusicVolumeSlider.Value));
			_cfg.SetCVar(ContentCVars.GuiEffectsVolume, LV100ToDB(GuiEffectsVolumeSlider.Value));
			_cfg.SetCVar(ContentCVars.GameEffectsVolume, LV100ToDB(GameEffectsVolumeSlider.Value));
			_cfg.SetCVar(ContentCVars.AmbienceVolume, LV100ToDB(AmbienceVolumeSlider.Value));
		}

		private float DBToLV100(float db, float multiplier = 1f)
		{
			var lvl100 = (float)(Math.Pow(10, db / 10) * 100 / multiplier);
			return lvl100;
		}

		private float LV100ToDB(float lv100, float multiplier = 1f)
		{
			// Saving negative infinity doesn't work, so use -10000000 instead (MidiManager does it)
			var db = MathF.Max(-10000000, (float)(Math.Log(lv100 * multiplier / 100, 10) * 10));
			return db;
		}
	}
}
