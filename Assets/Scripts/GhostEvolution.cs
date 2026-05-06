using System;
using UnityEngine;
using UnityEngine.UI;

public class GhostEvolution : MonoBehaviour
{
    public enum EvolutionChoice
    {
        A,
        B,
        C,
        Random
    }

    public enum EvolutionStage
    {
        Baby,
        Teen,
        Adult
    }

    [Header("Timing")]
    public float firstEvolutionDelay = 60f;
    public float evolutionRepeatInterval = 120f;

    [Header("UI")]
    public GameObject evolutionPopup;
    public Text evolutionText;
    public Button choiceAButton;
    public Button choiceBButton;
    public Button choiceCButton;
    public Button choiceRandomButton;

    [Header("References")]
    public GhostStats ghostStats;

    private float elapsedTime;
    private float nextEvolutionDelay;
    private bool popupOpen;
    private EvolutionStage currentStage;

    public event Action<EvolutionChoice, EvolutionStage> OnEvolutionSelected;

    private void Start()
    {
        elapsedTime = 0f;
        nextEvolutionDelay = firstEvolutionDelay;
        popupOpen = false;
        currentStage = EvolutionStage.Baby;

        if (evolutionPopup != null)
            evolutionPopup.SetActive(false);
    }

    private void OnEnable()
    {
        if (choiceAButton != null)
            choiceAButton.onClick.AddListener(OnChoiceA);
        if (choiceBButton != null)
            choiceBButton.onClick.AddListener(OnChoiceB);
        if (choiceCButton != null)
            choiceCButton.onClick.AddListener(OnChoiceC);
        if (choiceRandomButton != null)
            choiceRandomButton.onClick.AddListener(OnChoiceRandom);
    }

    private void OnDisable()
    {
        if (choiceAButton != null)
            choiceAButton.onClick.RemoveListener(OnChoiceA);
        if (choiceBButton != null)
            choiceBButton.onClick.RemoveListener(OnChoiceB);
        if (choiceCButton != null)
            choiceCButton.onClick.RemoveListener(OnChoiceC);
        if (choiceRandomButton != null)
            choiceRandomButton.onClick.RemoveListener(OnChoiceRandom);
    }

    private void OnChoiceA() => HandleChoice(EvolutionChoice.A);
    private void OnChoiceB() => HandleChoice(EvolutionChoice.B);
    private void OnChoiceC() => HandleChoice(EvolutionChoice.C);
    private void OnChoiceRandom() => HandleChoice(EvolutionChoice.Random);

    private void Update()
    {
        if (popupOpen || currentStage == EvolutionStage.Adult)
            return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= nextEvolutionDelay)
        {
            ShowEvolutionPopup();
            elapsedTime = 0f;
            nextEvolutionDelay = evolutionRepeatInterval;
        }
    }

    private void ShowEvolutionPopup()
    {
        if (evolutionPopup == null)
            return;

        if (evolutionText != null)
            evolutionText.text = "Une évolution est disponible ! Choisissez une voie pour votre fantôme.";

        evolutionPopup.SetActive(true);
        popupOpen = true;
    }

    private void HandleChoice(EvolutionChoice choice)
    {
        if (evolutionPopup != null)
            evolutionPopup.SetActive(false);

        popupOpen = false;
        OnEvolutionSelected?.Invoke(choice, currentStage);
        AdvanceStage();
    }

    private void AdvanceStage()
    {
        if (currentStage == EvolutionStage.Adult)
            return;

        if (currentStage == EvolutionStage.Baby)
            currentStage = EvolutionStage.Teen;
        else if (currentStage == EvolutionStage.Teen)
            currentStage = EvolutionStage.Adult;
    }
}
