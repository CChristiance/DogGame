using Godot;
using System;

public partial class Human : Player
{
    private Area2D _carriedObject = null;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_carriedObject != null)
        {
            _carriedObject.Position = new Vector2(Position.X, Position.Y - 12);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("grab"))
        {
            if (_carriedObject == null && _raycast.IsColliding()) // Hands empty - pick up
            {
                _carriedObject = (Area2D)_raycast.GetCollider();
                _carriedObject.SetCollisionLayerValue(2, false); // Disable object's collision
                if (_carriedObject is Dog _carriedDog)
                {
                    _carriedDog.pickUp();
                }
            }
            else if (!_raycast.IsColliding()) // Hands full - place something down
            {
                Vector2 dropPoint;
                if (direction == Direction.Left)
                {
                    dropPoint = Position + -_raycast.TargetPosition;
                }
                else
                {
                    dropPoint = Position + _raycast.TargetPosition;
                }
                _carriedObject.Position = dropPoint;

                if (_carriedObject is Dog _carriedDog)
                {
                    _carriedDog.setDown(dropPoint);
                }

                _carriedObject.SetCollisionLayerValue(2, true);
                _carriedObject = null;
            }
            else
            {
                GD.Print(_raycast.GetCollider());
            }
        }
    }
}
