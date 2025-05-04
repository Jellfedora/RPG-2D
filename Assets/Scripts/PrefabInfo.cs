using UnityEngine;

// Script MonoBehaviour attaché à un prefab dans l’éditeur.
// Contient des métadonnées pour l'identification et les interactions.
public class PrefabInfo : MonoBehaviour
{
    [Tooltip("Identifiant unique utilisé dans le jeu (ex: meadow_tree_1)")]
    public string gameIdentifier;

    [Tooltip("Le nom du prefab dans Resources (rempli automatiquement si vide)")]
    public string prefabName;

    [Tooltip("Définit si l'objet peut être détruit par le joueur ou l'environnement")]
    public bool isDestructible = false;

    [Tooltip("Points de vie de base de l'objet, si destructible")]
    public int baseHealth = 100;

    private void Awake()
    {
        // Optionnel : auto-remplir prefabName si vide
        if (string.IsNullOrEmpty(prefabName))
        {
            prefabName = gameObject.name;
        }
    }
}
