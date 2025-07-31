using UnityEngine;

public class Food : MonoBehaviour
{
    protected float consumeDuration = 1.0F;
    protected float energyRestored = 1.0F;

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
        consumer.energy += this.energyRestored;
        Destroy(this.gameObject);
        return true;
    }
}
