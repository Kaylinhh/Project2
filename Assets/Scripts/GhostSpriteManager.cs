using System;
using UnityEngine;

public class GhostSpriteManager : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite asleepSprite;
    public Sprite sadSprite;
    public Sprite famishedSprite;
    public Sprite angrySprite;
    public Sprite normalSprite;

    [Header("Baby Evolution Sprites")]
    public Sprite choiceASprite;
    public Sprite choiceBSprite;
    public Sprite choiceCSprite;
    public Sprite randomSprite;

    [Header("Teen Evolution Sprites")]
    public Sprite teenASprite;
    public Sprite teenBSprite;
    public Sprite teenCSprite;

    [Header("Adult Evolution Sprites")]
    public Sprite adultASprite;
    public Sprite adultBSprite;
    public Sprite adultCSprite;

    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public GhostStats ghostStats;
    public GhostEvolution ghostEvolution;

    private void OnEnable()
    {
        if (ghostStats != null)
        {
            ghostStats.OnHungerChanged += OnStatChanged;
            ghostStats.OnHappinessChanged += OnStatChanged;
            ghostStats.OnEnergyChanged += OnStatChanged;
            UpdateSprite();
        }

        if (ghostEvolution != null)
            ghostEvolution.OnEvolutionSelected += OnEvolutionChosen;
    }

    private void OnDisable()
    {
        if (ghostStats != null)
        {
            ghostStats.OnHungerChanged -= OnStatChanged;
            ghostStats.OnHappinessChanged -= OnStatChanged;
            ghostStats.OnEnergyChanged -= OnStatChanged;
        }

        if (ghostEvolution != null)
            ghostEvolution.OnEvolutionSelected -= OnEvolutionChosen;
    }

    private void OnStatChanged(float _)
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null || ghostStats == null)
            return;

        float hunger = ghostStats.GetHunger();
        float happiness = ghostStats.GetHappiness();
        float energy = ghostStats.GetEnergy();

        bool isAsleep = energy < 30f;
        bool isSad = happiness < 30f;
        bool isFamished = hunger < 30f;
        int lowCount = 0;

        if (isAsleep) lowCount++;
        if (isSad) lowCount++;
        if (isFamished) lowCount++;

        Sprite nextSprite = normalSprite;

        if (lowCount >= 2)
            nextSprite = angrySprite;
        else if (isAsleep)
            nextSprite = asleepSprite;
        else if (isSad)
            nextSprite = sadSprite;
        else if (isFamished)
            nextSprite = famishedSprite;
        else
            nextSprite = normalSprite;

        if (nextSprite != null)
            spriteRenderer.sprite = nextSprite;
    }

    public void ApplyEvolution(GhostEvolution.EvolutionChoice choice, GhostEvolution.EvolutionStage stage)
    {
        Sprite chosenSprite = GetSpriteForChoice(choice, stage);
        SetEvolutionSprite(chosenSprite);
    }

    private Sprite GetSpriteForChoice(GhostEvolution.EvolutionChoice choice, GhostEvolution.EvolutionStage stage)
    {
        if (stage == GhostEvolution.EvolutionStage.Baby)
        {
            return choice switch
            {
                GhostEvolution.EvolutionChoice.A => choiceASprite,
                GhostEvolution.EvolutionChoice.B => choiceBSprite,
                GhostEvolution.EvolutionChoice.C => choiceCSprite,
                GhostEvolution.EvolutionChoice.Random => GetRandomBabySprite(),
                _ => normalSprite
            };
        }
        else if (stage == GhostEvolution.EvolutionStage.Teen)
        {
            return choice switch
            {
                GhostEvolution.EvolutionChoice.A => teenASprite,
                GhostEvolution.EvolutionChoice.B => teenBSprite,
                GhostEvolution.EvolutionChoice.C => teenCSprite,
                GhostEvolution.EvolutionChoice.Random => GetRandomTeenSprite(),
                _ => normalSprite
            };
        }
        else if (stage == GhostEvolution.EvolutionStage.Adult)
        {
            return choice switch
            {
                GhostEvolution.EvolutionChoice.A => adultASprite,
                GhostEvolution.EvolutionChoice.B => adultBSprite,
                GhostEvolution.EvolutionChoice.C => adultCSprite,
                GhostEvolution.EvolutionChoice.Random => GetRandomAdultSprite(),
                _ => normalSprite
            };
        }

        return normalSprite;
    }

    private Sprite GetRandomBabySprite()
    {
        Sprite[] sprites = new[] { choiceASprite, choiceBSprite, choiceCSprite, randomSprite };
        int index = UnityEngine.Random.Range(0, sprites.Length);
        return sprites[index];
    }

    private Sprite GetRandomTeenSprite()
    {
        Sprite[] sprites = new[] { teenASprite, teenBSprite, teenCSprite };
        int index = UnityEngine.Random.Range(0, sprites.Length);
        return sprites[index];
    }

    private Sprite GetRandomAdultSprite()
    {
        Sprite[] sprites = new[] { adultASprite, adultBSprite, adultCSprite };
        int index = UnityEngine.Random.Range(0, sprites.Length);
        return sprites[index];
    }

    private void SetEvolutionSprite(Sprite sprite)
    {
        if (spriteRenderer != null && sprite != null)
            spriteRenderer.sprite = sprite;
    }

    private void OnEvolutionChosen(GhostEvolution.EvolutionChoice choice, GhostEvolution.EvolutionStage stage)
    {
        ApplyEvolution(choice, stage);
    }
}
