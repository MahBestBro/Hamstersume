using UnityEngine;

[CreateAssetMenu(fileName = "FoodStats", menuName = "Scriptable Objects/FoodStats")]
public class FoodStats : ScriptableObject
{
    [Range(0, 100)]
    public int speedStatIncrease;
    [Range(0, 100)]
    public int staminaStatIncrease;
    [Range(0, 100)]
    public int powerStatIncrease;

    [Range(0, 800)]
    public int electricityCost;

    [SerializeField]
    public float energyRestored = 10.0F;
    
    [SerializeField]
    public float consumeDuration = 1.0F;
}
