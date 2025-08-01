using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HamsterEnergy
{
    [SerializeField]
    public float energy;

    [SerializeField]
    public float maximumEnergy;

    [SerializeField]
    public Image imgEnergyMeter;


    /// Check Energy
    public float GetEnergyDefecit()
    {
        return Mathf.Max(maximumEnergy - energy, 0F);
    }
    public float GetPercentEnergy()
    {
        return energy / maximumEnergy;
    }
    public float GetPercentEnergyDefecit()
    {
        return GetEnergyDefecit() / maximumEnergy;
    }
    public float GetEnergyExcess()
    {
        return Mathf.Max(energy - maximumEnergy, 0F);
    }

    /// Display Energy
    public void UpdateEnergyDisplay(int sortingIndex)
    {
        if(imgEnergyMeter)
        {
            imgEnergyMeter.fillAmount = this.GetPercentEnergy();
        }
    }


    /// Use Energy
    public bool ExhaustEnergy(float energySpent)
    {
        this.energy -= energySpent;
        if (this.energy < 0)
        {
            this.energy = 0;
            return true;
        }
        return false;
    }


    /// Refilling Energy

    public void InitialiseEnergy()
    {
        this.RefillMax();
    }

    public void RefillMax()
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
