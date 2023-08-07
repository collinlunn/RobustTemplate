using Robust.Client.Graphics;

namespace Content.Client.TestShader
{
	public sealed class TestShaderSystem : EntitySystem
	{
		[Dependency] private readonly IOverlayManager _overlayMan = default!;

		private Overlay _testOverlay = default!;

		public override void Initialize()
		{
			base.Initialize();
			_testOverlay = new TestOverlay();
			_overlayMan.AddOverlay(_testOverlay);
		}

		public override void Shutdown()
		{
			base.Shutdown();
			_overlayMan.RemoveOverlay(_testOverlay);
		}
	}
}
