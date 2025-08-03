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

    public int TotalParticipants { get { return numberPlayerParticipants + npcParticipants.Count; } }

    public void GenerateRandomRace(
        int numOpponents, 
        HamsterVariant[] opponentVariants, 
        HamsterProfile defaultProfile, 
        int raceIndex,
        int numPlayerRacers = -1
    )
    {
        float statMultiplier = 12.0f * (int)(raceIndex + 1);

        if (numPlayerRacers >= 0) this.numberPlayerParticipants = numPlayerRacers;
        this.npcParticipants = new List<HamsterProfile>(numOpponents);
        this.playerParticipants = new List<HamsterProfile>(this.numberPlayerParticipants);
        for (int i = 0; i < numOpponents; i++)
        {
            int variantIndex = UnityEngine.Random.Range(0, opponentVariants.Length);
            HamsterVariant variant = opponentVariants[variantIndex];

            HamsterProfile newHamProfile = defaultProfile;
            newHamProfile.hVariant = variant;
            newHamProfile.hStats.statSpeed = (int)(variant.startingSpeedStatFrac * statMultiplier);
            newHamProfile.hStats.statStamina = (int)(variant.startingStaminaStatFrac * statMultiplier);
            newHamProfile.hStats.statPower = (int)(variant.startingPowerStatFrac * statMultiplier);
            
            Debug.Log(variantIndex);

            this.npcParticipants.Add(newHamProfile);
        }
        this.trackType = "Standard";
    }
}
