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
    [SerializeField]
    Image imgSleepMeter;
    [SerializeField]
    Image imgEnergyIncreaseIndicator;

    private float energyIncrAmtBuffer;
    private float energyIncrIndicatorBaseWidth = 2.43F;

    private float sleepDrainRate = 0.1F;
    private float sleepFullDurationSecs = 10.0F;
    private float sleepDurationRemaining = 0F;


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

    /// Display Energy
    public void UpdateEnergyDisplay(int sortingIndex)
    {
        if(imgEnergyMeter)
        {
            imgEnergyMeter.fillAmount = this.GetPercentEnergy();
            imgEnergyMeter.canvas.sortingOrder = sortingIndex;
            
            this.UpdateSleepDisplay();

            if (imgEnergyIncreaseIndicator.gameObject.activeSelf)
            {
                this.IndicateEnergyIncreaseUpdate();
            }
        }
    }
    public void UpdateSleepDisplay()
    {
        if (imgSleepMeter)
        {
            imgSleepMeter.fillAmount = (sleepDurationRemaining > 0F) ? (sleepDurationRemaining / sleepFullDurationSecs) : 0F;
        }
    }
    public void IndicateEnergyIncreaseStart(float energyIncrease)
    {
        imgEnergyIncreaseIndicator.gameObject.SetActive(true);
        this.energyIncrAmtBuffer = energyIncrease;
        this.IndicateEnergyIncreaseUpdate();
    }
    public void IndicateEnergyIncreaseStop()
    {
        imgEnergyIncreaseIndicator.gameObject.SetActive(false);
    }
    public void IndicateEnergyIncreaseUpdate()
    {
        float percentAfterIncrease = this.GetPercentOfMax(energy + this.energyIncrAmtBuffer);
        Vector2 indicatorSize = imgEnergyIncreaseIndicator.rectTransform.sizeDelta;
        indicatorSize.x = energyIncrIndicatorBaseWidth * percentAfterIncrease;
        imgEnergyIncreaseIndicator.rectTransform.sizeDelta = indicatorSize;
    }

    /// Config Sleep
    
    public void SetFullSleepDuration(float seconds)
    {
        this.sleepFullDurationSecs = seconds;
    }

    /// Update Sleep

    public void SleepFull()
    {
        this.sleepDurationRemaining = this.sleepFullDurationSecs;
    }
    public void OvereatNap()
    {
        float overeatAmtPercent = this.GetPercentEnergy() - 1F;
        if (overeatAmtPercent > 0F)
        {
            this.sleepDurationRemaining = this.sleepFullDurationSecs * overeatAmtPercent;
        }
        this.ResetToMax();
    }
    public bool TickSleep(float deltaTime)
    {
        if (this.sleepDurationRemaining > 0F)
        {
            this.sleepDurationRemaining -= deltaTime;
            return true;
        }
        return false;
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
