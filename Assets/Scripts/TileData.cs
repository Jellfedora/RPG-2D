using UnityEngine;
using WorldGeneration;

// Stocke les infos dynamiques d’une tuile spécifique pendant le jeu.
public class TileData
{
    public Vector2Int position;        // Position locale dans le chunk
    public TileType type;              // Type de la tuile

    public string prefabName;          // Nom du prefab (dans Resources)
    public string gameIdentifier;      // Identifiant unique en jeu (ex: meadow_tree_1)

    public bool isDestroyed = false;   // État de destruction
    public int currentHealth = 100;    // État de santé (modifiable selon prefabInfo si nécessaire)

    public TileData(Vector2Int pos, TileType tileType)
    {
        position = pos;
        type = tileType;
    }
}
