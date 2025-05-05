using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Taille du monde")]
    [SerializeField] public int chunkSize = 25;

    [Header("Références")]
    [SerializeField] private Transform worldParent;
    private readonly Dictionary<Vector2Int, Chunk> chunks = new();
    private System.Random seedRandom;

    // Générer le monde avec un nom et une seed
    public void GenerateWorld(string worldName, int worldSize, string worldSeed)
    {
        // Initialiser le générateur aléatoire avec la seed
        seedRandom = new System.Random(worldSeed.GetHashCode()); // Utilisation du hashCode de la seed pour générer un int

        Debug.Log($"[GridManager] Génération du monde {worldName} de taille {worldSize} avec la seed {worldSeed}");

        int chunkCount = Mathf.CeilToInt((float)worldSize / chunkSize);

        // Génération des chunks
        for (int cx = 0; cx < chunkCount; cx++)
        {
            for (int cy = 0; cy < chunkCount; cy++)
            {
                Vector2Int chunkPos = new(cx, cy);
                if (!chunks.ContainsKey(chunkPos))
                    GenerateChunk(chunkPos);
            }
        }
    }

    // Génération d'un chunk en fonction de la position et de la seed
    private void GenerateChunk(Vector2Int chunkPos)
    {
        // Passer le seedRandom à chaque chunk
        Chunk chunk = new(chunkPos, chunkSize, worldParent, seedRandom);
        chunk.AssignBiome();
        chunks[chunkPos] = chunk;
    }

    // Méthode pour obtenir un chunk à une position donnée
    public Chunk GetChunkAtPosition(Vector2Int worldPos)
    {
        Vector2Int chunkPos = new(
            Mathf.FloorToInt(worldPos.x / (float)chunkSize),
            Mathf.FloorToInt(worldPos.y / (float)chunkSize)
        );

        return chunks.TryGetValue(chunkPos, out var chunk) ? chunk : null;
    }
}
