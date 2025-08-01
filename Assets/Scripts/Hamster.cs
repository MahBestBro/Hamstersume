using System.Collections;
using UnityEditor.UI;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
    public Collider2D _collider2D;
    [HideInInspector]
    public HamsterWheel wheel;
    [HideInInspector]
    public float energyLossPerSec;
    [HideInInspector]
    public float tireDurationSecs;
    [HideInInspector]
    public float minIdleTimeSecs;
    [HideInInspector]
    public float maxIdleTimeSecs;
    [HideInInspector]
    public float walkSpeed;
    [HideInInspector]
    public Bounds walkArea;
    //TODO: Walk min and max "radius"

    float idleElapsedTime = 0.0f;
    float idleDuration;
    [SerializeField]
    Vector2 walkDestination = Vector2.zero;

    float wheelEletricityTriggerElapsedTime = 0.0f;
    float tireElapsedTime = 0.0f;
    HamsterState tiredAwakenState = HamsterState.Waiting;

    Food targetFood = null;

    [SerializeField]
    HamsterStats hStats;
    [SerializeField]
    public HamsterEnergy hEnergy {
        get {  return this.hStats.hEnergy; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new protected void Start()
    {
        base.Start();
        _collider2D = GetComponent<Collider2D>();

        hEnergy.InitialiseEnergy();
        EnterState(HamsterState.Waiting);
    }

    // Update is called once per frame
    protected void Update()
    {
        this.ComputeSortOrderIndex();
        HandleCurrentState(state);
        // TODO: Update Energy Meter
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

                if (!isGrabbed)
                {
                    this.spriteRenderer.flipX = Vector2.Dot(toTravel, Vector2.right) >= 0.0f;
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
                    Microwave.increaseElectricity?.Invoke(wheel.energyGain);
                    wheelEletricityTriggerElapsedTime -= wheel.energyGainTriggerPeriodSecs;
                }

                if (hEnergy.ExhaustEnergy(energyLossPerSec * Time.deltaTime)) 
                {
                    EnterState(HamsterState.Tired);
                }
                break;

            case HamsterState.Eating:
                if (this.targetFood != null)
                {
                    if (this.targetFood.Consume(this.hStats, Time.deltaTime))
                    {
                        this.targetFood = null; // if food is consumed completely, unassign
                        float energyExcess = this.hEnergy.GetEnergyExcess();
                        if (energyExcess > 0)
                        {
                            this.tireDurationSecs = energyExcess;
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

    public bool TryEnterState(HamsterState newState)
    {
        if (this.state == HamsterState.Tired)
        {
            tiredAwakenState = newState;
            return false;
        } 
        else
        {
            EnterState(newState);
            return true;
        }
    }

    public void EnterState(HamsterState newState) 
    {
        switch (newState)
        {
            case HamsterState.Waiting:
                this.isGrabbable = true;
                idleElapsedTime = 0.0f;
                idleDuration = Random.Range(minIdleTimeSecs, maxIdleTimeSecs);
                tiredAwakenState = HamsterState.Waiting;
                break;

            case HamsterState.Walking:
                this.isGrabbable = true;
                walkDestination = new Vector2(
                    Random.Range(walkArea.min.x, walkArea.max.x), 
                    Random.Range(walkArea.min.y, walkArea.max.y)
                );
                break;

            case HamsterState.Exercising:
                this.isGrabbable = true;
                transform.position = wheel.transform.position;
                wheelEletricityTriggerElapsedTime = 0.0f;
                tiredAwakenState = HamsterState.Exercising;
                this.RaiseFloorHere();
                break;

            case HamsterState.Tired:
                this.isGrabbable = true;
                tireElapsedTime = 0.0f;
                break;

            case HamsterState.Eating:
                this.isGrabbable = false;
                break; 
        }

        state = newState;
    }

    public bool EatFood(Food food)
    {
        if (this.targetFood == null)
        {
            if (TryEnterState(HamsterState.Eating))
            {
                this.targetFood = food;
                return true;
            }
        }
        return false;
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
