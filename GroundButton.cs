using Godot;
using System;

public abstract partial class GroundButton : Area2D
{
    protected AnimationPlayer _animationPlayer;
    public bool pressed = false;

    AudioStreamPlayer _aspOn;
    AudioStreamPlayer _aspOff;

    public override void _Ready()
    {
        _aspOn = GetNode<AudioStreamPlayer>("SwitchOn");
        _aspOff = GetNode<AudioStreamPlayer>("SwitchOff");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AreaEntered += _OnAreaEntered;
        AreaExited += _OnAreaExited;
    }

    protected virtual void _OnAreaEntered(Area2D area)
    {
        _animationPlayer.Play("Pressed");
        _aspOn.Play();
        pressed = true;
    }

    protected virtual void _OnAreaExited(Area2D area)
    {
        if (!HasOverlappingAreas())
        {
            _animationPlayer.Play("Unpressed");
            _aspOff.Play();
            pressed = false;
        }
    }
}
