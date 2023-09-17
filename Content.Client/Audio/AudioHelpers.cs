using Content.Shared.ContentCVars;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Audio;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Client.Audio
{
	static class AudioHelpers
	{
		private static string GuiEffectRoot = "/Audio/";

		public static void TryPlayGuiEffect(string fileName, AudioParams? audioParams = null)
		{
			IoCManager.Resolve<IEntitySystemManager>()
				.GetEntitySystem<SharedAudioSystem>()
				.PlayGlobal(GuiEffectRoot + fileName, Filter.Local(), false, audioParams: audioParams ?? DefaultGuiEffectsAudioParams);
		}

		public static void AddButtonSound(string fileName, IEnumerable<BaseButton> buttons, AudioParams? audioParams = null)
		{
			foreach (var button in buttons)
			{
				button.OnPressed += _ => TryPlayGuiEffect(fileName, audioParams);
			}
		}

		public static AudioParams DefaultGuiEffectsAudioParams
		{
			get
			{
				return new AudioParams
				{
					Volume = IoCManager.Resolve<IConfigurationManager>()
						.GetCVar(ContentCVars.GuiEffectsVolume)
				};
			}
		}

		public static AudioParams DefaultGameEffectsAudioParams
		{
			get
			{
				return new AudioParams
				{
					Volume = IoCManager.Resolve<IConfigurationManager>()
						.GetCVar(ContentCVars.GameEffectsVolume)
				};
			}
		}
	}
}
