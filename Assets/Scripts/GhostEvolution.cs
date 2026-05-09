using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public float firstEvolutionDelay = 10f;
    public float evolutionRepeatInterval = 5f;

    [Header("UI")]
    public GameObject evolutionPopup;
    public TextMeshProUGUI evolutionText;
    public TextMeshProUGUI countdownText;
    public Button choiceAButton;
    public Button choiceBButton;
    public Button choiceCButton;
    public Button choiceRandomButton;

    [Header("References")]
    public GhostStats ghostStats;
    public GhostUI ghostUI;

    private float elapsedTime;
    private float nextEvolutionDelay;
    private bool popupOpen;
    private EvolutionStage currentStage;

    public event Action<EvolutionChoice, EvolutionStage> OnEvolutionSelected;
    public event Action OnAdultReached;

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
            choiceAButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); OnChoiceA(); });
        if (choiceBButton != null)
            choiceBButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); OnChoiceB(); });
        if (choiceCButton != null)
            choiceCButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); OnChoiceC(); });
        if (choiceRandomButton != null)
            choiceRandomButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); OnChoiceRandom(); });
    }

    private void OnDisable()
    {
        if (choiceAButton != null)
            choiceAButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); OnChoiceA(); });
        if (choiceBButton != null)
            choiceBButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); OnChoiceB(); });
        if (choiceCButton != null)
            choiceCButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); OnChoiceC(); });
        if (choiceRandomButton != null)
            choiceRandomButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); OnChoiceRandom(); });
    }

    private void OnChoiceA() => HandleChoice(EvolutionChoice.A);
    private void OnChoiceB() => HandleChoice(EvolutionChoice.B);
    private void OnChoiceC() => HandleChoice(EvolutionChoice.C);
    private void OnChoiceRandom() => HandleChoice(EvolutionChoice.Random);

    private void Update()
    {
        if (popupOpen || currentStage == EvolutionStage.Adult)
        {
            ClearCountdownText();
            return;
        }

        elapsedTime += Time.deltaTime;
        UpdateCountdownText();

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

        AudioManager.Instance?.PlayEvolutionSFX();

        if (evolutionText != null)
        {
            if (currentStage == EvolutionStage.Baby)
                evolutionText.text = "Your baby ghost evolved! How do you think it will turn out?";
            else if (currentStage == EvolutionStage.Teen)
                evolutionText.text = "Your teen ghost evolved! How do you think it will turn out?";
        }

        SetChoiceButtonTexts();

        evolutionPopup.SetActive(true);
        popupOpen = true;
        ghostStats?.PauseStatDecay();
        ghostUI?.SetActionButtonsInteractable(false);
    }

    private void SetChoiceButtonTexts()
    {
        if (choiceAButton == null || choiceBButton == null || choiceCButton == null || choiceRandomButton == null)
            return;

        if (currentStage == EvolutionStage.Baby)
        {
            SetButtonText(choiceAButton, "cute");
            SetButtonText(choiceBButton, "spooky");
            SetButtonText(choiceCButton, "mysterious");
        }
        else if (currentStage == EvolutionStage.Teen)
        {
            SetButtonText(choiceAButton, "chic");
            SetButtonText(choiceBButton, "sinister");
            SetButtonText(choiceCButton, "mystical");
        }
        else
        {
            SetButtonText(choiceAButton, "Choice A");
            SetButtonText(choiceBButton, "Choice B");
            SetButtonText(choiceCButton, "Choice C");
        }

        SetButtonText(choiceRandomButton, "I don't know");
    }

    private void SetButtonText(Button button, string text)
    {
        if (button == null)
            return;

        var label = button.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
            label.text = text;
    }

    private void UpdateCountdownText()
    {
        if (countdownText == null)
            return;

        float remainingSeconds = Mathf.Max(0f, nextEvolutionDelay - elapsedTime);
        countdownText.text = $"Something is happening! In {Mathf.CeilToInt(remainingSeconds)} seconds";
    }

    private void ClearCountdownText()
    {
        if (countdownText != null)
            countdownText.text = string.Empty;
    }

    private void HandleChoice(EvolutionChoice choice)
    {
        bool willReachAdult = currentStage == EvolutionStage.Teen;
        AdvanceStage();

        if (evolutionPopup != null)
            evolutionPopup.SetActive(false);

        popupOpen = false;

        if (willReachAdult)
        {
            ghostStats?.EndGame();
            ghostUI?.SetActionButtonsInteractable(false);
        }
        else
        {
            ghostStats?.ResumeStatDecay();
            ghostUI?.SetActionButtonsInteractable(true);
        }

        OnEvolutionSelected?.Invoke(choice, currentStage);
    }

    private void AdvanceStage()
    {
        if (currentStage == EvolutionStage.Adult)
            return;

        if (currentStage == EvolutionStage.Baby)
            currentStage = EvolutionStage.Teen;
        else if (currentStage == EvolutionStage.Teen)
        {
            currentStage = EvolutionStage.Adult;
            OnAdultReached?.Invoke();
        }
    }
}
