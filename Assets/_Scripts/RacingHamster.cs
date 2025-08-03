using System.ComponentModel;
using UnityEngine;

public class RacingHamster : MonoBehaviour
{
    [SerializeField]
    public HamsterProfile hamsterProfile;
    [SerializeField]
    public float endurance;
    public float maxEndurance;
    [SerializeField]
    float velocity = 0;
    float minSpeed = 3.0F;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float acceleration = 0;
    float decelerationFactor = 1F;
    [SerializeField]
    float burstAcceleration;
    bool isSprinting = false;
    [SerializeField]
    bool isTired = false;

    public int laneNumber;

    [HideInInspector]
    public Collider2D collider2D_;

    public Racecourse racecourse;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    float distanceTravelled;
    [SerializeField]
    float _raceCompletion;


    public float RaceCompletion
    {
        get 
        {
            float totalStraightDistance = 2.0f * racecourse.straightLength;
            float curveRadius = racecourse.minCurveRadius + racecourse.laneWidth * ((float)laneNumber - 1.0f);
            float totalCurveDistance = 2.0f * Mathf.PI * curveRadius;
            float trackDistance = totalStraightDistance + totalCurveDistance;

            return distanceTravelled / trackDistance;
        }
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider2D_ = transform.GetComponent<Collider2D>();
        if (spriteRenderer == null) spriteRenderer = transform.Find("HamsterSprite").GetComponent<SpriteRenderer>();
    }

    public void InitialiseSelf(Racecourse _racecourse)
    {
        this.racecourse = _racecourse;
        this.Start();
        this.spriteRenderer.sprite = hamsterProfile.hVariant.hamsterIdle;

        this.maxSpeed = this.minSpeed + this.hamsterProfile.hStats.statSpeed;
        this.velocity = this.maxSpeed;
        this.burstAcceleration = Mathf.Max(this.hamsterProfile.hStats.statPower, 0F);
        this.maxEndurance = Mathf.Min(this.hamsterProfile.hStats.statStamina * 5F, 3F);
        this.endurance = this.maxEndurance;
        this.isTired = false;
        this.isSprinting = false;
    }

    float TickSpeed(float deltaTime)
    {
        if (this.isTired)
        {
            this.endurance = this.endurance + (deltaTime * 0.5F);
            this.acceleration = -decelerationFactor;
            if (this.endurance >= this.maxEndurance) {
                this.endurance = this.maxEndurance;
                this.isTired = false;
                this.acceleration = this.burstAcceleration;
                this.isSprinting = true;
            }
        } else
        {
            this.endurance = this.endurance - deltaTime;
            if (this.endurance <= 0F)
            {
                this.endurance = 0F;
                this.isTired = true;
            }
            if (!this.isSprinting)
            {
                this.acceleration = -(1F - (this.endurance / this.maxEndurance)) * decelerationFactor;
            }
        }        
        this.velocity = Mathf.Min(Mathf.Max(minSpeed, this.velocity + this.acceleration * deltaTime), this.maxSpeed);
        return this.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (racecourse == null) return;

        if ((bool)racecourse?.RaceIsUnderway())
        {
            this.TickSpeed(Time.fixedDeltaTime);
            this.spriteRenderer.sprite = hamsterProfile.hVariant.hamsterRunning;
            float distanceCovered = 0.0f;
            transform.position = racecourse.NextPosOnRaceCourse(
                transform.position,
                this.velocity,
                laneNumber,
                ref distanceCovered
            );
            distanceTravelled += distanceCovered;
        }

        spriteRenderer.flipX = racecourse.GetRaceFacing(transform.position) == Facing.Right;

        _raceCompletion = RaceCompletion;
    }

    private void FixedUpdate()
    {
        
    }
}
