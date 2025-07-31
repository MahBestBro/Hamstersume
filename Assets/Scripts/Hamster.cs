using System.Collections;
using UnityEditor.UI;
using UnityEngine;

public enum HamsterState 
{
    Waiting,
    Walking,
    Exercising,
    Tired,
    Eating,
}

public class Hamster : Grabbable
{
    public HamsterState state;
    
    [HideInInspector]
    public int sortingOrder;
    [HideInInspector]
    public Collider2D _collider2D;
    [HideInInspector]
    public HamsterWheel wheel;

    [HideInInspector]
    public float minIdleTimeSecs;
    [HideInInspector]
    public float maxIdleTimeSecs;
    [HideInInspector]
    public float walkSpeed;
    [HideInInspector]
    public float maxEnergy;
    [HideInInspector]
    public float energyLossPerSec;
    [HideInInspector]
    public float energyRegenPerSec;
    [HideInInspector]
    public float tireDurationSecs;
    [HideInInspector]
    public Bounds walkArea;
    //TODO: Walk min and max "radius"

    SpriteRenderer bodySpriteRenderer;
    SpriteRenderer meterSpriteRenderer;
    SpriteRenderer meterBackgroundSpriteRenderer;
    Transform energyMeterTransform;
    Transform energyMeterBarTransform;
    float maxEnergyMeterXScale;

    float idleElapsedTime = 0.0f;
    float idleDuration;
    [SerializeField]
    Vector2 walkDestination = Vector2.zero;

    float wheelEletricityTriggerElapsedTime = 0.0f;
    float tireElapsedTime = 0.0f;
    HamsterState tiredAwakenState = HamsterState.Waiting;

    Food targetFood = null;

    [SerializeField]
    HamsterStats stats;

    float energy
    {
        get => stats.energy;
        set
        {
            stats.energy = value;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new protected void Start()
    {
        base.Start();
        bodySpriteRenderer = this.spriteRenderer;
        _collider2D = GetComponent<Collider2D>();
        
        energyMeterTransform = transform.Find("EnergyMeter");
        
        energyMeterBarTransform = energyMeterTransform.Find("Meter");
        meterSpriteRenderer = energyMeterBarTransform.GetComponent<SpriteRenderer>();
        maxEnergyMeterXScale = energyMeterBarTransform.localScale.x;
        
        meterBackgroundSpriteRenderer = energyMeterTransform.Find("Background").GetComponent<SpriteRenderer>();

        energy = maxEnergy;
        EnterState(HamsterState.Waiting);
    }

    // Update is called once per frame
    protected void Update()
    {

        int screenY = (int)Camera.main.WorldToScreenPoint(transform.position).y; 
        sortingOrder = Screen.height - screenY;
        bodySpriteRenderer.sortingOrder = sortingOrder;
        meterBackgroundSpriteRenderer.sortingOrder = sortingOrder + 1;
        meterSpriteRenderer.sortingOrder = sortingOrder + 2;

        HandleCurrentState(state);

        Vector3 newScale = energyMeterBarTransform.localScale;
        newScale.x = maxEnergyMeterXScale * energy / maxEnergy;
        energyMeterBarTransform.localScale = newScale;
        
        Vector3 xOffset = 0.5f * maxEnergyMeterXScale * (1.0f - energy / maxEnergy) * Vector3.left;
        energyMeterBarTransform.position = energyMeterTransform.position + xOffset;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere((Vector3)walkDestination, 0.25f);
    }


    void HandleCurrentState(HamsterState state)
    {
        switch (state)
        {
            case HamsterState.Waiting:
                idleElapsedTime += Time.deltaTime;
                if (idleElapsedTime >= idleDuration) 
                {
                    EnterState(HamsterState.Walking);
                }

                break;

            case HamsterState.Walking:
                Vector2 toTravel = walkDestination - (Vector2)transform.position; 

                if (!pickedUp)
                {
                    bodySpriteRenderer.flipX = Vector2.Dot(toTravel, Vector2.right) >= 0.0f;
                }

                if (toTravel.magnitude >= walkSpeed * Time.deltaTime)
                {
                    Vector2 direction = toTravel.normalized;
                    transform.position += (Vector3)(walkSpeed * direction * Time.deltaTime);
                }
                else 
                {
                    transform.position = walkDestination;
                    EnterState(HamsterState.Waiting);
                }
                
                break;

            case HamsterState.Exercising:
                wheelEletricityTriggerElapsedTime += Time.deltaTime;
                if (wheelEletricityTriggerElapsedTime >= wheel.energyGainTriggerPeriodSecs)
                {
                    Shop.increaseElectricity?.Invoke(wheel.energyGain);
                    wheelEletricityTriggerElapsedTime -= wheel.energyGainTriggerPeriodSecs;
                }

                energy = Mathf.Max(energy - energyLossPerSec * Time.deltaTime, 0.0f);
                if (energy <= 0.0f) 
                {
                    EnterState(HamsterState.Tired);
                }
                break;

            case HamsterState.Eating:
                if (this.targetFood != null)
                {
                    if (this.targetFood.Consume(this.stats, Time.deltaTime))
                    {
                        this.targetFood = null; // if food is consumed completely, unassign
                        if (this.energy > this.maxEnergy)
                        {
                            this.tireDurationSecs = (this.maxEnergy - this.energy);
                        }
                    }
                } else
                {
                    EnterState(HamsterState.Waiting);
                }
                break;

            case HamsterState.Tired: 
                tireElapsedTime += Time.deltaTime;
                if (tireElapsedTime >= tireDurationSecs)
                {
                    EnterState(this.tiredAwakenState);
                }
                break;
        }
    }

    public void TryEnterState(HamsterState newState)
    {
        if (this.state == HamsterState.Tired)
        {
            tiredAwakenState = newState;
        } 
        else
        {
            EnterState(newState);
        }
    }

    public void EnterState(HamsterState newState) 
    {
        switch (newState)
        {
            case HamsterState.Waiting:
                idleElapsedTime = 0.0f;
                idleDuration = Random.Range(minIdleTimeSecs, maxIdleTimeSecs);
                tiredAwakenState = HamsterState.Waiting;
                break;

            case HamsterState.Walking:
                walkDestination = new Vector2(
                    Random.Range(walkArea.min.x, walkArea.max.x), 
                    Random.Range(walkArea.min.y, walkArea.max.y)
                );
                break;

            case HamsterState.Exercising: 
                transform.position = wheel.transform.position;
                wheelEletricityTriggerElapsedTime = 0.0f;
                tiredAwakenState = HamsterState.Exercising;
                this.RaiseFloorHere();
                break;

            case HamsterState.Tired: 
                tireElapsedTime = 0.0f;
                break;
        }

        state = newState;
    }

    bool EatFood(Food food)
    {
        if (this.targetFood == null)
        {
            this.targetFood = food;
            return true;
        } else {
            return false;
        }
    }

    

    //void ExitCurrentState() 
    //{
    //    switch (state)
    //    {
    //        case HamsterState.Waiting: break;
//
    //        case HamsterState.Walking: break;
//
    //        case HamsterState.PickedUp: break;
    //    }
    //}
}
