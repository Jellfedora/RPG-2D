using UnityEngine;
using UnityEngine.UI;

// Page d'accueil du menu principal
// Permet de naviguer entre la création de monde et le chargement de monde
public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuPage; // Page principale du menu
    public GameObject createWorldPage; // Page de création de monde
    public GameObject loadWorldPage; // Page de chargement de monde
    public GameObject gamePage; // Page de jeu

    public Button showCreateWorldPageButton;
    public Button showLoadWorldPageButton;

    private void Start()
    {
        showCreateWorldPageButton.onClick.AddListener(() => ShowPage(createWorldPage));
        showLoadWorldPageButton.onClick.AddListener(() => ShowPage(loadWorldPage));

        UIManager.Instance.ShowMainMenu();
    }

    // Fonction pour afficher une page et masquer les autres
    public void ShowPage(GameObject page)
    {
        mainMenuPage.SetActive(page == mainMenuPage);
        createWorldPage.SetActive(page == createWorldPage);
        loadWorldPage.SetActive(page == loadWorldPage);
        gamePage.SetActive(page == gamePage);
    }
}
