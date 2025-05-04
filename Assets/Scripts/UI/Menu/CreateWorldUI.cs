using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Page de création de monde
public class CreateWorldUI : MonoBehaviour
{
    public TMP_InputField worldNameInput;   // Champ pour le nom de la carte
    public TMP_InputField worldSeedInput;   // Champ pour la seed de la carte
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

        // Lancer la génération via GridManager
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        gridManager.GenerateWorld(worldName, worldSeed);

        // TODO Sauvegarder le monde

        Debug.Log($"[CreateWorldUI] Génération du monde {worldName} avec la seed {worldSeed}");

        // Afficher l'UI de jeu
        UIManager.Instance.ShowGameUI();
    }

    void SaveWorld(string worldName, int size)
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, $"{worldName}.txt");
        string worldData = $"Nom: {worldName}, Taille: {size}";

        System.IO.File.WriteAllText(filePath, worldData);
        Debug.Log($"Carte sauvegardée sous {filePath}");
    }

    
}
