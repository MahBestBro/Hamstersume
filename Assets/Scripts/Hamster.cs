using System.Collections;
using UnityEngine;

public enum HamsterState 
{
    Waiting,
    Walking,
    Exercising,
    Tired,
    Eating,
}

public class Hamster : MonoBehaviour
{
    public Collider2D collider2D_;
    
    public HamsterState state;
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

    SpriteRenderer spriteRenderer;

    float idleElapsedTime = 0.0f;
    float idleDuration;
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
    void Start()
    {
        energy = maxEnergy;
        EnterState(HamsterState.Waiting);

        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D_ = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        int screenY = (int)Camera.main.WorldToScreenPoint(transform.position).y; 
        spriteRenderer.sortingOrder = Screen.height - screenY;

        HandleCurrentState(state);
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
        } else
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
                    Random.Range(walkArea.min.x, walkArea.max.x)
                );
                break;

            case HamsterState.Exercising: 
                transform.position = wheel.transform.position;
                wheelEletricityTriggerElapsedTime = 0.0f;
                tiredAwakenState = HamsterState.Exercising;
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
