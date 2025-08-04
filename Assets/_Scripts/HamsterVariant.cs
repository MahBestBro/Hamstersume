using UnityEngine;


[CreateAssetMenu(fileName = "HamsterVariant", menuName = "Scriptable Objects/HamsterVariant")]
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
    public float startingMaxEnergy = 100F;

    public Sprite GetSpriteByID(int id)
    {
        switch (id)
        {
            case 0:
                return hamsterIdle;
            case 2:
                return hamsterRunning;
            case 3:
                return hamsterSleeping;
        }
        return null;
    }
}
