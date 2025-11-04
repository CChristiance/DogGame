using Godot;
using System;

public abstract partial class GroundButton : Area2D
{
    protected AnimationPlayer _animationPlayer;
    public bool pressed = false;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AreaEntered += _OnAreaEntered;
        AreaExited += _OnAreaExited;
    }

    protected virtual void _OnAreaEntered(Area2D area)
    {
        _animationPlayer.Play("Pressed");
        pressed = true;
    }

    protected virtual void _OnAreaExited(Area2D area)
    {
        if (!HasOverlappingAreas())
        {
            _animationPlayer.Play("Unpressed");
            pressed = false;
        }
    }
}
