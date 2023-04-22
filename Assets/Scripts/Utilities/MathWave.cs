using UnityEngine;

public static class MathWave
{
    public static float CalculateSine(float valueToSine, float frequency, float offset, float amplitude, float inverted)
    {
        return Mathf.Sin(valueToSine * frequency + (offset * Mathf.PI)) * amplitude * inverted;
    }
}
