using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RacingEventData
{
    public List<HamsterProfile> npcParticipants;
    public List<HamsterProfile> playerParticipants;
    public int numberPlayerParticipants = 1;
    public string trackType = "Standard";

    public void GenerateRandomRace(int numOpponents, int numPlayerRacers = -1)
    {
        if (numPlayerRacers >= 0) this.numberPlayerParticipants = numPlayerRacers;
        this.npcParticipants = new List<HamsterProfile>(numOpponents);
        this.playerParticipants = new List<HamsterProfile>(this.numberPlayerParticipants);
        for (int i = 0; i < numOpponents; i++)
        {
            HamsterProfile newHamProfile = new HamsterProfile();
            this.npcParticipants.Add(newHamProfile);
        }
        this.trackType = "Standard";
    }
}
