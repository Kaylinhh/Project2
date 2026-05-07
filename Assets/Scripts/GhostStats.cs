using System;
using UnityEngine;

public class GhostStats : MonoBehaviour
{
    // Stats (0-100)
    [SerializeField] private float hunger = 50f;
    [SerializeField] private float happiness = 50f;
    [SerializeField] private float energy = 50f;

    // Stat decrease/increase rate (per second)
    [SerializeField] private float hungerDecreaseRate = 5f;
    [SerializeField] private float happinessDecreaseRate = 3f;
    [SerializeField] private float energyDecreaseRate = 2f;

    // Amounts changed by actions
    [SerializeField] private float feedAmount = 30f;
    [SerializeField] private float playAmount = 25f;
    [SerializeField] private float sleepAmount = 40f;

    public Action<float> OnHungerChanged;
    public Action<float> OnHappinessChanged;
    public Action<float> OnEnergyChanged;
    public Action OnGameOver;

    private bool isGameOver;
    private bool pauseStatDecay;

    private void Update()
    {
        if (isGameOver || pauseStatDecay)
            return;

        // Decrease stats automatically over time
        SetHunger(hunger - hungerDecreaseRate * Time.deltaTime);
        SetHappiness(happiness - happinessDecreaseRate * Time.deltaTime);
        SetEnergy(energy - energyDecreaseRate * Time.deltaTime);
    }

    public void PauseStatDecay()
    {
        pauseStatDecay = true;
    }

    public void ResumeStatDecay()
    {
        pauseStatDecay = false;
    }

    /// <summary>Feed the ghost (decreases Hunger)</summary>
    public void Feed()
    {
        SetHunger(hunger + feedAmount);
        Debug.Log($"Feed! Hunger: {hunger:F1}");
    }

    /// <summary>Play with the ghost (increases Happiness)</summary>
    public void Play()
    {
        SetHappiness(happiness + playAmount);
        SetEnergy(energy - 10f); // Playing causes fatigue
        Debug.Log($"Play! Happiness: {happiness:F1}, Energy: {energy:F1}");
    }

    /// <summary>Make the ghost sleep (increases Energy)</summary>
    public void Sleep()
    {
        SetEnergy(energy + sleepAmount);
        SetHunger(hunger + 10f); // Sleeping increases hunger
        Debug.Log($"Sleep! Energy: {energy:F1}, Hunger: {hunger:F1}");
    }

    private void SetHunger(float value)
    {
        float clamped = Mathf.Clamp(value, 0f, 100f);
        if (Mathf.Approximately(clamped, hunger))
            return;

        hunger = clamped;
        OnHungerChanged?.Invoke(hunger);
        CheckGameOver();
    }

    private void SetHappiness(float value)
    {
        float clamped = Mathf.Clamp(value, 0f, 100f);
        if (Mathf.Approximately(clamped, happiness))
            return;

        happiness = clamped;
        OnHappinessChanged?.Invoke(happiness);
        CheckGameOver();
    }

    private void SetEnergy(float value)
    {
        float clamped = Mathf.Clamp(value, 0f, 100f);
        if (Mathf.Approximately(clamped, energy))
            return;

        energy = clamped;
        OnEnergyChanged?.Invoke(energy);
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (isGameOver)
            return;

        if (hunger <= 0f || happiness <= 0f || energy <= 0f)
        {
            isGameOver = true;
            OnGameOver?.Invoke();
        }
    }

    // Getters to access stats
    public float GetHunger() => hunger;
    public float GetHappiness() => happiness;
    public float GetEnergy() => energy;
}
