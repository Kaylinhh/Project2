using UnityEngine;

[RequireComponent(typeof(Transform))]
public class GhostAnimation : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Distance the ghost travels from its starting point in each direction.")]
    public float moveDistance = 3f;

    [Tooltip("Speed of the ghost movement in units per second.")]
    public float moveSpeed = 2f;

    [Tooltip("Start by moving to the right if true, otherwise to the left.")]
    public bool startRight = true;

    [Tooltip("Use the local position of the transform instead of world position.")]
    public bool useLocalPosition = true;

    [Header("Flip")]
    [Tooltip("Optional sprite renderer to flip horizontally. If not set, the transform scale is flipped.")]
    public SpriteRenderer spriteRenderer;

    [Header("Pause")]
    [Tooltip("Enable random pauses while the ghost is moving.")]
    public bool randomPauseEnabled = true;

    [Tooltip("Probability per second that the ghost will pause while moving.")]
    [Range(0f, 1f)]
    public float pauseChancePerSecond = 0.15f;

    [Tooltip("Enable pauses when the ghost reaches each end of the path.")]
    public bool pauseAtEnds = false;

    [Tooltip("Duration of the pause in seconds.")]
    public float pauseDuration = 1f;

    [Tooltip("Chance to pause at an end point, from 0 to 1.")]
    [Range(0f, 1f)]
    public float pauseChance = 0.75f;

    private Vector3 startPosition;
    private float currentOffset;
    private bool movingRight;
    private bool paused;
    private float pauseTimer;

    private void Start()
    {
        startPosition = useLocalPosition ? transform.localPosition : transform.position;
        movingRight = startRight;
        currentOffset = 0f;
        paused = false;
        pauseTimer = 0f;
        UpdateFlip();
    }

    private void Update()
    {
        if (paused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                paused = false;
            }
            return;
        }

        if (randomPauseEnabled && pauseDuration > 0f && Random.value < pauseChancePerSecond * Time.deltaTime)
        {
            StartPause();
            return;
        }

        float delta = moveSpeed * Time.deltaTime;
        currentOffset += movingRight ? delta : -delta;

        if (currentOffset >= moveDistance)
        {
            currentOffset = moveDistance;
            HandleEndReached();
        }
        else if (currentOffset <= -moveDistance)
        {
            currentOffset = -moveDistance;
            HandleEndReached();
        }

        Vector3 position = startPosition + Vector3.right * currentOffset;
        if (useLocalPosition)
            transform.localPosition = position;
        else
            transform.position = position;
    }

    private void HandleEndReached()
    {
        ReverseDirection();

        if (pauseAtEnds && pauseDuration > 0f && Random.value <= pauseChance)
        {
            StartPause();
        }
    }

    private void StartPause()
    {
        paused = true;
        pauseTimer = pauseDuration;
    }

    private void ReverseDirection()
    {
        movingRight = !movingRight;
        UpdateFlip();
    }

    private void UpdateFlip()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !movingRight;
            return;
        }

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (movingRight ? 1f : -1f);
        transform.localScale = scale;
    }
}
