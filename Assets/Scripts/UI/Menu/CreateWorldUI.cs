using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Page de création de monde
public class CreateWorldUI : MonoBehaviour
{
    public TMP_InputField worldNameInput;   // Champ pour le nom de la carte
    public TMP_InputField worldSeedInput;   // Champ pour la seed de la carte
    public Slider worldSizeSlider; // Dropdown pour la taille de la carte
    public Button generateButton;       // Bouton pour générer la carte
    public Button backButton;           // Bouton pour revenir à la page d'accueil

    private void Start()
    {
        generateButton.onClick.AddListener(CreateWorld);
        backButton.onClick.AddListener(GoBackToMainMenu);
    }

    // Fonction pour revenir à la page d'accueil
    public void GoBackToMainMenu()
    {
        UIManager.Instance.ShowMainMenu();
    }

    void CreateWorld()
    {
        // Vérifier si le nom de la carte est valide
        string worldName = worldNameInput.text;

        if (string.IsNullOrEmpty(worldName))
        {
            Debug.LogWarning("Le nom de la carte est requis.");
            return;
        }

        // Vérifier si la seed est valide
        string worldSeed = worldSeedInput.text;
        if (string.IsNullOrEmpty(worldSeed))
        {
            Debug.LogWarning("La seed est requise.");
            return;
        }

        // Vérifier si la taille de la carte est valide
        int worldSize = (int)worldSizeSlider.value;
        if (worldSize <= 0)
        {
            Debug.LogWarning("La taille de la carte doit être supérieure à 0.");
            return;
        }

        // Lancer la génération via GridManager
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        gridManager.GenerateWorld(worldName, worldSize, worldSeed);

        // Sauvegarder le monde
        SaveWorld(worldName, worldSize);

        Debug.Log($"[CreateWorldUI] Génération du monde {worldName} de taille {worldSize} avec la seed {worldSeed}");

        // Afficher l'UI de jeu
        UIManager.Instance.ShowGameUI();
    }

    void SaveWorld(string worldName, int size) {
        WorldData worldData = new WorldData
        {
            worldName = worldName,
            worldSeed = worldSeedInput.text, // Utilisation de la seed entrée par l'utilisateur
            worldSize = size
        };

        // Ajouter les chunks et leurs données
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        int chunkCount = Mathf.CeilToInt((float)size / gridManager.chunkSize);
        for (int cx = 0; cx < chunkCount; cx++)
        {
            for (int cy = 0; cy < chunkCount; cy++)
            {
                Vector2Int chunkPos = new Vector2Int(cx, cy);
                Chunk chunk = gridManager.GetChunkAtPosition(chunkPos);
                if (chunk != null)
                {
                    WorldData.ChunkData chunkData = new WorldData.ChunkData
                    {
                        chunkPosition = chunk.chunkPosition
                    };

                    // Ajouter les données des tuiles
                    foreach (var tile in chunk.tiles)
                    {
                        chunkData.tiles.Add(tile.Value); // Ajout de chaque TileData
                    }

                    worldData.chunks.Add(chunkData);
                }
            }
        }

        // Sérialiser en JSON et sauvegarder
        string json = JsonUtility.ToJson(worldData, true);
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, $"{worldName}.json");
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log($"Carte sauvegardée sous {filePath}");
    }
}
