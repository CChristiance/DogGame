using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

public abstract partial class Player : Area2D
{
    public int gridSize;

    protected Vector2 _desiredPosition;
    protected Vector2 _lastPosition;
    private float _t = 0.0f;
    protected Direction direction;
    protected Push push;

    [Export] protected StringName moveLeft, moveRight, moveUp, moveDown;

    protected bool _canHorizontal = true;
    protected bool _canVertical = true;

    protected AnimationPlayer _animationPlayer;
    protected RayCast2D _raycast;

    [Export] private float _speed = 100.0f;
    private Vector2 _velocity;

    protected enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public override void _Ready()
    {
        _lastPosition = _desiredPosition;
        _desiredPosition = Position;
        gridSize = Global.Instance.gridSize;
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _raycast = GetNode<RayCast2D>("RayCast2D");
        push = GetNode<Push>("Push");
    }

    public override void _PhysicsProcess(double delta)
    {
        float horizontalInput = Input.GetActionStrength(moveRight) - Input.GetActionStrength(moveLeft); // -1 if left, 1 if right, 0 if neither
        float verticalInput = Input.GetActionStrength(moveDown) - Input.GetActionStrength(moveUp); // -1 if up, 1 if down, 0 if neither

        // No diagonals in grid-based movement - one direction must be prioritized over the other
        if (_canHorizontal && !(horizontalInput.CompareTo(0) == 0) && _checkFloorButton()) // If horizontal input is detected
        {
            if (horizontalInput < 0) // Facing left
            {
                _raycast.TargetPosition = new Vector2(16, 0);
                RotationDegrees = 180;
                Scale = new Vector2(1, -1);
                _animationPlayer.Play("walk_side");
                direction = Direction.Left;
            }
            else // Facing right
            {
                _raycast.TargetPosition = new Vector2(16, 0);
                RotationDegrees = 0;
                Scale = new Vector2(1, 1);
                _animationPlayer.Play("walk_side");
                direction = Direction.Right;
            }

            // Collision check
            if (!_raycast.IsColliding())
            {
                _canVertical = false; // Disable vertical input to prevent going off grid
                _t = 0.5f;
                if (Position.X.CompareTo(_desiredPosition.X) == horizontalInput) // If we passed the last grid
                {
                    _lastPosition = _desiredPosition;
                    _desiredPosition = new Vector2(_desiredPosition.X + gridSize * horizontalInput, Position.Y);
                }
                Position = new Vector2(Position.X + _speed * horizontalInput * (float)delta, Position.Y);
            }
            else
            {
                Position = Position.Lerp(_desiredPosition, _t);
            }

        }
        else if (_canVertical && !(verticalInput.CompareTo(0) == 0) && _checkFloorButton()) // If vertical input is detected
        {
            if (verticalInput < 0) // Facing Up
            {
                _raycast.TargetPosition = new Vector2(0, -16);
                _animationPlayer.Play("walk_up");
                direction = Direction.Up;
            }
            else // Facing Down
            {
                _raycast.TargetPosition = new Vector2(0, 16);
                _animationPlayer.Play("walk_down");
                direction = Direction.Down;
            }

            if (!_raycast.IsColliding())
            {
                _canHorizontal = false; // Disable horizontal input to prevent going off grid
                _t = 0.5f;
                if (Position.Y.CompareTo(_desiredPosition.Y) == verticalInput)
                {
                    _lastPosition = _desiredPosition;
                    _desiredPosition = new Vector2(Position.X, _desiredPosition.Y + gridSize * verticalInput);
                }
                Position = new Vector2(Position.X, Position.Y + _speed * verticalInput * (float)delta);
            }
            else
            {
                Position = Position.Lerp(_desiredPosition, _t);
            }
        }
        else if (!Position.IsEqualApprox(_desiredPosition))
        {
            // snap to grid
            // TODO: either fine tune variables or redo with velocity -- feels bad currently
            _t += (float)delta * 0.999f;
            Position = Position.Lerp(_desiredPosition, _t);
        }
        else
        {
            _canHorizontal = true;
            _canVertical = true;

            switch (direction)
            {
                case Direction.Left:
                    _animationPlayer.Play("idle_side");
                    break;
                case Direction.Right:
                    _animationPlayer.Play("idle_side");
                    break;
                case Direction.Up:
                    _animationPlayer.Play("idle_up");
                    break;
                case Direction.Down:
                    _animationPlayer.Play("idle_down");
                    break;
                default:
                    break;
            }
        }

        // If human and dog bonk
        if (HasOverlappingAreas())
        {
            bool overlappingDog = GetOverlappingAreas().OfType<Dog>().Any();
            bool overlappingBox = GetOverlappingAreas().OfType<FloorBox>().Any();
            if (overlappingDog || overlappingBox)
            {
                Position = Position.Lerp(_lastPosition, _t);
                _desiredPosition = _lastPosition;
            }
        }
    }

    private bool _checkFloorButton()
    {
        // Button check - fixes bug when one player steps off button while other is on ephemeral ground
        if (HasOverlappingAreas())
        {
            bool isOverlappingButton = GetOverlappingAreas().OfType<FloorButton>().Any();
            if (isOverlappingButton)
            {
                FloorButton overlappingButton = GetOverlappingAreas().OfType<FloorButton>().FirstOrDefault();
                // Check if other player is on ephemeral ground
                Node2D otherPlayer = null;
                if (this is Human)
                {
                    otherPlayer = GetParent().GetChildren().OfType<Dog>().FirstOrDefault();
                }
                else if (this is Dog)
                {
                    otherPlayer = GetParent().GetChildren().OfType<Human>().FirstOrDefault();
                }

                // Check FloorButton Vector2I coords list and compare it to otherPlayer position
                List<Vector2I> coords = overlappingButton.vector2Is;

                // Flash child red
                Sprite2D sprite = GetChildren().OfType<Sprite2D>().FirstOrDefault();
                
                if (sprite != null && GetChildren().OfType<Timer>().FirstOrDefault() == null)
                {
                    Color originalColor = sprite.Modulate;
                    sprite.Modulate = Colors.Red;
                    Timer timer = new();
                    timer.WaitTime = 0.2f;
                    timer.OneShot = true;
                    timer.Autostart = true;
                    timer.Timeout += () =>
                    {
                        sprite.Modulate = originalColor;
                        timer.QueueFree();
                    };
                    AddChild(timer);
                }
            }
        }
        return false;
    }
}
