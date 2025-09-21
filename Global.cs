using Godot;
using System;

public partial class Global : Node
{
    public static Global Instance;
    [Export] public int gridSize = 16; // Size of the grid, in pixels
    
    public override void _Ready()
    {
		  Instance = this;
    }
}
