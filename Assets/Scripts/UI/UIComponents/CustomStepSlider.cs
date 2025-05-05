using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class CustomStepSlider : MonoBehaviour {
    public int minValue = 0;
    public int maxValue = 1000;
    public int defaultValue = 500;
    public int defaultStep = 100;
    public TextMeshProUGUI currentValueText;

    public Slider slider;

    private void Awake() {
        slider.wholeNumbers = true; // Permet d'avoir des entiers uniquement
        slider.minValue = minValue; // Assignation de la valeur minimale
        slider.maxValue = maxValue; // Assignation de la valeur maximale
        slider.value = defaultValue; // Définir la valeur par défaut
    }

    private void Start() {
        // Initialiser la valeur avec un pas arrondi
        float initialValue = RoundToStep(slider.value);
        slider.value = initialValue;
        UpdateText(initialValue); // Mettre à jour le texte affiché

        // Ajouter un écouteur d'événements pour détecter les changements de valeur
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Fonction qui arrondit la valeur au pas
    private float RoundToStep(float value) {
        return Mathf.Round(value / defaultStep) * defaultStep;
    }

    // Méthode pour mettre à jour le texte affiché sur le slider
    private void UpdateText(float value) {
        if (currentValueText != null)
            currentValueText.text = value.ToString("F0");
    }

    // Méthode appelée à chaque changement de valeur du slider
    private void OnSliderValueChanged(float value) {
        float steppedValue = RoundToStep(value); // Appliquer le pas
        slider.SetValueWithoutNotify(steppedValue); // Mettre à jour la valeur sans notifier l'événement
        UpdateText(steppedValue); // Mettre à jour le texte affiché
    }
}
