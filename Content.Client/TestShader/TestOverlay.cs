using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;

namespace Content.Client.TestShader
{
	public sealed class TestOverlay : Overlay
	{
		[Dependency] private readonly IPrototypeManager _prototypeManager = default!;

		public override bool RequestScreenTexture => true;

		public override OverlaySpace Space => OverlaySpace.WorldSpace;

		private readonly ShaderInstance _testSourceShader;

		private readonly ShaderInstance _testCanvasShader;

		private bool _shaderEnabled = false;

		public TestOverlay()
		{
			IoCManager.InjectDependencies(this);
			_testSourceShader = _prototypeManager.Index<ShaderPrototype>("test_source_shader").InstanceUnique();
			_testCanvasShader = _prototypeManager.Index<ShaderPrototype>("test_canvas_shader").InstanceUnique();
		}

		protected override bool BeforeDraw(in OverlayDrawArgs args)
		{
			return _shaderEnabled;
		}

		protected override void Draw(in OverlayDrawArgs args)
		{
			if (ScreenTexture == null)
				return;
			
			var handle = args.WorldHandle;
			var viewport = args.WorldBounds;

			_testSourceShader?.SetParameter("TEST_UNIFORM", ScreenTexture);
			handle.UseShader(_testSourceShader);
			handle.DrawRect(viewport, Color.White);

			handle.UseShader(null);
		}
	}
}