using Content.Client.UI;
using Content.Shared.ContentCVars;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Client.InGame;

[GenerateTypedNameReferences]
public sealed partial class InGameHUDScreen : UIScreen
{
	[Dependency] private readonly IGameTiming _gameTiming = default!;
	[Dependency] private readonly IEyeManager _eyeManager = default!;
	[Dependency] private readonly IConfigurationManager _configurationManager = default!;
	[Dependency] private readonly IClientNetManager _netManager = default!;

	private MainViewportContainer _viewport;

	public InGameHUDScreen()
	{
		RobustXamlLoader.Load(this);
		IoCManager.InjectDependencies(this);

		_viewport = new MainViewportContainer(_eyeManager);
		CustomCursor.SetCursor(_viewport);
		_eyeManager.MainViewport = _viewport;

		ViewportBox.AddChild(_viewport);
		FpsBox.AddChild(new FpsCounter(_gameTiming));
		SetAnchorPreset(ViewportBox, LayoutPreset.Wide);
		SetAnchorPreset(_viewport, LayoutPreset.Wide);

		FpsBox.Visible = _configurationManager.GetCVar(ContentCVars.HudFpsVisible);
		PingLabel.Visible = _configurationManager.GetCVar(ContentCVars.HudPingVisible);

		_configurationManager.OnValueChanged(ContentCVars.HudFpsVisible, SetFpsVisible);
		_configurationManager.OnValueChanged(ContentCVars.HudPingVisible, SetPingVisible);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		if (!disposing)
		{
			return;
		}
		_configurationManager.UnsubValueChanged(ContentCVars.HudFpsVisible, SetFpsVisible);
		_configurationManager.UnsubValueChanged(ContentCVars.HudPingVisible, SetPingVisible);
	}

	protected override void FrameUpdate(FrameEventArgs args)
	{
		base.FrameUpdate(args);
		PingLabel.Text = $"Ping: {_netManager.ServerChannel?.Ping.ToString() ?? "Not Connected" }";
	}

	private void SetFpsVisible(bool visible)
	{
		FpsBox.Visible = visible;
	}
	private void SetPingVisible(bool visible)
	{
		PingLabel.Visible = visible;
	}
}
