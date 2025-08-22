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
        int nPlayer = 1; // + (currentRaceIndex%2); // adding more hamsters introduces complications with rankings
        race.GenerateRandomRace(5 - nPlayer, opponentVariants, prefabProfile, currentRaceIndex, nPlayer);
        return race;
    }

    public bool QueueNextRace()
    {
        this.currentRaceIndex++;
        this.AddRace(this.GenerateRace());
        return (this.currentRaceIndex < this.races.Count);
    }
}
