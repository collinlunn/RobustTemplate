using Content.Shared.ContentCVars;
using Robust.Client;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Client.Audio
{
	static class AudioHelpers
	{
		private static string GuiEffectRoot = "/Audio/";

		private static float GuiEffectVolume
		{
			get => IoCManager.Resolve<IConfigurationManager>()
				.GetCVar(ContentCVars.GuiEffectsVolume);
		}

		public static void TryPlayGuiEffect(string fileName, AudioParams? audioParams = null)
		{
			var audioParamsAdjusted = (audioParams ?? AudioParams.Default)
				.AddVolume(GuiEffectVolume);

			var runLevel = IoCManager.Resolve<IBaseClient>().RunLevel;

			if (runLevel < ClientRunLevel.Connected)
				return;

			IoCManager.Resolve<IEntitySystemManager>()
				.GetEntitySystem<SharedAudioSystem>()
				.PlayGlobal(GuiEffectRoot + fileName, Filter.Local(), false, audioParams: audioParamsAdjusted);
		}

		public static void AddButtonSound(string fileName, IEnumerable<BaseButton> buttons, AudioParams? audioParams = null)
		{
			foreach (var button in buttons)
			{
				button.OnPressed += _ => TryPlayGuiEffect(fileName, audioParams);
			}
		}
	}
}
