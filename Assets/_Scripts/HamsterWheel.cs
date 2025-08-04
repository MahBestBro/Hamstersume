using System;
using UnityEngine;

public class HamsterWheel : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public SpinAnimation spinAnimation;
    public Animator animator;
    int anim_spinSpeed;

    [Range(0, 50)]
    public int energyGain;
    [Range(0.0f, 10.0f)]
    public float energyGainTriggerPeriodSecs;

    public Transform energybarOffsetAnchor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        anim_spinSpeed = Animator.StringToHash("spinSpeed");
    }

    public Vector2 GetEnergybarOffset()
    {
        return energybarOffsetAnchor.position - this.transform.position;
    }

    public void StartSpinning(float speed, bool clockwise)
    {
        float spinningSpeed = 1F;
        //float spinningSpeed = this.spinAnimation.spinSpeed;
        if (speed != -1) spinningSpeed = speed;
        spinningSpeed = (Mathf.Abs(spinningSpeed) * ((clockwise) ? -1F : 1F));
        //this.spinAnimation.spinSpeed = spinningSpeed;
        //this.spinAnimation.spinning = true;
        animator.SetFloat(anim_spinSpeed, spinningSpeed);
        animator.speed = Mathf.Abs(spinningSpeed);
    }

    public void StopSpinning()
    {
        //this.spinAnimation.spinning = false;
        animator.speed = 1F;
        animator.SetFloat(anim_spinSpeed, 0F);
    }
}
