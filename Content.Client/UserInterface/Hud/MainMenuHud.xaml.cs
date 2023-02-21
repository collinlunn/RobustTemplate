using System;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Hud;

[GenerateTypedNameReferences]
public sealed partial class MainMenuHud : Control
{
    public event Action<BaseButton.ButtonEventArgs>? OnConnectButtonPressed
    {
        add => ConnectButton.OnPressed += value;
        remove => ConnectButton.OnPressed -= value;
    }

    public string Username
    {
        get => UsernameLineEdit.Text;
        set => UsernameLineEdit.Text = value;
    }

    public string Address
    {
        get => AddressLineEdit.Text;
        set => AddressLineEdit.Text = value;
    }

}