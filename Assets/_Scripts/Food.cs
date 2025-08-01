using UnityEngine;

public class Food : Grabbable
{
    [SerializeField]
    protected float consumeDuration = 1.0F;
    [SerializeField]
    protected float energyRestored = 10.0F;
    

    public float EnergyProvided
    {
        get
        {
            return this.energyRestored;
        }
    }

    /*
     * <returns>
     * True if food is fully consumed, False if food is partially consumed
     * </returns>
     */
    public bool Consume(HamsterStats consumer, float elapsedTime)
    {
        this.consumeDuration -= elapsedTime;
        if (this.consumeDuration > 0.0F)
        {
            return false;
        }
        this.OnConsumed(consumer);
        return true;
    }

    protected bool OnConsumed(HamsterStats consumer)
    {
        consumer.hEnergy.RestoreFixedEnergy(this.energyRestored);
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
}
