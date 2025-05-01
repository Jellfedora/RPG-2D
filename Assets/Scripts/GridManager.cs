using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Taille du monde")]
    [SerializeField] private int worldSize = 50;
    [SerializeField] private int chunkSize = 25;

    [Header("Références")]
    [SerializeField] private Transform worldParent;
    private readonly Dictionary<Vector2Int, Chunk> chunks = new();
    private System.Random random = new();

    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        int chunkCount = Mathf.CeilToInt((float)worldSize / chunkSize);

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

    // ➤ Génération de la grille de chunks
    private void GenerateChunk(Vector2Int chunkPos)
    {
        Chunk chunk = new(chunkPos, chunkSize, worldParent);
        chunk.AssignBiome();
        chunks[chunkPos] = chunk;

        Debug.Log($"Chunk {chunkPos} : {chunk.biomeType}");
    }

    // ➤ Méthode pour obtenir un chunk à une position donnée
    public Chunk GetChunkAtPosition(Vector2Int worldPos)
    {
        Vector2Int chunkPos = new(
            Mathf.FloorToInt(worldPos.x / (float)chunkSize),
            Mathf.FloorToInt(worldPos.y / (float)chunkSize)
        );

        return chunks.TryGetValue(chunkPos, out var chunk) ? chunk : null;
    }
}
