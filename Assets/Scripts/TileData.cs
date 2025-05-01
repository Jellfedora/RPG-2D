using UnityEngine;
using WorldGeneration;

public class TileData
{
    public Vector2Int position;   // Position locale dans le chunk
    public TileType type;         // Type de la tuile

    // Constructeur principal
    public TileData(Vector2Int pos, TileType tileType)
    {
        position = pos;
        type = tileType;
    }
}