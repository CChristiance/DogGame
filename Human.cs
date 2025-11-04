using Godot;
using System;
using System.Linq;

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
        base._Input(@event);

        if (@event.IsActionPressed("grab"))
        // TODO: Move use check for Carryable component
        {
            if (_carriedObject == null && _raycast.IsColliding()) // Hands empty - pick up
            {
                var collidingArea = (Area2D)_raycast.GetCollider();
                if (collidingArea is Dog || collidingArea is FloorBox)
                {
                    _carriedObject = collidingArea;
                    _carriedObject.SetCollisionLayerValue(2, false); // Disable object's collision
                    if (_carriedObject is Dog _carriedDog)
                    {
                        _carriedDog.pickUp();
                    }
                }
            }
            else if (_carriedObject != null && !_raycast.IsColliding()
                    && _canHorizontal && _canVertical) // Hands full - place something down
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

                if (_carriedObject.GetNodeOrNull("Causality") != null
                    && GetNode<Level>("../..").time == Level.Time.Past)
                {
                    _carriedObject.GetNode<Causality>("Causality").UpdatePast();
                }
                else if (_carriedObject.GetNodeOrNull("Causality") != null
                    && GetNode<Level>("../..").time == Level.Time.Present)
                {
                    _carriedObject.GetNode<Causality>("Causality").UpdatePresent();
                }

                _carriedObject = null;
            }
            else
            {
                // GD.Print(_raycast.GetCollider());
            }
        }
        else if (@event.IsActionPressed("interact"))
        {
            push.PushFunction(_raycast);
            var collidingArea = (Area2D)_raycast.GetCollider();
            if (collidingArea is Door collidingDoor)
            {
                // Can only exit level when holding dog :)
                if (_carriedObject is Dog)
                {
                    collidingDoor.Interact();
                }
            }
        }
        // else if (@event.IsActionPressed("interact"))
        // {
        //     var collidingArea = (Area2D)_raycast.GetCollider();
        //     // TODO: Consider changing check to using a 'Pushable' component
        //     // TODO: Move this code into a 'Push' component so the dog can push, too
        //     // TODO: Bug when pushing dog due to lastPosition and desiredPosition shenanigans
        //     if (collidingArea is Dog || collidingArea is FloorBox)
        //     {
        //         // Check for room to push
        //         Vector2 temp = _raycast.Position;
        //         _raycast.Position = _raycast.TargetPosition;
        //         _raycast.AddException(collidingArea);
        //         _raycast.ForceRaycastUpdate();
        //         if (!_raycast.IsColliding())
        //         {
        //             Vector2 posDifference = Position - collidingArea.Position;
        //             Vector2 targetPosition = collidingArea.Position;
        //             if (Math.Abs(posDifference.X) > Math.Abs(posDifference.Y)) // Horizontal push
        //             {
        //                 if (posDifference.X > 0) // Right
        //                 {
        //                     targetPosition.X -= gridSize;
        //                 }
        //                 else // Left
        //                 {
        //                     targetPosition.X += gridSize;
        //                 }
        //             }
        //             else // Vertical push
        //             {
        //                 if (posDifference.Y < 0) // Up
        //                 {
        //                     targetPosition.Y += gridSize;
        //                 }
        //                 else // Down
        //                 {
        //                     targetPosition.Y -= gridSize;
        //                 }
        //             }
        //             // TODO: Put this in physics process of component to lerp for smooth pushing

        //             collidingArea.Position = targetPosition;
        //             if (collidingArea is Dog collidingDog)
        //             {
        //                 collidingDog.setDown(targetPosition);
        //             }

        //             // Updating causality
        //             if (collidingArea.GetNodeOrNull("Causality") != null
        //                 && GetNode<Level>("../..").time == Level.Time.Past)
        //             {
        //                 collidingArea.GetNode<Causality>("Causality").UpdatePast();
        //             }
        //             else if (collidingArea.GetNodeOrNull("Causality") != null
        //                 && GetNode<Level>("../..").time == Level.Time.Present)
        //             {
        //                 collidingArea.GetNode<Causality>("Causality").UpdatePresent();
        //             }
        //         }
        //         else
        //         {
        //             GD.Print(_raycast.GetCollider().GetType());
        //         }
        //         _raycast.Position = temp;
        //         _raycast.RemoveException(collidingArea);
        //     }
        //     else if (collidingArea is Door collidingDoor)
        //     {
        //         // Can only exit level when holding dog :)
        //         if (_carriedObject is Dog)
        //         {
        //             collidingDoor.Interact();
        //         }
        //     }
        // }
    }
}
