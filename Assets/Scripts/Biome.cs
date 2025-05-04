using System.Collections.Generic;
using UnityEngine;
using WorldGeneration;

public abstract class Biome
{
    public string Name { get; protected set; }

    // Liste des objets (arbres, rochers, etc.)
    protected List<GameObject> objectPrefabs;

    protected int minObjectsPerChunk;
    protected int maxObjectsPerChunk;

    // Seed pour les classes dérivées
    protected System.Random random;

    public Biome(System.Random seedRandom) {
        random = seedRandom;
    }

    public abstract TileType GetTileTypeAt(Vector2Int worldPos);

    public abstract void GenerateObjects(Chunk chunk, int startX, int startY, int endX, int endY, List<Vector2Int> occupiedPositions);

    public abstract void GenerateTileVisuals(Chunk chunk);
}
