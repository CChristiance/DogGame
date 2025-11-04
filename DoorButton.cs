using Godot;
using System;

public partial class DoorButton : GroundButton
{
    private DoorButtonHandler _doorButtonHandler;

    public override void _Ready()
    {
        base._Ready();
        _doorButtonHandler = GetNode<DoorButtonHandler>("../");
    }

    protected override void _OnAreaEntered(Area2D area)
    {
        base._OnAreaEntered(area);
        _doorButtonHandler.UpdateState();
    }

    protected override void _OnAreaExited(Area2D area)
    {
        base._OnAreaExited(area);
        if (!HasOverlappingAreas())
        {
            _doorButtonHandler.UpdateState();
        }
    }
}
