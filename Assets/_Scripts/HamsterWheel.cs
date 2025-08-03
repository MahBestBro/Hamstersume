using System;
using UnityEngine;

public class HamsterWheel : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public SpinAnimation spinAnimation;

    [Range(0, 50)]
    public int energyGain;
    [Range(0.0f, 10.0f)]
    public float energyGainTriggerPeriodSecs;

    public Transform energybarOffsetAnchor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector2 GetEnergybarOffset()
    {
        return energybarOffsetAnchor.position - this.transform.position;
    }

    public void StartSpinning(float speed, bool clockwise)
    {
        if (speed != -1) this.spinAnimation.spinSpeed = speed;
        this.spinAnimation.spinSpeed = (Mathf.Abs(this.spinAnimation.spinSpeed) * ((clockwise) ? -1F : 1F));
        this.spinAnimation.spinning = true;
    }

    public void StopSpinning()
    {
        this.spinAnimation.spinning = false;
    }
}
