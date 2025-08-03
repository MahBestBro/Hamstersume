using System.Collections.Generic;
using UnityEngine;

public class RacingCircuit : MonoBehaviour
{
    [SerializeField]
    public List<RacingEventData> races;
    [SerializeField]
    int currentRaceIndex = 0;

    [SerializeField]
    GameObject hamsterPrefab;

    [SerializeField]
    HamsterVariant[] opponentVariants;

    HamsterProfile prefabProfile;

    public RacingEventData CurrentRace 
    { 
        get { return (currentRaceIndex <= races.Count) ? races[currentRaceIndex] : null; }
    }

    private void Start()
    {
        prefabProfile = hamsterPrefab.GetComponent<Hamster>().hamsterProfile;
        this.AddRace(this.GenerateRace());
    }

    void AddRace(RacingEventData race)
    {
        this.races.Add(race);
    }

    RacingEventData GenerateRace()
    {
        RacingEventData race = new RacingEventData();
        race.GenerateRandomRace(3, opponentVariants, prefabProfile, currentRaceIndex);
        return race;
    }

    public bool QueueNextRace()
    {
        this.AddRace(this.GenerateRace());
        this.currentRaceIndex++;
        return (this.currentRaceIndex < this.races.Count);
    }
}
