using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    [HideInInspector]
    public HamsterEnergy hEnergy;

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


    /// Display Energy
    public void UpdateEnergyDisplay(int sortingIndex)
    {
        if (imgEnergyMeter)
        {
            Debug.Log(hEnergy.GetPercentEnergy());
            imgEnergyMeter.fillAmount = hEnergy.GetPercentEnergy();
            imgEnergyMeter.canvas.sortingOrder = sortingIndex;
            
            UpdateSleepDisplay();

            if (imgEnergyIncreaseIndicator.gameObject.activeSelf)
            {
                IndicateEnergyIncreaseUpdate();
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
        float percentAfterIncrease = hEnergy.GetPercentOfMax(hEnergy.energy + this.energyIncrAmtBuffer);
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
        float overeatAmtPercent = hEnergy.GetPercentEnergy() - 1F;
        if (overeatAmtPercent > 0F)
        {
            this.sleepDurationRemaining = this.sleepFullDurationSecs * overeatAmtPercent;
        }
        this.hEnergy.ResetToMax();
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

    
}
