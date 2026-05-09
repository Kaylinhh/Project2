using System;
using UnityEngine;

public class GhostSpriteManager : MonoBehaviour
{

    public Sprite babySprite;

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
    public GhostEvolution ghostEvolution;

    private void Start()
    {
        SetEvolutionSprite(babySprite);
    }

    private void OnEnable()
    {
        if (ghostEvolution != null)
            ghostEvolution.OnEvolutionSelected += OnEvolutionChosen;
    }

    private void OnDisable()
    {
        if (ghostEvolution != null)
            ghostEvolution.OnEvolutionSelected -= OnEvolutionChosen;
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
            return babySprite;
        }
        else if (stage == GhostEvolution.EvolutionStage.Teen)
        {
            return choice switch
            {
                GhostEvolution.EvolutionChoice.A => teenASprite,
                GhostEvolution.EvolutionChoice.B => teenBSprite,
                GhostEvolution.EvolutionChoice.C => teenCSprite,
                GhostEvolution.EvolutionChoice.Random => GetRandomTeenSprite(),
                _ => null
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
                _ => null
            };
        }

        return null;
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
