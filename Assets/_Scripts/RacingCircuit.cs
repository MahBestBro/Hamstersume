using System.Collections.Generic;
using UnityEngine;

public class RacingCircuit : MonoBehaviour
{
    [SerializeField]
    public List<RacingEventData> races;
    [SerializeField]
    int currentRaceIndex = 0;
    public int RacesCompleted { get { return currentRaceIndex; } }

    [SerializeField]
    GameObject hamsterPrefab;

    [SerializeField]
    HamsterVariant[] opponentVariants;

    HamsterProfile prefabProfile;

    public RacingEventData CurrentRace 
    { 
        get { return (currentRaceIndex <= races.Count) ? races[currentRaceIndex] : null; }
    }

    public void InitialiseCircuit()
    {
        prefabProfile = hamsterPrefab.GetComponent<Hamster>().hamsterProfile;
        for (int i = 0; i <= currentRaceIndex; i++) // TEMP
        {
            this.AddRace(this.GenerateRace());
        }
    }

    void AddRace(RacingEventData race)
    {
        this.races.Add(race);
    }

    RacingEventData GenerateRace()
    {
        RacingEventData race = new RacingEventData();
        race.GenerateRandomRace(4, opponentVariants, prefabProfile, currentRaceIndex);
        return race;
    }

    public bool QueueNextRace()
    {
        this.AddRace(this.GenerateRace());
        this.currentRaceIndex++;
        return (this.currentRaceIndex < this.races.Count);
    }
}
