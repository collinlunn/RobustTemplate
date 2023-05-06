using Content.Shared.UI;

namespace Content.Client.UI
{
	public sealed class UiConnectionLoadedEvent : EntityEventArgs
	{
		public UiState State { get; }

		public UiConnectionLoadedEvent(UiState state)
		{
			State = state;
		}
	}

	public sealed class UiConnectionUnloadedEvent : EntityEventArgs
	{

	}

	public sealed class UiStateReceivedEvent : EntityEventArgs
	{
		public UiState State { get; }

		public UiStateReceivedEvent(UiState state)
		{
			State = state;
		}
	}
}
