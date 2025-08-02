using UnityEngine;

public static class MathHelpers
{
    //NOTE: Mathf.Approximately doesn't give you control over epsilon grrrrr
    public static bool WithinEpsilon(float a, float b, float epsilon)
    {
        return Mathf.Abs(a - b) <= epsilon;
    }

}
