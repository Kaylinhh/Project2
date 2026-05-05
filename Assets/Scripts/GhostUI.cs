using UnityEngine;
using UnityEngine.UI;

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

    [Header("Ghost References")]
    public GhostStats ghostStats;

    private void OnEnable()
    {
        if (ghostStats != null)
        {
            ghostStats.OnHungerChanged += UpdateHunger;
            ghostStats.OnHappinessChanged += UpdateHappiness;
            ghostStats.OnEnergyChanged += UpdateEnergy;
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
    }

    private void OnDisable()
    {
        if (ghostStats != null)
        {
            ghostStats.OnHungerChanged -= UpdateHunger;
            ghostStats.OnHappinessChanged -= UpdateHappiness;
            ghostStats.OnEnergyChanged -= UpdateEnergy;

            if (feedButton != null)
                feedButton.onClick.RemoveListener(ghostStats.Feed);
            if (playButton != null)
                playButton.onClick.RemoveListener(ghostStats.Play);
            if (sleepButton != null)
                sleepButton.onClick.RemoveListener(ghostStats.Sleep);
        }
    }

    private void UpdateHunger(float value)
    {
        if (hungerSlider != null)
            hungerSlider.value = value;
    }

    private void UpdateHappiness(float value)
    {
        if (happinessSlider != null)
            happinessSlider.value = value;
    }

    private void UpdateEnergy(float value)
    {
        if (energySlider != null)
            energySlider.value = value;
    }
}
