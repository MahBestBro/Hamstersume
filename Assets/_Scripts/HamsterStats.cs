using UnityEngine;

[System.Serializable]
public class HamsterStats
{
    const int PSEUDO_STAT_MAX = 60;

    public int statSpeed;
    public int statStamina;
    public int statPower;

    public float SpeedFrac
    {
        get 
        {
            return (float)statSpeed / (float)PSEUDO_STAT_MAX;
        }
    }

    public float StaminaFrac
    {
        get 
        {
            return (float)statStamina / (float)PSEUDO_STAT_MAX;
        }
    }


    public float PowerFrac
    {
        get 
        {
            return (float)statPower / (float)PSEUDO_STAT_MAX;
        }
    }


    public HamsterEnergy hEnergy;
}
