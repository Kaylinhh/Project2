using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GhostUI : MonoBehaviour
{
    [Header("Stat References")]
    public Slider hungerSlider;
    public Slider happinessSlider;
    public Slider energySlider;

    [Header("Button References")]
    public Button feedButton;
    public Button playButton;
    public Button sleepButton;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Button restartButton;

    [Header("End Game")]
    public GameObject endGamePanel;
    public Button endGameRestartButton;

    [Header("Ghost References")]
    public GhostStats ghostStats;
    public GhostEvolution ghostEvolution;
    public GhostAnimation ghostAnimation;

    private void OnEnable()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

    
        if (ghostStats != null)
        {
            ghostStats.OnHungerChanged += UpdateHunger;
            ghostStats.OnHappinessChanged += UpdateHappiness;
            ghostStats.OnEnergyChanged += UpdateEnergy;
            ghostStats.OnGameOver += HandleGameOver;
        }

        if (ghostEvolution != null)
        {
            ghostEvolution.OnAdultReached += HandleEndGame;
        }

        if (ghostStats != null)
        {
            if (feedButton != null)
                feedButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); ghostStats.Feed(); });
            if (playButton != null)
                playButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); ghostStats.Play(); });
            if (sleepButton != null)
                sleepButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); ghostStats.Sleep(); });
        }

        if (restartButton != null)
            restartButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); RestartGame(); });

        if (endGameRestartButton != null)
            endGameRestartButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); RestartGame(); });
    }

    private void OnDisable()
    {
        if (ghostStats != null)
        {
            ghostStats.OnHungerChanged -= UpdateHunger;
            ghostStats.OnHappinessChanged -= UpdateHappiness;
            ghostStats.OnEnergyChanged -= UpdateEnergy;
            ghostStats.OnGameOver -= HandleGameOver;

            if (feedButton != null)
                feedButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); ghostStats.Feed(); });
            if (playButton != null)
                playButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); ghostStats.Play(); });
            if (sleepButton != null)
                sleepButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); ghostStats.Sleep(); });
        }

        if (ghostEvolution != null)
        {
            ghostEvolution.OnAdultReached -= HandleEndGame;
        }

        if (restartButton != null)
            restartButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); RestartGame(); });

        if (endGameRestartButton != null)
            endGameRestartButton.onClick.RemoveListener(() => { AudioManager.Instance?.PlayButtonClickSFX(); RestartGame(); });
    }

    private void UpdateHunger(float value)
    {
        if (hungerSlider != null)
        {
            hungerSlider.value = value;
            SetSliderColor(hungerSlider, value);
        }
    }

    private void UpdateHappiness(float value)
    {
        if (happinessSlider != null)
        {
            happinessSlider.value = value;
            SetSliderColor(happinessSlider, value);
        }
    }

    private void UpdateEnergy(float value)
    {
        if (energySlider != null)
        {
            energySlider.value = value;
            SetSliderColor(energySlider, value);
        }
    }

    private void SetSliderColor(Slider slider, float value)
    {
        if (slider.fillRect == null)
            return;

        Image fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage == null)
            return;

        if (value < 30f)
            fillImage.color = Color.red;
        else if (value < 60f)
            fillImage.color = new Color(1f, 0.64f, 0f); // orange
        else
            fillImage.color = Color.green;
    }

    private void HandleGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        AudioManager.Instance?.PlayGameOverSFX();

        if (ghostAnimation != null)
            ghostAnimation.enabled = false;

        SetActionButtonsInteractable(false);

        if (hungerSlider != null)
            hungerSlider.interactable = false;
        if (happinessSlider != null)
            happinessSlider.interactable = false;
        if (energySlider != null)
            energySlider.interactable = false;

        if (restartButton != null)
            restartButton.interactable = true;
    }

    private void HandleEndGame()
    {
        ghostStats?.EndGame();
        StartCoroutine(ShowEndGamePanelAfterDelay(3f));
    }

    private IEnumerator ShowEndGamePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (endGamePanel != null)
            endGamePanel.SetActive(true);

        AudioManager.Instance?.PlayEndGameSFX();

        if (ghostAnimation != null)
            ghostAnimation.enabled = false;

        SetActionButtonsInteractable(false);

        if (endGameRestartButton != null)
            endGameRestartButton.interactable = true;
    }

    public void SetActionButtonsInteractable(bool interactable)
    {
        if (feedButton != null)
            feedButton.interactable = interactable;
        if (playButton != null)
            playButton.interactable = interactable;
        if (sleepButton != null)
            sleepButton.interactable = interactable;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
