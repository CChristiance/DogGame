using Godot;
using System;

public partial class Door : Area2D
{
    [Export] private bool _isOpen = false;
    private AnimationPlayer _animationPlayer;
    private float _radius = 0f;
    private float _raidusMin = 0f;
    private float _radiusMax = 1000f;
    private float _radiusDelta = 5f;
    private bool _screenWipe = false;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        // AreaEntered += _OnAreaEntered;
        // AreaExited += _OnAreaExited;
        if (_isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        _animationPlayer.Play("Open");
        _isOpen = true;
    }

    public void Close()
    {
        _animationPlayer.Play("Closed");
        _isOpen = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_screenWipe == true && _radius < _radiusMax)
        {
            _radius += _radiusDelta;
            QueueRedraw();
        }
    }

    public void Interact()
    {
        // Screen wipe starting from door;
        // Move to next level
        if (_isOpen)
        {
            _screenWipe = true;
            // TODO: Transition to either level select or next level
            Timer timer = new();
            timer.WaitTime = 1.5f;
            timer.Autostart = true;
            timer.OneShot = true;
            timer.Timeout += () =>
            {
                GetTree().ChangeSceneToFile("LevelSelect.tscn");
                timer.QueueFree();
            };
            AddChild(timer);
        }
    }

    public override void _Draw()
    {
        if (_radius > 0.0f)
        {
            DrawCircle(Vector2.Zero, _radius, Colors.Black);
        }
    }
}
