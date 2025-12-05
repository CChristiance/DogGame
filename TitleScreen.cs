using Godot;
using System;

public partial class TitleScreen : Control
{
    AudioStreamPlayer _aspHover;
    AudioStreamPlayer _aspClick;

    public override void _Ready()
    {
        _aspHover = GetNode<AudioStreamPlayer>("Hover");
        _aspClick = GetNode<AudioStreamPlayer>("Click");

        var grid = GetNode("CenterContainer/GridContainer").GetChildren();
        foreach (Button button in grid)
        {
            button.MouseEntered += () => _OnButtonHover();
            button.Pressed += () => _OnButtonPress(button);
        }
    }

    private void _OnButtonHover()
    {
        _aspHover.Play();
    }

    private void _OnButtonPress(Button button)
    {
        _aspClick.Play();

        if (button.Text == "Start" && ResourceLoader.Exists("LevelSelect.tscn"))
        {
            GetTree().ChangeSceneToFile("LevelSelect.tscn");
        }
        else if (button.Text == "Start")
        {
            GD.Print($"Scene doesn't exist: {"LevelSelect.tscn"}");
        }
        else if (button.Text == "Credits" && ResourceLoader.Exists("CreditsScreen.tscn"))
        {
            GetTree().ChangeSceneToFile("CreditsScreen.tscn");
        }
        else if (button.Text == "Credits")
        {
            GD.Print($"Scene doesn't exist: {"CreditsScreen.tscn"}");
        }
    }
}
