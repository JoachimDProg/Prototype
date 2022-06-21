using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float shieldMinIntensity = 0;
    [SerializeField] private float shieldMaxIntensity = 1;
    private float shieldTransition = 0;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color color = spriteRenderer.color;

        color.a = Mathf.Lerp(shieldMinIntensity, shieldMaxIntensity, shieldTransition);

        shieldTransition += Time.deltaTime;
        if (shieldTransition >= 1)
            shieldTransition = 0;

        spriteRenderer.color = color;
    }
}
