using Godot;
using System;

public partial class Causality : Node2D
{
    // Future actions don't affect the past, but past actions affect the future.
    private Vector2 _pastPos, _presentPos;

    public override void _Ready()
    {
        base._Ready();
        _presentPos = GetParent<Node2D>().Position;
        _pastPos = _presentPos;
    }

    // Use when the past is changed (i.e. object is interacted in the past)
    public void UpdatePast()
    {
        _pastPos = GetParent<Node2D>().Position;
        _presentPos = _pastPos;
    }

    public void UpdatePresent()
    {
        _presentPos = GetParent<Node2D>().Position;
    }

    public void ToPast()
    {
        GetParent<Node2D>().Position = _pastPos;
    }

    public void ToPresent()
    {
        GetParent<Node2D>().Position = _presentPos;
    }
}
