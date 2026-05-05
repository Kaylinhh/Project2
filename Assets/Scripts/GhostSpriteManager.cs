using UnityEngine;

public class GhostSpriteManager : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite asleepSprite;
    public Sprite sadSprite;
    public Sprite famishedSprite;
    public Sprite angrySprite;
    public Sprite normalSprite;

    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public GhostStats ghostStats;

    private void OnEnable()
    {
        if (ghostStats != null)
        {
            ghostStats.OnHungerChanged += OnStatChanged;
            ghostStats.OnHappinessChanged += OnStatChanged;
            ghostStats.OnEnergyChanged += OnStatChanged;
            UpdateSprite();
        }
    }

    private void OnDisable()
    {
        if (ghostStats != null)
        {
            ghostStats.OnHungerChanged -= OnStatChanged;
            ghostStats.OnHappinessChanged -= OnStatChanged;
            ghostStats.OnEnergyChanged -= OnStatChanged;
        }
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

        if (isAsleep)
            nextSprite = asleepSprite;
        else if (lowCount >= 2)
            nextSprite = angrySprite;
        else if (isSad)
            nextSprite = sadSprite;
        else if (isFamished)
            nextSprite = famishedSprite;
        else
            nextSprite = normalSprite;

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
}
