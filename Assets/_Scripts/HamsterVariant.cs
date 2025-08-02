using UnityEngine;


[CreateAssetMenu(fileName = "HamsterVariant", menuName = "Scriptable Objects/HamsterSkinset")]
public class HamsterVariant : ScriptableObject
{
    public Sprite hamsterIdle;
    public Sprite hamsterRunning;
    public Sprite hamsterSleeping;

    [Range(0.0f, 1.0f)]
    public float startingSpeedStatFrac;
    [Range(0.0f, 1.0f)]
    public float startingStaminaStatFrac;
    [Range(0.0f, 1.0f)]
    public float startingPowerStatFrac;
    
    [Range(1.0f, 200.0f)]
    public float startingMaxEnergy;
}
