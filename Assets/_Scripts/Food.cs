using UnityEngine;

public class Food : Grabbable
{
    //[SerializeField]
    //protected float energyRestored = 10.0F;
    //[SerializeField]
    //protected float consumeDuration = 1.0F;
    //
    //[SerializeField]
    //[Range(0, 100)]
    //protected int speedStatIncrease;
    //[SerializeField]
    //[Range(0, 100)]
    //protected int staminaStatIncrease;
    //[SerializeField]
    //[Range(0, 100)]
    //protected int powerStatIncrease;
//
    //[Range(0, 800)]
    //public int electricityCost;

    [SerializeField]
    FoodStats foodStats;

    public FoodStats Stats
    {
        get
        {
            return foodStats;
        }
    } 

    public float EnergyProvided
    {
        get
        {
            return foodStats.energyRestored;
        }
    }

    /*
     * <returns>
     * True if food is fully consumed, False if food is partially consumed
     * </returns>
     */
    public bool Consume(HamsterStats consumer, float elapsedTime)
    {
        foodStats.consumeDuration -= elapsedTime;
        if (foodStats.consumeDuration > 0.0F)
        {
            return false;
        }
        this.OnConsumed(consumer);
        return true;
    }

    protected bool OnConsumed(HamsterStats consumer)
    {
        consumer.hEnergy.RestoreFixedEnergy(foodStats.energyRestored);

        consumer.statSpeed += foodStats.speedStatIncrease;
        consumer.statStamina += foodStats.staminaStatIncrease;
        consumer.statPower += foodStats.powerStatIncrease;

        Destroy(this.gameObject);
        return true;
    }

    override protected void OnDrop(Transform interactable) {
        base.OnDrop(interactable);
        if (interactable != null)
        {
            Hamster hamster = interactable.GetComponent<Hamster>();
            if (hamster != null)
            {
                if (hamster.EatFood(this))
                {
                    this.isGrabbable = false;
                }
            }
        }
    }

    public override void OnHoverInteractableEnter(Interactable hoverInteractable)
    {
        Hamster hoveredHamster = hoverInteractable?.GetComponent<Hamster>();
        if (hoveredHamster) {
            hoveredHamster.energyDisplay.IndicateEnergyIncreaseStart(foodStats.energyRestored);
            if (hoveredHamster.state == HamsterState.Walking)
            {
                hoveredHamster.TryEnterState(HamsterState.Waiting);
            }
        }
    }
    public override void OnHoverInteractableExit(Interactable hoverInteractable)
    {
        Hamster hoveredHamster = hoverInteractable?.GetComponent<Hamster>();
        if (hoveredHamster)
        {
            hoveredHamster.energyDisplay.IndicateEnergyIncreaseStop();
        }
    }
}
