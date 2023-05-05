using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.UI
{
	public abstract class SharedStateUiConnection
	{
		public SharedStateUiConnection()
		{
			IoCManager.InjectDependencies(this);
		}

		public const uint PreInitId = 0;

		public uint Id { get; set; } = PreInitId;
	}
}
