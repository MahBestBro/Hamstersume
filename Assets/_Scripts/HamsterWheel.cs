using System;
using UnityEngine;

public class HamsterWheel : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 

    [Range(0, 50)]
    public int energyGain;
    [Range(0.0f, 10.0f)]
    public float energyGainTriggerPeriodSecs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
