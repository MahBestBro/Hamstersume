using System.Collections.Generic;
using UnityEngine;

public class RacingCircuit : MonoBehaviour
{
    [SerializeField]
    public List<RacingEventData> races;
    [SerializeField]
    int currentRaceIndex = 0;

    public RacingEventData CurrentRace { 
        get { return (currentRaceIndex <= races.Count) ? races[currentRaceIndex] : null; }
    }

    private void Start()
    {
        this.AddRace(this.GenerateRace());
    }

    void AddRace(RacingEventData race)
    {
        this.races.Add(race);
    }

    RacingEventData GenerateRace()
    {
        RacingEventData race = new RacingEventData();
        race.GenerateRandomRace(3, 1);
        return race;
    }

    public bool QueueNextRace()
    {
        this.currentRaceIndex++;
        return (this.currentRaceIndex < this.races.Count);
    }
}
