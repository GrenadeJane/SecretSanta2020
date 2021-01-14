using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilesResourcesLoader
{
    private const string wall = "wall";
    private const string ground = "grass";
    private const string groundDigged = "grass!fade";
    private const string digged = "digged";

    public static Tile GetWallTile()
    {
        return GetTileByName(wall);
    }

    public static Tile GetGroundUsedTile()
    {
        return GetTileByName(groundDigged);
    }

    public static Tile GetGroundTile()
    {
        return GetTileByName(ground);
    }

    public static Tile GetUsedTile()
    {
        return GetTileByName(digged);
    }

    private static Tile GetTileByName(string name)
    {
        return (Tile)Resources.Load(name, typeof(Tile));
    }
}