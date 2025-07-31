using System;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Range(0, 100)]
    public int electricty;

    Text electricityText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        electricityText = GameObject.Find("Canvas").transform.Find("ElectricityText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        electricityText.text = electricty.ToString();
    }


    public void CookPizza()
    {
        electricty -= 15 * Convert.ToInt32(electricty >= 15);
    }

    public void CookLasagna()
    {
        electricty -= 10 * Convert.ToInt32(electricty >= 10);
    }

    public void CookCurry()
    {
        electricty -= 5 * Convert.ToInt32(electricty >= 5);
    }
}
