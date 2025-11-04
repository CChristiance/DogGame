using Godot;
using System;

public partial class LevelSelect : Control
{
    public override void _Ready()
    {
        var grid = GetNode("CenterContainer/GridContainer").GetChildren();
        foreach (Button button in grid)
        {
            button.Pressed += () => _OnButtonPress(button);
        }
    }

    private void _OnButtonPress(Button button)
    {
        string targetScene = "Level_";
        targetScene += button.Text + ".tscn";
        if (ResourceLoader.Exists(targetScene))
        {
            GetTree().ChangeSceneToFile(targetScene);
        }
        else
        {
            GD.Print($"Scene doesn't exist: {targetScene}");
        }
    }
}
