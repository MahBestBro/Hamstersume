using System.Collections.Generic;
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
    public Bounds hamsterPhysicsBounds = new Bounds(Vector2.zero, new Vector2(18, 20));
    public float hamsterPhysicsBoundsBuffer = 1F;
    [Range(0, 100.0f)]
    public float maxHamsterEnergy;
    [Range(0, 100.0f)]
    public float hamsterEnergyLossPerSec;
    [Range(0, 180.0f)]
    public float hamsterTireDurationSecs;

	public Hamster[] GetManagedHamsters()
	{
		return this.GetComponentsInChildren<Hamster>();
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hamsterWalkArea.center, hamsterWalkArea.size);
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireCube(hamsterPhysicsBounds.center, hamsterPhysicsBounds.size);
        Gizmos.DrawWireCube(hamsterPhysicsBounds.center, hamsterPhysicsBounds.size+(Vector3.one*2F*hamsterPhysicsBoundsBuffer));
    }
}
