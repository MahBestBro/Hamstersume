using System;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Range(0, 100)]
    public int electricity;
    [Range(1.0f/60.0f, 10.0f)]
    public float hamsterEletricityGainPeriodSecs;

    public HamsterTracker hamsterTracker;

    float elapsedTime = 0.0f;

    Text electricityText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        electricityText = GameObject.Find("Canvas").transform.Find("ElectricityText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        electricityText.text = electricity.ToString();

        //TODO: Change if certain hamsters can gain more electricity, or have different rates, etc.
        if (elapsedTime >= hamsterEletricityGainPeriodSecs)
        {
            electricity += 5;
            elapsedTime = 0.0f;
        }

        elapsedTime += Time.deltaTime * Convert.ToSingle(hamsterTracker.hamsterWheelMap.Count > 0);
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
