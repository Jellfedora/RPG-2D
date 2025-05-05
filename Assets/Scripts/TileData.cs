using UnityEngine;
using WorldGeneration;

// Stocke les infos dynamiques d’une tuile spécifique pendant le jeu.
[System.Serializable]
public class TileData
{
    public Vector2Int position;        // Position locale dans le chunk
    public TileType type;              // Type de la tuile

    public string prefabName;          // Nom du prefab (dans Resources)
    public string gameIdentifier;      // Identifiant unique en jeu (ex: meadow_tree_1)

    public bool isDestroyed = false;   // État de destruction
    public int currentHealth = 100;    // État de santé (modifiable selon prefabInfo si nécessaire)

    // Constructeur principal
    public TileData(Vector2Int pos, TileType tileType)
    {
        position = pos;
        type = tileType;
    }

    // Constructeur pour la sérialisation
    public TileData(int x, int y, string prefab, string identifier, bool destroyed, int health)
    {
        position = new Vector2Int(x, y);
        prefabName = prefab;
        gameIdentifier = identifier;
        isDestroyed = destroyed;
        currentHealth = health;
    }

    // Cette méthode convertit TileData en un format sérialisé (s'il faut manipuler des objets complexes, par exemple TileType)
    public static TileData FromSerializable(int x, int y, string prefab, string identifier, bool destroyed, int health)
    {
        return new TileData(x, y, prefab, identifier, destroyed, health);
    }
}
