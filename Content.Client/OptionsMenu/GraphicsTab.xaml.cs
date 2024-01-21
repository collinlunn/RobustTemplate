﻿using Content.Client.Audio;
using Content.Shared.ContentCVars;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.Utility;
using static Robust.Client.Input.Mouse;

namespace Content.Client.OptionsMenu
{
	[GenerateTypedNameReferences]
	public sealed partial class GraphicsTab : Control
	{
		[Dependency] private readonly IConfigurationManager _cfg = default!;

		private readonly List<float> _uiSizes = new() { 0.5f, 1, 1.5f, 2 };

		private ContentRadioOptions<float> _uiSizeRadio;

		public GraphicsTab()
		{
			RobustXamlLoader.Load(this);
			IoCManager.InjectDependencies(this);

			ApplyButton.OnPressed += _ => ApplyPressed();
			ResetButton.OnPressed += _ => Reset();

			_uiSizeRadio = new();
			_uiSizeRadio.AddButton($"OS Auto: { UserInterfaceManager.DefaultUIScale * 100}%", 0);
			foreach (var size in _uiSizes)
			{
				_uiSizeRadio.AddButton($"{size * 100}%", size);
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
			
			var setRadio = _uiSizeRadio.SetValue(_cfg.GetCVar(CVars.DisplayUIScale));
			DebugTools.Assert(setRadio);
		}

		private void ApplyPressed()
		{
			_cfg.SetCVar(CVars.DisplayVSync, VSyncCheckBox.Pressed);
			_cfg.SetCVar(CVars.DisplayWindowMode,
						 (int)(FullscreenCheckBox.Pressed ? WindowMode.Fullscreen : WindowMode.Windowed));
			_cfg.SetCVar(ContentCVars.HudFpsVisible, FpsCounterCheckBox.Pressed);
			_cfg.SetCVar(ContentCVars.HudPingVisible, PingCounterCheckBox.Pressed);
			_cfg.SetCVar(CVars.DisplayUIScale, _uiSizeRadio.SelectedValue);
			_cfg.SaveToFile();
		}

		private void Reset()
		{
			_cfg.SetCVar(CVars.DisplayVSync, CVars.DisplayVSync.DefaultValue);
			_cfg.SetCVar(CVars.DisplayWindowMode, CVars.DisplayWindowMode.DefaultValue);
			_cfg.SetCVar(ContentCVars.HudFpsVisible, ContentCVars.HudFpsVisible.DefaultValue);
			_cfg.SetCVar(ContentCVars.HudPingVisible, ContentCVars.HudPingVisible.DefaultValue);
			_cfg.SetCVar(CVars.DisplayUIScale, CVars.DisplayUIScale.DefaultValue);
			UpdateButtons();
			_cfg.SaveToFile();
		}

		public void OnClosed()
		{
			//if apply wasn't pressed, the buttons will look incorrect on next open
			UpdateButtons();
		}
	}
}
