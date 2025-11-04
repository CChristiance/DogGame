using Godot;
using System;

public partial class DoorButtonHandler : Node2D
{
    private Door _door;

    public override void _Ready()
    {
        _door = GetNode<Door>("../Door");
        UpdateState();
    }

    public void UpdateState()
    {
        bool _allPressed = true;
        foreach (DoorButton dButton in GetChildren())
        {
            if (!dButton.pressed)
            {
                _allPressed = false;
                break;
            }
        }

        if (_allPressed)
        {
            _door.Open();
        }
        else
        {
            _door.Close();
        }
    }
}
