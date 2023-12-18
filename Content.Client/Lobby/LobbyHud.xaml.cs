using Content.Client.Audio;
using Content.Client.UI;
using Content.Shared.Lobby;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Lobby;

[GenerateTypedNameReferences]
public sealed partial class LobbyHud : Control
{
	public LobbyHud()
	{
		RobustXamlLoader.Load(this);
		IoCManager.InjectDependencies(this);

		AudioHelpers.AddButtonSound(AudioHelpers.PresetSoundFiles.Pop, new List<BaseButton>
		{
			StartGameButton,
			StartMappingButton,
			JoinGameButton,
			OptionsButton,
			DisconnectButton
		});

		CustomCursor.SetCursor(this);
	}

	public void SetDefaultState()
	{
		ConnectedPlayersBox.DisposeAllChildren();
		ConnectedPlayersBox.AddChild(new Label
		{
			Text = "Loading..."
		});
	}

	public void SetState(LobbyUiState state)
	{
		ConnectedPlayersBox.DisposeAllChildren();

		foreach (var player in state.ConnectedPlayers)
		{
			ConnectedPlayersBox.AddChild(new Label
			{
				Text = player
			});
		}

		StartGameButton.Visible = !state.GameStarted;
		StartMappingButton.Visible = !state.GameStarted;
		JoinGameButton.Visible = state.GameStarted;
	}
}