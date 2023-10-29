﻿using Content.Client.Audio;
using Content.Shared.ContentCVars;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Client.OptionsMenu
{
	[GenerateTypedNameReferences]
	public sealed partial class GraphicsTab : Control
	{
		[Dependency] private readonly IConfigurationManager _cfg = default!;
		[Dependency] private readonly MainMenuAudio _audio = default!;

		private readonly List<float> _uiSizes = new() { 0.5f, 1, 2 };

		private RadioOptions<float> _uiSizeRadio;

		public GraphicsTab()
		{
			RobustXamlLoader.Load(this);
			IoCManager.InjectDependencies(this);

			ApplyButton.OnPressed += _ => ApplyPressed();
			_audio.AddButtonSound("pop.wav",
				ApplyButton,
				VSyncCheckBox,
				FullscreenCheckBox,
				FpsCounterCheckBox,
				PingCounterCheckBox);

			_uiSizeRadio = new RadioOptions<float>(RadioOptionsLayout.Horizontal);
			_uiSizeRadio.OnItemSelected += args => _uiSizeRadio.Select(args.Id);

			_uiSizeRadio.AddItem($"Auto ({UserInterfaceManager.DefaultUIScale*100}%)", 0);
			foreach (var size in _uiSizes)
			{
				_uiSizeRadio.AddItem($"{size * 100}%", size);
			}
			UiSizeBox.AddChild(_uiSizeRadio);

			UpdateButtons();
		}

		public void UpdateButtons()
		{
			VSyncCheckBox.Pressed = _cfg.GetCVar(CVars.DisplayVSync);
			FullscreenCheckBox.Pressed = _cfg.GetCVar(CVars.DisplayWindowMode) == (int)WindowMode.Fullscreen;
			FpsCounterCheckBox.Pressed = _cfg.GetCVar(ContentCVars.HudFpsVisible);
			PingCounterCheckBox.Pressed = _cfg.GetCVar(ContentCVars.HudPingVisible);
			_uiSizeRadio.SelectByValue(_cfg.GetCVar(CVars.DisplayUIScale));
		}

		private void ApplyPressed()
		{
			_cfg.SetCVar(CVars.DisplayVSync, VSyncCheckBox.Pressed);
			_cfg.SetCVar(CVars.DisplayWindowMode,
						 (int)(FullscreenCheckBox.Pressed ? WindowMode.Fullscreen : WindowMode.Windowed));
			_cfg.SetCVar(ContentCVars.HudFpsVisible, FpsCounterCheckBox.Pressed);
			_cfg.SetCVar(ContentCVars.HudPingVisible, PingCounterCheckBox.Pressed);
			_cfg.SetCVar(CVars.DisplayUIScale, _uiSizeRadio.SelectedValue);
		}
	}
}
