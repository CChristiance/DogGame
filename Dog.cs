using Godot;
using System;

public partial class Dog : Player
{
    private bool canMove = true;

    public override void _PhysicsProcess(double delta)
    {
        if (canMove)
        {
            base._PhysicsProcess(delta);
        }
    }

    public void pickUp()
    {
        canMove = false;
    }

    public void setDown(Vector2 dropPoint)
    {
        _desiredPosition = dropPoint;
        canMove = true;
    }

    public void turnOld()
    {
        _animationPlayer.Play("idle_old");
        canMove = false;
    }

    public void turnYoung()
    {
        _animationPlayer.Play("idle_side");
        canMove = true;
    }
}
