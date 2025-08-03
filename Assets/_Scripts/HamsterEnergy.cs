using UnityEngine;

[System.Serializable]
public class HamsterEnergy
{
    [SerializeField]
    public float energy;

    [SerializeField]
    public float maximumEnergy;


    /// Check Energy
    public float GetEnergyDefecit()
    {
        return Mathf.Max(maximumEnergy - energy, 0F);
    }
    public float GetPercentEnergy()
    {
        return energy / maximumEnergy;
    }
    public float GetPercentOfMax(float energyAmt)
    {
        return energyAmt / maximumEnergy;
    }
    public float GetPercentEnergyDefecit()
    {
        return GetEnergyDefecit() / maximumEnergy;
    }
    public float GetEnergyExcess()
    {
        return Mathf.Max(energy - maximumEnergy, 0F);
    }



    /// Use Energy
    public bool ExhaustEnergy(float energySpent)
    {
        energy -= energySpent;
        if (energy < 0)
        {
            energy = 0;
            return true;
        }
        return false;
    }

    /// Refilling Energy

    public void InitialiseEnergy()
    {
        this.ResetToMax();
    }

    public void ResetToMax()
    {
        this.energy = this.maximumEnergy;
    }

    public bool RestoreFixedEnergy(float energyRestored)
    {
        this.energy += energyRestored;
        if (this.energy > maximumEnergy)
        {
            return false;
        }
        return true;
    }
}
