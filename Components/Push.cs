using Godot;
using System;
using System.Linq;

public partial class Push : Node2D
{
    // RayCast2D _raycast;
    int gridSize;

    public override void _Ready()
    {
        base._Ready();

        // _raycast = GetOwner().GetChildren().OfType<RayCast2D>().FirstOrDefault();
        gridSize = Global.Instance.gridSize;
    }

    public void PushFunction(RayCast2D originalRaycast)
    {
        RayCast2D _raycast = new();
        _raycast.Position = originalRaycast.Position;
        _raycast.TargetPosition = originalRaycast.TargetPosition;
        _raycast.CollisionMask = originalRaycast.CollisionMask;
        _raycast.CollideWithAreas = true;
        AddChild(_raycast);
        _raycast.ForceRaycastUpdate();

        if (_raycast.GetCollider() is not Area2D)
        {
            _raycast.Free();
            return;
        }
        
        var collidingArea = (Area2D)_raycast.GetCollider();
        // TODO: Consider changing check to using a 'Pushable' component
        if (collidingArea is Dog || collidingArea is FloorBox)
        {
            // Check for room to push
            Vector2 temp = _raycast.Position;
            _raycast.Position = _raycast.TargetPosition;
            _raycast.AddException(collidingArea);
            _raycast.ForceRaycastUpdate();
            if (!_raycast.IsColliding())
            {
                // GD.Print(GlobalPosition, collidingArea.GlobalPosition);
                Vector2 posDifference = GlobalPosition - collidingArea.GlobalPosition;
                Vector2 targetPosition = collidingArea.GlobalPosition;
                if (Math.Abs(posDifference.X) > Math.Abs(posDifference.Y)) // Horizontal push
                {
                    if (posDifference.X > 0) // Right
                    {
                        targetPosition.X -= gridSize;
                    }
                    else // Left
                    {
                        targetPosition.X += gridSize;
                    }
                }
                else // Vertical push
                {
                    if (posDifference.Y < 0) // Up
                    {
                        targetPosition.Y += gridSize;
                    }
                    else // Down
                    {
                        targetPosition.Y -= gridSize;
                    }
                }
                // TODO: Put this in physics process of component to lerp for smooth pushing

                collidingArea.GlobalPosition = targetPosition;
                if (collidingArea is Dog collidingDog)
                {
                    collidingDog.setDown(targetPosition);
                }

                // Updating causality
                if (collidingArea.GetNodeOrNull("Causality") != null
                    && GetNode<Level>("../../..").time == Level.Time.Past)
                {
                    collidingArea.GetNode<Causality>("Causality").UpdatePast();
                }
                else if (collidingArea.GetNodeOrNull("Causality") != null
                    && GetNode<Level>("../../..").time == Level.Time.Present)
                {
                    collidingArea.GetNode<Causality>("Causality").UpdatePresent();
                }
            }
            else
            {
                // GD.Print(_raycast.GetCollider().GetType());
            }
        }

        _raycast.Free();
    }
}
