using UnityEngine;

public class HamsterManager : MonoBehaviour
{
    [Range(0, 100.0f)]
    public float hamsterMinIdleTimeSecs;
    [Range(0, 100.0f)]
    public float hamsterMaxIdleTimeSecs;
    [Range(0, 100.0f)]
    public float hamsterWalkSpeed;
    public Bounds hamsterWalkArea;
    [Range(0, 100.0f)]
    public float maxHamsterEnergy;
    [Range(0, 100.0f)]
    public float hamsterEnergyLossPerSec;
    [Range(0, 180.0f)]
    public float hamsterTireDurationSecs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            Hamster hamster = transform.GetChild(i).GetComponent<Hamster>();
            hamster.minIdleTimeSecs = hamsterMinIdleTimeSecs;
            hamster.maxIdleTimeSecs = hamsterMaxIdleTimeSecs;
            hamster.walkSpeed = hamsterWalkSpeed;
            hamster.walkArea = hamsterWalkArea;
            hamster.hEnergy.maximumEnergy = maxHamsterEnergy;
            hamster.energyLossPerSec = hamsterEnergyLossPerSec;
            hamster.hEnergy.SetFullSleepDuration(hamsterTireDurationSecs);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hamsterWalkArea.center, hamsterWalkArea.size);
    }
}
