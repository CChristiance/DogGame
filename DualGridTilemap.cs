using Godot;
using System;
using System.Collections.Generic;
using static TileType;

// Source: jess::codes

public partial class DualGridTilemap : Node2D
{
    TileMapLayer worldMapLayer;
    TileMapLayer displayMapLayer;
    public Vector2I grassPlaceholderAtlasCoord = new(0, 0);
    public Vector2I emptyPlaceholderAtlasCoord = new(0, 1);
    readonly Vector2I[] NEIGHBORS = new Vector2I[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1) };

    readonly Dictionary<Tuple<TileType, TileType, TileType, TileType>, Vector2I> neighborsToAtlasCoord = new() {
        { new(Grass, Grass, Grass, Grass), new Vector2I(2, 1) }, // All corners
        { new(None, None, None, Grass), new Vector2I(1, 3) }, // Outer bottom-right corner
        { new(None, None, Grass, None), new Vector2I(0, 0) }, // Outer bottom-left corner
        { new(None, Grass, None, None), new Vector2I(0, 2) }, // Outer top-right corner
        { new(Grass, None, None, None), new Vector2I(3, 3) }, // Outer top-left corner
        { new(None, Grass, None, Grass), new Vector2I(1, 0) }, // Right edge
        { new(Grass, None, Grass, None), new Vector2I(3, 2) }, // Left edge
        { new(None, None, Grass, Grass), new Vector2I(3, 0) }, // Bottom edge
        { new(Grass, Grass, None, None), new Vector2I(1, 2) }, // Top edge
        { new(None, Grass, Grass, Grass), new Vector2I(1, 1) }, // Inner bottom-right corner
        { new(Grass, None, Grass, Grass), new Vector2I(2, 0) }, // Inner bottom-left corner
        { new(Grass, Grass, None, Grass), new Vector2I(2, 2) }, // Inner top-right corner
        { new(Grass, Grass, Grass, None), new Vector2I(3, 1) }, // Inner top-left corner
        { new(None, Grass, Grass, None), new Vector2I(2, 3) }, // Bottom-left top-right corners
        { new(Grass, None, None, Grass), new Vector2I(0, 1) }, // Top-left down-right corners
        { new(None, None, None, None), new Vector2I(0, 3) }, // No corners
    };

    
    public override void _Ready() {
        worldMapLayer = GetNode<TileMapLayer>("worldMapLayer");
        displayMapLayer = GetNode<TileMapLayer>("displayMapLayer");
        // Refresh all display tiles
        foreach (Vector2I coord in worldMapLayer.GetUsedCells())
        {
            setDisplayTile(coord);
        }
    }

    /// <summary>
    /// <para>Returns the map coordinates of the cell containing the given <paramref name="localPosition"/>. If <paramref name="localPosition"/> is in global coordinates, consider using <see cref="Godot.Node2D.ToLocal(Vector2)"/> before passing it to this method. See also <see cref="Godot.TileMapLayer.MapToLocal(Vector2I)"/>.</para>
    /// </summary>
    public Vector2I LocalToMap(Vector2 pos)
    {
        return worldMapLayer.LocalToMap(pos);
    }

    public void SetTile(Vector2I coords, Vector2I atlasCoords) {
        worldMapLayer.SetCell(coords, 0, atlasCoords);
        setDisplayTile(coords);
    }

    void setDisplayTile(Vector2I pos) {
        // loop through 4 display NEIGHBORS
        for (int i = 0; i < NEIGHBORS.Length; i++) {
            Vector2I newPos = pos + NEIGHBORS[i];
            displayMapLayer.SetCell(newPos, 1, calculateDisplayTile(newPos));
        }
    }

    Vector2I calculateDisplayTile(Vector2I coords) {
        // get 4 world tile NEIGHBORS
        TileType botRight = getWorldTile(coords - NEIGHBORS[0]);
        TileType botLeft = getWorldTile(coords - NEIGHBORS[1]);
        TileType topRight = getWorldTile(coords - NEIGHBORS[2]);
        TileType topLeft = getWorldTile(coords - NEIGHBORS[3]);

        // return tile (atlas coord) that fits the neighbour rules
        return neighborsToAtlasCoord[new(topLeft, topRight, botLeft, botRight)];
    }

    TileType getWorldTile(Vector2I coords) {
        Vector2I atlasCoord = worldMapLayer.GetCellAtlasCoords(coords);
        if (atlasCoord == grassPlaceholderAtlasCoord)
            return Grass;
        else
            return None;
    }
}

public enum TileType
{
    None,
    Grass
}