using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
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

    public bool isNewHamster = true;
    float energyLossPerSec;
    float minIdleTimeSecs;
    float maxIdleTimeSecs;
    float walkSpeed;
    Bounds walkArea;
    //TODO: Walk min and max "radius"

    float idleElapsedTime = 0.0f;
    float idleDuration;
    [SerializeField]
    Vector2 walkDestination = Vector2.zero;

    float wheelEletricityTriggerElapsedTime = 0.0f;
    [SerializeField]
    HamsterState tiredAwakenState = HamsterState.Waiting;

    Food targetFood = null;

    [SerializeField]
    public HamsterProfile hamsterProfile;
    public HamsterVariant hamsterVariant
    {
        get { return hamsterProfile.hVariant; }
    }
    HamsterStats hStats { 
        get { return hamsterProfile.hStats; }
    }
    public HamsterEnergy hEnergy 
    {
        get {  return this.hStats.hEnergy; }
    }

    HamsterStatDisplay statDisplay;

    [SerializeField]
    Hoverable hover;
    
    [HideInInspector]
    public EnergyDisplay energyDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new protected void Start()
    {
        base.Start();
        _collider2D = GetComponent<Collider2D>();

        hover = GetComponent<Hoverable>();

		this.InitialiseFromManager();
        if (isNewHamster) this.InitialiseNewHamster();
        hEnergy.maximumEnergy = hamsterVariant.startingMaxEnergy;

        hEnergy.InitialiseEnergy();
        EnterState(HamsterState.Waiting);

        energyDisplay = transform.Find("EnergyMeter").GetComponent<EnergyDisplay>();
        energyDisplay.hEnergy = hEnergy;
        energyDisplay.SetInitialPos();

        statDisplay = transform.Find("StatDisplay").GetComponent<HamsterStatDisplay>();
        statDisplay.ToggleVisibility(hover.isHovered);
    }

	void InitialiseFromManager()
	{
		HamsterManager hamsterManager = transform.parent.GetComponent<HamsterManager>();
        if (hamsterManager){
            minIdleTimeSecs = hamsterManager.hamsterMinIdleTimeSecs;
            maxIdleTimeSecs = hamsterManager.hamsterMaxIdleTimeSecs;
            walkSpeed = hamsterManager.hamsterWalkSpeed;
            walkArea = hamsterManager.hamsterWalkArea;
            this.physicsBounds = hamsterManager.hamsterPhysicsBounds;
            this.physicsBoundsBuffer = hamsterManager.hamsterPhysicsBoundsBuffer;
            //hEnergy.maximumEnergy = hamsterManager.maxHamsterEnergy;
            energyLossPerSec = hamsterManager.hamsterEnergyLossPerSec;
            energyDisplay.SetFullSleepDuration(hamsterManager.hamsterTireDurationSecs);
        }
	}

    public void InitialiseNewHamster()
    {
        float s = 8.0f * (float)(Math.Max(HamsterDataPocket.instance.raceCircuit.races.Count, 1));
        hStats.statSpeed = (int)(hamsterVariant.startingSpeedStatFrac * s);
        hStats.statStamina = (int)(hamsterVariant.startingStaminaStatFrac * s);
        hStats.statPower = (int)(hamsterVariant.startingPowerStatFrac * s);

        //TODO: HAcky Fix, double check order of instantiation
    }

    public override void OnCaptured(GrabbableCapturer capturer)
    {
        base.OnCaptured(capturer);
        capturer.ScaleDroppedTransform(this.transform);
        this.InitialiseFromManager();
    }

    // Update is called once per frame
    protected void Update()
	{
		this.ComputeSortOrderIndex();
		HandleCurrentState(state);
		energyDisplay.UpdateEnergyDisplay(this.spriteRenderer.sortingOrder);
		this.statDisplay.UpdateStatDisplay(this.hStats, this.spriteRenderer.sortingOrder);
	}

    public void OnHoverEnter_() 
    {
        statDisplay.ToggleVisibility(hover.isHovered);
    }

    public void OnHoverExit_()
    {
        statDisplay.ToggleVisibility(hover.isHovered);
    }

    public override void OnHoverInteractableEnter(Interactable hoverInteractable) {
        base.OnHoverInteractableEnter(hoverInteractable);
        HamsterSelectionBox hamsterBox = hoverInteractable.GetComponent<HamsterSelectionBox>();
        if (hamsterBox)
        {
            hoverInteractable.Highlight();
        }
    }

    public override void OnHoverInteractableExit(Interactable hoverInteractable)
    {
        base.OnHoverInteractableExit(hoverInteractable);
        HamsterSelectionBox hamsterBox = hoverInteractable.GetComponent<HamsterSelectionBox>();
        if (hamsterBox)
        {
            hoverInteractable.Unhighlight();
        }
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
                            EnterState(HamsterState.Tired);
                            energyDisplay.OvereatNap();
                        }
                    }
                } else
                {
                    EnterState(HamsterState.Waiting);
                }
                break;

            case HamsterState.Tired: 
                if (!energyDisplay.TickSleep(Time.deltaTime))
                {
                    EnterState(this.tiredAwakenState);
                }
                break;
        }
    }

    public bool TryEnterState(HamsterState newState)
    {
        if (newState == HamsterState.Eating && this.state == HamsterState.Exercising)
        {
            return false;
        }
        if (this.state == HamsterState.Tired)
        {
            if (newState != HamsterState.Eating)
            {
                tiredAwakenState = newState;
            }
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
        energyDisplay.ResetPos();
        Sprite newSprite = hamsterVariant.hamsterIdle;
        switch (newState)
        {
            case HamsterState.Waiting:
                this.isGrabbable = true;
                idleElapsedTime = 0.0f;
                idleDuration = UnityEngine.Random.Range(minIdleTimeSecs, maxIdleTimeSecs);
                tiredAwakenState = HamsterState.Waiting;
                break;

            case HamsterState.Walking:
                this.isGrabbable = true;
                walkDestination = this.GetRandomWalkDestination();
                break;

            case HamsterState.Exercising:
                energyDisplay.OffsetPos(this.wheel.GetEnergybarOffset());
                wheel.StartSpinning(-1, !this.spriteRenderer.flipX);
                newSprite = hamsterVariant.hamsterRunning;
                this.isGrabbable = true;
                transform.position = wheel.transform.position;
                wheelEletricityTriggerElapsedTime = 0.0f;
                tiredAwakenState = HamsterState.Exercising;
                this.RaiseFloorHere();
                break;

            case HamsterState.Tired:
                newSprite = hamsterVariant.hamsterSleeping;
                wheel?.StopSpinning();
                this.isGrabbable = true;
                energyDisplay.SleepFull();
                break;

            case HamsterState.Eating:
                this.isGrabbable = false;
                break; 
        }

        state = newState;
        newSprite ??= hamsterVariant.hamsterIdle;
        if (newSprite) this.spriteRenderer.sprite = newSprite;
    }

    public override void OnPhysicsReset()
    {
        base.OnPhysicsReset();
        this.floorHeight = this.GetRandomWalkDestination().y;
    }

    public Vector2 GetRandomWalkDestination()
    {
        return new Vector2(
            UnityEngine.Random.Range(walkArea.min.x, walkArea.max.x),
            UnityEngine.Random.Range(walkArea.min.y, walkArea.max.y)
        );
    }

    public bool EatFood(Food food)
    {
        if (this.targetFood == null)
        {
            if (TryEnterState(HamsterState.Eating))
            {
                this.targetFood = food;
                this.targetFood.transform.position = this.spriteRenderer.transform.position;
                this.targetFood.ComputeSortOrderIndex();
                this.targetFood.spriteRenderer.sortingOrder += 1;
                return true;
            }
        }
        return false;
    }
}
