using System;
using UnityEngine;
using UnityEngine.UI;

public class HamsterStatDisplay : MonoBehaviour
{
    CanvasGroup canvasGroup;

    Image containerImage;

    Image speedMeterImage;
    Image speedIconImage;
    Image staminaMeterImage;
    Image staminaIconImage;
    Image powerMeterImage;
    Image powerIconImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        containerImage = transform.Find("Container").GetComponent<Image>();
        speedMeterImage = transform.Find("SpeedMeter").Find("Meter").GetComponent<Image>();
        speedIconImage = transform.Find("SpeedMeter").Find("Icon").GetComponent<Image>();
        staminaMeterImage = transform.Find("StaminaMeter").Find("Meter").GetComponent<Image>();
        staminaIconImage = transform.Find("StaminaMeter").Find("Icon").GetComponent<Image>();
        powerMeterImage = transform.Find("PowerMeter").Find("Meter").GetComponent<Image>();
        powerIconImage = transform.Find("PowerMeter").Find("Icon").GetComponent<Image>();

        Debug.Log($"here? - { containerImage}");
        Debug.Log($"here? - { speedMeterImage}");
        Debug.Log($"here? - { speedIconImage}");
        Debug.Log($"here? - { staminaMeterImage}");
        Debug.Log($"here? - { staminaIconImage}");
        Debug.Log($"here? - { powerMeterImage}");
        Debug.Log($"here? - { powerIconImage}");
    }


    public void UpdateStatDisplay(HamsterStats stats, int sortingIndex)
    {
        speedMeterImage.fillAmount = stats.SpeedFrac;
        staminaMeterImage.fillAmount = stats.StaminaFrac;
        powerMeterImage.fillAmount = stats.PowerFrac;
        
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
        canvasGroup.alpha = Convert.ToSingle(visible);
    }
}
