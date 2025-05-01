using System.Collections.Generic;
using UnityEngine;
using WorldGeneration;

public class Chunk
{
    public Vector2Int chunkPosition { get; private set; }
    public int size { get; private set; }
    public Transform chunkTransform { get; private set; }

    public Dictionary<Vector2Int, TileData> tiles = new();
    public BiomeType biomeType { get; private set; }
    public Biome biome { get; private set; }

    public Chunk(Vector2Int pos, int chunkSize, Transform parent)
    {
        chunkPosition = pos;
        size = chunkSize;

        GameObject chunkGO = new($"Chunk_{pos.x}_{pos.y}");
        chunkGO.transform.parent = parent;
        chunkTransform = chunkGO.transform;

        AssignBiome();       // On assigne le biome au chunk
        GenerateTiles();     // On génère les tuiles
        biome.GenerateTileVisuals(this); // On génère les visuels des tuiles
        GenerateObjects();   // On génère les objets
    }

    // ➤ Méthode pour assigner un biome au chunk
    public void AssignBiome()
    {
        float noiseValue = Mathf.PerlinNoise(chunkPosition.x * 0.1f, chunkPosition.y * 0.1f);

        // ➤ Étend ici pour intégrer d'autres types de biomes
        biomeType = BiomeType.Meadow;
        biome = new MeadowBiome();
    }

    // ➤ Méthode pour obtenir le type de tuile à une position donnée
    public Vector2Int WorldToLocalPosition(Vector2Int worldPos)
    {
        return new Vector2Int(
            worldPos.x - chunkPosition.x * size,
            worldPos.y - chunkPosition.y * size
        );
    }

    // ➤ Génère les tuiles du chunk selon le biome
    private void GenerateTiles()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2Int localPos = new Vector2Int(x, y);

                // Calcul de la position globale dans le monde
                Vector2Int worldPos = new Vector2Int(
                    chunkPosition.x * size + x,
                    chunkPosition.y * size + y
                );

                // Obtenir le type de tuile selon le biome
                TileType tileType = biome.GetTileTypeAt(worldPos);

                // Création de la tuile avec ses données
                tiles[localPos] = new TileData(localPos, tileType);
            }
        }
    }

    // ➤ Génère les objets du biome dans ce chunk
    private void GenerateObjects()
    {
        List<Vector2Int> occupiedPositions = new();

        int startX = chunkPosition.x * size;
        int startY = chunkPosition.y * size;
        int endX = startX + size;
        int endY = startY + size;

        biome.GenerateObjects(this, startX, startY, endX, endY, occupiedPositions);
    }
}
