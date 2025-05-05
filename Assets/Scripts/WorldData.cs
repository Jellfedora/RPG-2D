using System.Collections.Generic;
using UnityEngine;

// Stockage des données du monde
public class WorldData
{
    public string worldName;
    public string worldSeed;
    public int worldSize;
    public List<ChunkData> chunks = new List<ChunkData>(); // Liste des chunks du monde

    [System.Serializable]
    public class ChunkData
    {
        public Vector2Int chunkPosition;
        public List<TileData> tiles = new List<TileData>();
    }
}
