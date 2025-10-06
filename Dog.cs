using Godot;
using System;

public partial class Dog : Player
{
    private bool isCarried = false;
    private bool isOld = true;

    public override void _Ready()
    {
        base._Ready();
        turnOld();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!isOld && !isCarried)
        {
            base._PhysicsProcess(delta);
        }
    }

    public void pickUp()
    {
        isCarried = true;
    }

    public void setDown(Vector2 dropPoint)
    {
        _lastPosition = dropPoint;
        _desiredPosition = dropPoint;
        isCarried = false;
    }

    public void turnOld()
    {
        _animationPlayer.Play("idle_old");
        isOld = true;
    }

    public void turnYoung()
    {
        _animationPlayer.Play("idle_side");
        isOld = false;
    }
}
