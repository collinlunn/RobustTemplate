using Content.Shared.ContentCVars;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Shared.Configuration;

namespace Content.Client.Audio
{
	/// <summary>
	///		Adjusts incoming sounds fom the server by the client game effects volume.
	///		This assumes that all SFX triggered by the server audio system will be game effects,
	///		and not music, ambience, gui effects, etc.
	/// </summary>
	[UsedImplicitly]
	public sealed class ContentAudioSystem : EntitySystem
	{
		[Dependency] private readonly IConfigurationManager _cfg = default!;

		private float ClientVolume { get => _cfg.GetCVar(ContentCVars.GameEffectsVolume); }

		public override void Initialize()
		{
			base.Initialize();
			SubscribeNetworkEvent<PlayAudioGlobalMessage>(InterceptVolume, before: new[] { typeof(AudioSystem) });
			SubscribeNetworkEvent<PlayAudioEntityMessage>(InterceptVolume, before: new[] { typeof(AudioSystem) });
			SubscribeNetworkEvent<PlayAudioPositionalMessage>(InterceptVolume, before: new[] { typeof(AudioSystem) });
		}

		private void InterceptVolume(AudioMessage args)
		{
			args.AudioParams = args.AudioParams.AddVolume(ClientVolume);
		}
	}
}
