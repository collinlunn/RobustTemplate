using Robust.Shared.Input.Binding;
using Robust.Shared.Input;
using Robust.Shared.Players;
using Content.Shared.Input;
using Robust.Client.ViewVariables;
using Robust.Client.Input;

namespace Content.Client.AccessVV
{
	public sealed class AccessVVSystem : EntitySystem
	{
		[Dependency] private readonly IClientViewVariablesManager _cvvm = default!;

		public override void Initialize()
		{
			base.Initialize();
			CommandBinds.Builder
				.Bind(ContentKeyFunctions.OpenVV, new PointerInputCmdHandler(OpenVV))
				.Register<AccessVVSystem>();
		}

		public override void Shutdown()
		{
			base.Shutdown();
			CommandBinds.Unregister<AccessVVSystem>();
		}

		private bool OpenVV(in PointerInputCmdHandler.PointerInputCmdArgs args)
		{
			_cvvm.OpenVV($"/entity/{args.EntityUid}");
			return true;
		}
	}
}
