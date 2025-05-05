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
    private System.Random random; // Générateur aléatoire pour ce chunk

    // Constructeur du chunk
    public Chunk(Vector2Int pos, int chunkSize, Transform parent, System.Random seedRandom)
    {
        chunkPosition = pos;        // Position du chunk dans la grille
        size = chunkSize;           // Taille d'un chunk (en tuiles)
        random = seedRandom;        // Générateur aléatoire local pour ce chunk

        // Création du GameObject représentant le chunk
        GameObject chunkGO = new($"Chunk_{pos.x}_{pos.y}");
        chunkGO.transform.parent = parent;
        chunkTransform = chunkGO.transform;

        // ➤ Positionner correctement le chunk dans l'espace monde
        chunkTransform.position = new Vector3(chunkPosition.x * size, chunkPosition.y * size, 0f);

        AssignBiome();                    // Assigner le biome
        GenerateTiles();                  // Générer les tuiles du chunk
        biome.GenerateTileVisuals(this); // Générer les visuels des tuiles

        // ➤ Si on est au centre du monde, on instancie le point de spawn
        if (chunkPosition == Vector2Int.zero)
        {
            GameObject spawnPrefab = Resources.Load<GameObject>("Prefabs/Spawn");
            GameObject spawn = Object.Instantiate(spawnPrefab, chunkTransform);

            // ➤ Positionner le spawn au centre du chunk en utilisant la projection isométrique
            Vector2Int centerLocalPos = new Vector2Int(size / 2, size / 2);
            Vector2Int centerWorldPos = new Vector2Int(
                chunkPosition.x * size + centerLocalPos.x,
                chunkPosition.y * size + centerLocalPos.y
            );
            
            // Utiliser la même fonction IsoPosition que dans MeadowBiome
            Vector3 spawnPos = IsoPosition(centerWorldPos.x, centerWorldPos.y);
            
            // Ajouter le même décalage vertical que pour les autres objets
            spawnPos.y += 0.5f;
            
            spawn.transform.localPosition = spawnPos;
        }

        else
        {
            // Générer les objets du biome si ce n’est pas le chunk de spawn
            GenerateObjects();
        }
    }

    // ➤ Attribution du biome en fonction de la position
    public void AssignBiome()
    {
        bool isNearCenter = Mathf.Abs(chunkPosition.x) <= 1 && Mathf.Abs(chunkPosition.y) <= 1;

        if (isNearCenter)
        {
            biomeType = BiomeType.Meadow;
            biome = new MeadowBiome(random);
        }
        else
        {
            // Pour l'instant tous les biomes sont des prairies
            biomeType = BiomeType.Meadow;
            biome = new MeadowBiome(random);
        }
    }

    // ➤ Convertir une position monde en position locale dans le chunk
    public Vector2Int WorldToLocalPosition(Vector2Int worldPos)
    {
        return new Vector2Int(
            worldPos.x - chunkPosition.x * size,
            worldPos.y - chunkPosition.y * size
        );
    }

    // ➤ Génération des tuiles du chunk selon le biome
    private void GenerateTiles()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2Int localPos = new(x, y);
                Vector2Int worldPos = new(
                    chunkPosition.x * size + x,
                    chunkPosition.y * size + y
                );

                TileType tileType = biome.GetTileTypeAt(worldPos);
                tiles[localPos] = new TileData(localPos, tileType);
            }
        }
    }

    // ➤ Génération des objets du biome dans ce chunk
    private void GenerateObjects()
    {
        List<Vector2Int> occupiedPositions = new();

        int startX = chunkPosition.x * size;
        int startY = chunkPosition.y * size;
        int endX = startX + size;
        int endY = startY + size;

        biome.GenerateObjects(this, startX, startY, endX, endY, occupiedPositions);
    }

    private Vector3 IsoPosition(int x, int y)
    {
        float isoX = (x - y) * 0.25f;
        float isoY = (x + y) * 0.125f;
        return new Vector3(isoX, isoY, 0f);
    }
}

