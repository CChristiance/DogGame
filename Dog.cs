using Godot;
using System;

public partial class Dog : Player
{
    private bool isCarried = false;
    private bool isOld = true;
    private float _t;

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
            _t = 0.5f;
        }
        else if (isOld && !isCarried)
        {
            if (!Position.IsEqualApprox(_desiredPosition))
            {
                _t += (float)delta * 0.999f;
                Position = Position.Lerp(_desiredPosition, _t);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionPressed("dog_interact"))
        {
            push.PushFunction(_raycast);
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
