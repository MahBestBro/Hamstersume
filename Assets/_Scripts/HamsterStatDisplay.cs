using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HamsterStatDisplay : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;

    Image containerImage;

    Image speedMeterImage;
    Image speedIconImage;
    Image staminaMeterImage;
    Image staminaIconImage;
    Image powerMeterImage;
    Image powerIconImage;

    TMP_Text speedLabel;
    TMP_Text staminaLabel;
    TMP_Text powerLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        containerImage = transform.Find("Container").GetComponent<Image>();

        Transform speedMeter = transform.Find("SpeedMeter");
        speedMeterImage = speedMeter.Find("Meter").GetComponent<Image>();
        speedIconImage =  speedMeter.Find("Icon").GetComponent<Image>();
        speedLabel = speedMeter.Find("StatLabel (TMP)").GetComponent<TMP_Text>();
        
        Transform staminaMeter = transform.Find("StaminaMeter");
        staminaMeterImage = staminaMeter.Find("Meter").GetComponent<Image>();
        staminaIconImage = staminaMeter.Find("Icon").GetComponent<Image>();
        staminaLabel = staminaMeter.Find("StatLabel (TMP)").GetComponent<TMP_Text>();
        
        Transform powerMeter = transform.Find("PowerMeter");
        powerMeterImage = powerMeter.Find("Meter").GetComponent<Image>();
        powerIconImage = powerMeter.Find("Icon").GetComponent<Image>();
        powerLabel = powerMeter.Find("StatLabel (TMP)").GetComponent<TMP_Text>();
    }

    public void UpdateStatDisplay(HamsterStats stats)
    {
        speedMeterImage.fillAmount = stats.SpeedFrac;
        staminaMeterImage.fillAmount = stats.StaminaFrac;
        powerMeterImage.fillAmount = stats.PowerFrac;

        speedLabel.text = $"{stats.statSpeed}";
        staminaLabel.text = $"{stats.statStamina}";
        powerLabel.text = $"{stats.statPower}";
    }

    public void UpdateStatDisplay(HamsterStats stats, int sortingIndex)
    {
        this.UpdateStatDisplay(stats);
        
        containerImage.canvas.sortingOrder = sortingIndex;
        speedMeterImage.canvas.sortingOrder = sortingIndex + 1;
        speedIconImage.canvas.sortingOrder = sortingIndex + 1;
        staminaMeterImage.canvas.sortingOrder = sortingIndex + 1;
        staminaIconImage.canvas.sortingOrder = sortingIndex + 1;
        powerMeterImage.canvas.sortingOrder = sortingIndex + 1;
        powerIconImage.canvas.sortingOrder = sortingIndex + 1;
    }

    public void ToggleVisibility(bool visible)
    {
        if (canvasGroup) canvasGroup.alpha = Convert.ToSingle(visible);
    }
}
