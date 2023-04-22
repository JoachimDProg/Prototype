using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float shieldMinIntensity = 0;
    [SerializeField] private float shieldMaxIntensity = 1;
    [SerializeField] private float shieldAlphaLerpSpeed = 1;
    private float shieldTransition = 0;
    private SpriteRenderer spriteRenderer;
    private Color color;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    void Update()
    {
        color.a = Mathf.Lerp(shieldMinIntensity, shieldMaxIntensity, shieldTransition / shieldAlphaLerpSpeed);

        spriteRenderer.color = color;

        shieldTransition += Time.deltaTime;
        if (shieldTransition >= shieldAlphaLerpSpeed)
            shieldTransition = 0;
    }
}
