using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public MainMenuUI mainMenuUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Afficher la page principale du menu
    public void ShowMainMenu()
    {
        mainMenuUI.ShowPage(mainMenuUI.mainMenuPage);
    }

    // Afficher la page de création de monde
    public void ShowCreateWorld()
    {
        mainMenuUI.ShowPage(mainMenuUI.createWorldPage);
    }

    // Afficher la page de chargement de monde
    public void ShowLoadWorld()
    {
        mainMenuUI.ShowPage(mainMenuUI.loadWorldPage);
    }

    // Afficher l'UI de jeu
    public void ShowGameUI()
    {
        mainMenuUI.ShowPage(mainMenuUI.gamePage);
    }
}