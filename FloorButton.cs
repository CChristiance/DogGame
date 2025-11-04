using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FloorButton : GroundButton
{
    DualGridTilemap dualGridTilemap;
    [Export] string coordsString;       // Format: (x1,y1) (x2,y2)...
    List<Vector2I> vector2Is = [];

    public override void _Ready()
    {
        base._Ready();

        dualGridTilemap = GetOwner().GetChildren().OfType<DualGridTilemap>().FirstOrDefault();

        var split = coordsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Manually parse string into Vector2Is
        foreach (var coords in split)
        {
            var trimmed = coords.Trim('(', ')');    // Remove parentheses
            var parts = trimmed.Split(',');         // Separate x and y values

            if (parts.Length == 2 &&
            int.TryParse(parts[0].Trim(), out int x) &&
            int.TryParse(parts[1].Trim(), out int y))
            {
                vector2Is.Add(new Vector2I(x, y));
                vector2Is.Add(new Vector2I(x, y - 13)); // Also add alt coords
            }
            else
            {
                GD.PushError($"Invalid vector format: {coords}");
            }
        }
    }

    protected override void _OnAreaEntered(Area2D area)
    {
        base._OnAreaEntered(area);
        foreach (Vector2I coords in vector2Is)
        {
            dualGridTilemap.SetTile(coords, dualGridTilemap.grassPlaceholderAtlasCoord);
        }
    }

    protected override void _OnAreaExited(Area2D area)
    {
        base._OnAreaExited(area);
        if (!HasOverlappingAreas())
        {
            foreach (Vector2I coords in vector2Is)
            {
                dualGridTilemap.SetTile(coords, dualGridTilemap.emptyPlaceholderAtlasCoord);
            }
        }
    }
}
