using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathWave
{
    public static float CalculateSine(float valueToSine, float frequency, float offset, float amplitude, float inverted)
    {
        float sin = Mathf.Sin(valueToSine * frequency + (offset * Mathf.PI)) * amplitude * inverted;

        return sin;
    }
}
