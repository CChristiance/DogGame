using Godot;
using System;

public partial class CreditsScreen : Control
{
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("escape"))
        {
            GetTree().ChangeSceneToFile("TitleScreen.tscn");
        }
    }
}
