using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using TMPro;

// Page de chargement de monde existant
public class LoadWorldUI : MonoBehaviour
{
    public TMP_Dropdown worldDropdown;        // Dropdown pour sélectionner un monde
    public Button loadButton;             // Bouton pour charger la carte
    public Button backButton;             // Bouton pour revenir à la page d'accueil

    private List<string> savedWorlds = new List<string>();

    private void Start()
    {
        backButton.onClick.AddListener(GoBackToMainMenu);
        loadButton.onClick.AddListener(LoadWorld);

        LoadSavedWorlds();
    }

    // Fonction pour revenir à la page d'accueil
    public void GoBackToMainMenu()
    {
        UIManager.Instance.ShowMainMenu();
    }

    void LoadSavedWorlds()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.txt");

        savedWorlds.Clear();
        foreach (var file in files)
        {
            savedWorlds.Add(Path.GetFileNameWithoutExtension(file));
        }

        worldDropdown.ClearOptions();
        worldDropdown.AddOptions(savedWorlds);
    }

    void LoadWorld()
    {
        string selectedWorld = savedWorlds[worldDropdown.value];

        string filePath = Path.Combine(Application.persistentDataPath, $"{selectedWorld}.txt");

        if (File.Exists(filePath))
        {
            string worldData = File.ReadAllText(filePath);
            Debug.Log($"Chargement du monde: {worldData}");
            // Charger les données du monde ici (logique de jeu)
        }
        else
        {
            Debug.LogWarning("Le monde sélectionné est introuvable.");
        }
    }
}
