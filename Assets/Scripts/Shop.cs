using System;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Action<int> increaseElectricity;

    [Range(0, 100)]
    public int electricity;

    public HamsterTracker hamsterTracker;

    Text electricityText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        increaseElectricity += IncreaseEletricity;

        electricityText = GameObject.Find("Canvas").transform.Find("ElectricityText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        electricityText.text = electricity.ToString();
    }


    void IncreaseEletricity(int electricityGain)
    {
        electricity += electricityGain;  
    }

    public void CookPizza()
    {
        electricity -= 15 * Convert.ToInt32(electricity >= 15);
    }

    public void CookLasagna()
    {
        electricity -= 10 * Convert.ToInt32(electricity >= 10);
    }

    public void CookCurry()
    {
        electricity -= 5 * Convert.ToInt32(electricity >= 5);
    }
}
