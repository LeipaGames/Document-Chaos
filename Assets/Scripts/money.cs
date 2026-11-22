using UnityEngine;

public class Money : MonoBehaviour
{
    // ========== ========== ========== ========== ========== Variables

    float timer;
    float timerMax = 1;
    float riseMultiplier = 0.5f;
    float fadeMultiplier = 1;

    private float startPosition;
    private SpriteRenderer spriteRenderer;

    // ========== ========== ========== ========== ========== Start & Update

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position.y;
    }

    void Update()
    {
        timer += Time.deltaTime;
        spriteRenderer.color = new Color(1, 1, 1, 1 - fadeMultiplier * timer);
        transform.position = new Vector2(transform.position.x, startPosition + riseMultiplier * timer);
        if (timer > timerMax) Destroy(gameObject);
    }
}
