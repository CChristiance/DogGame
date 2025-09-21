using Godot;
using System;

public partial class Main : Control
{
    [Export] private Button _button;

    public override void _Ready()
    {
        _button.Pressed += _OnButtonPress;
    }

    private void _OnButtonPress()
    {
        GetTree().ChangeSceneToFile("Level_1.tscn");
    }
}
