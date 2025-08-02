using UnityEngine;

public static class MathHelpers
{
    //NOTE: Mathf.Approximately doesn't give you control over epsilon grrrrr
    public static bool WithinEpsilon(float a, float b, float epsilon)
    {
        return Mathf.Abs(a - b) <= epsilon;
    }

    public static float ExponentialEaseIn(float x)
    {
        return Mathf.Clamp(Mathf.Pow(2.0f, 10.0f * (x - 1.0f)), 0.0f, 1.0f);
    }
    
    public static float ExponentialEaseOut(float x)
    {
        return Mathf.Clamp(1.0f - Mathf.Pow(2.0f, -10.0f * x), 0.0f, 1.0f);
    }
}
