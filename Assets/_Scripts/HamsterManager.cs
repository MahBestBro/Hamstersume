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

    public GameObject hamsterPrefab;

    public Hamster[] GetManagedHamsters()
	{
		return this.GetComponentsInChildren<Hamster>();
	}

    public void NukeManagedHamsters()
    {
        foreach (Hamster hamster in GetManagedHamsters())
        {
            Destroy(hamster.gameObject); 
        }
    }

    public Hamster CreateHamster(HamsterProfile profile)
    {
        float spawnX = UnityEngine.Random.Range(hamsterWalkArea.min.x, hamsterWalkArea.max.x);
        float floorHeight = UnityEngine.Random.Range(hamsterWalkArea.center.y, hamsterWalkArea.max.y);

        GameObject newHamsterObj = Instantiate(
            hamsterPrefab,
            new Vector3(spawnX, floorHeight, 0.0f),
            Quaternion.identity,
            this.transform
        );
        Hamster newHamster = newHamsterObj.GetComponent<Hamster>();
        HamsterEnergy tempEnergy = newHamster.hamsterProfile.hStats.hEnergy;
        newHamster.hamsterProfile = profile;
        newHamster.hamsterProfile.hStats.hEnergy = tempEnergy;
        
        newHamster.isNewHamster = false;

        //TODO: This is weird, refactor
        //HamsterEnergy tempHamsterEnergy = newHamster.hEnergy; 
        //tempHamsterEnergy.maximumEnergy = profile.hStats.hEnergy.maximumEnergy;
        //newHamster.hamsterProfile = profile;
        //newHamster.hamsterProfile.hStats.hEnergy = tempHamsterEnergy;

        //newHamsterObj.SetActive(true);
        //hamsterPrefab.SetActive(true);

        return newHamster;
    }

    public Hamster CreateHamster(HamsterVariant variant)
    {
        float spawnX = UnityEngine.Random.Range(hamsterWalkArea.min.x, hamsterWalkArea.max.x);
        float floorHeight = UnityEngine.Random.Range(hamsterWalkArea.center.y, hamsterWalkArea.max.y);
        //hamsterPrefab.SetActive(false);
        Hamster prefabHamster = hamsterPrefab.GetComponent<Hamster>();
        HamsterVariant originalVariant = prefabHamster.hamsterProfile.hVariant;
        GameObject newHamsterObj = Instantiate(
            hamsterPrefab,
            new Vector3(spawnX, floorHeight, 0.0f),
            Quaternion.identity,
            this.transform
        );
        Hamster newHamster = newHamsterObj.GetComponent<Hamster>();
        newHamster.hamsterProfile.hVariant = variant;

        return newHamster;
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
