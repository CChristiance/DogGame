using Godot;
using System;
using System.Linq;

public partial class Level : Node2D
{
    [Export] public Time time { get; set; } = Time.Present;
    public enum Time
    {
        Past,
        Present
    }

    [Export] Vector2 pastPosition;
    [Export] Vector2 presentPosition;

    private Camera2D _camera;
    private Node2D _objects;
    private Dog _dog;

    public override void _Ready()
    {    
        _camera = GetNode<Camera2D>("Camera2D");
        _objects = GetNode<Node2D>("Objects");
        _dog = GetNodeOrNull<Dog>("Objects/Dog");

        if (time == Time.Present)
        {
            _camera.Position = presentPosition;
            _objects.Position = presentPosition;
            _dog.turnOld();
        }
        else if (time == Time.Past)
        {
            _camera.Position = pastPosition;
            _objects.Position = pastPosition;
            _dog.turnYoung();
        }

        GD.Print(time);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("change_time"))
        {
            if (!_CheckValidSwitch())
            {
                return;
            }

            if (time == Time.Past)
            {
                time = Time.Present;
                _Causality(c => c.ToPresent());
                _camera.Position = presentPosition;
                _objects.Position = presentPosition;
                _dog.turnOld();
            }
            else if (time == Time.Present)
            {
                time = Time.Past;
                _Causality(c => c.ToPast());
                _camera.Position = pastPosition;
                _objects.Position = pastPosition;
                _dog.turnYoung();
            }
        }
        else if (@event.IsActionPressed("escape"))
        {
            GetTree().ChangeSceneToFile("LevelSelect.tscn");
        }
        else if (@event.IsActionPressed("reset"))
        {
            GetTree().ReloadCurrentScene();
        }
    }

    private void _Causality(Action<Causality> step)
    {
        foreach (Node obj in _objects.GetChildren())
        {
            var causality = obj.GetNodeOrNull<Causality>("Causality");
            if (causality == null)
            {
                continue;
            }

            step(causality);
        }
    }

    // DEBUG DEBUG DEBUG
    // private Vector2 debugCirclePosition = Vector2.Zero;
    // private const float CircleRadius = 5.0f;
    // private readonly Color CircleColor = Colors.Red;

    // public void UpdateDebugPosition(Vector2 newPosition)
    // {
    //     ZIndex = 1000;
    //     debugCirclePosition = newPosition;
    //     QueueRedraw();
    // }

    // public override void _Draw()
    // {
    //     DrawCircle(debugCirclePosition, CircleRadius, CircleColor);
    // }
    // END OF DEBUG DEBUG DEBUG

    // Ensures nothing is floating when changing time
    private bool _CheckValidSwitch()
    {
        PhysicsDirectSpaceState2D spaceState = GetWorld2D().DirectSpaceState;
        bool returnValue = true;

        foreach (Area2D child in _objects.GetChildren().OfType<Area2D>())
        {
            // 208 = 13 (tile offset) * 16 (pixels per tile)
            Vector2 offset = new(0, time == Time.Present ? -208 : 208);

            CollisionShape2D shapeNode = child.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            Shape2D shape = shapeNode.Shape;

            // if (child is Human)
            // {
            //     UpdateDebugPosition(child.GlobalPosition + offset);
            // }

            PhysicsShapeQueryParameters2D query = new()
            {
                Shape = shape,
                Transform = new Transform2D(0, child.GlobalPosition + offset),
                CollisionMask = child.CollisionMask
            };

            var space = GetWorld2D().DirectSpaceState;
            var result = space.IntersectShape(query);

            if (result.Count > 0)
            {
                // GD.Print($"{child.Name} -> Blocked");

                // Flash child red
                Sprite2D sprite = child.GetChildren().OfType<Sprite2D>().FirstOrDefault();
                
                // Prevents spamming the button - fixes bug when object stays red
                if (sprite == null || child.GetChildren().OfType<Timer>().FirstOrDefault() != null)
                {
                    returnValue = false;
                    continue;
                }

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
                child.AddChild(timer);

                returnValue = false;
            }
            else
            {
                // GD.Print($"{child.Name} -> Pass");
            }
        }
        return returnValue;
    }
}
