using Godot;
using System;

public partial class Door : Area2D
{
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AreaEntered += _OnAreaEntered;
        AreaExited += _OnAreaExited;
    }

    public void Open()
    {
        _animationPlayer.Play("Open");
    }

    public void Close()
    {
        _animationPlayer.Play("Closed");
    }

    private void _OnAreaEntered(Area2D area)
    {

    }

    private void _OnAreaExited(Area2D area)
    {
        
    }
}
