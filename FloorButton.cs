using Godot;
using System;

public partial class FloorButton : Area2D
{
    private AnimationPlayer _animationPlayer;
    [Export] private Door _door;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AreaEntered += _OnAreaEntered;
        AreaExited += _OnAreaExited;
    }

    private void _OnAreaEntered(Area2D area)
    {
        _animationPlayer.Play("Pressed");
        _door.Call("Open");
    }

    private void _OnAreaExited(Area2D area)
    {
        if (!HasOverlappingAreas())
        {
            _animationPlayer.Play("Unpressed");
            _door.Call("Close");
        }
    }
}
