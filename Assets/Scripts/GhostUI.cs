using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Ghost References")]
    public GhostStats ghostStats;

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

        if (ghostStats != null)
        {
            if (feedButton != null)
                feedButton.onClick.AddListener(ghostStats.Feed);
            if (playButton != null)
                playButton.onClick.AddListener(ghostStats.Play);
            if (sleepButton != null)
                sleepButton.onClick.AddListener(ghostStats.Sleep);
        }

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
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
                feedButton.onClick.RemoveListener(ghostStats.Feed);
            if (playButton != null)
                playButton.onClick.RemoveListener(ghostStats.Play);
            if (sleepButton != null)
                sleepButton.onClick.RemoveListener(ghostStats.Sleep);
        }

        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartGame);
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

        if (feedButton != null)
            feedButton.interactable = false;
        if (playButton != null)
            playButton.interactable = false;
        if (sleepButton != null)
            sleepButton.interactable = false;

        if (hungerSlider != null)
            hungerSlider.interactable = false;
        if (happinessSlider != null)
            happinessSlider.interactable = false;
        if (energySlider != null)
            energySlider.interactable = false;

        if (restartButton != null)
            restartButton.interactable = true;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
