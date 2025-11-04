using Godot;
using System;

public partial class WorldMapLayer : TileMapLayer
{
    public override void _Ready()
    {
        if (!Engine.IsEditorHint())
        {
            Visible = false;
        }
    }
}
