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
