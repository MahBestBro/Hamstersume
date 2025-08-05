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
    public float trackStraightsMultiplier = 1F;

    public int TotalParticipants { get { return numberPlayerParticipants + npcParticipants.Count; } }

    public void GenerateRandomRace(
        int numOpponents,
        HamsterVariant[] opponentVariants,
        HamsterProfile defaultProfile,
        int raceIndex,
        int numPlayerRacers = -1
    )
    {
        float progressFactor = raceIndex + 1;
        float statMultiplier = 12.0f * Mathf.Pow(progressFactor, 2);

        if (numPlayerRacers >= 0) this.numberPlayerParticipants = numPlayerRacers;
        this.npcParticipants = new List<HamsterProfile>(numOpponents);
        this.playerParticipants = new List<HamsterProfile>(this.numberPlayerParticipants);
        
        for (int i = 0; i < numOpponents; i++)
        {
            int variantIndex = UnityEngine.Random.Range(0, opponentVariants.Length);
            HamsterVariant variant = opponentVariants[variantIndex];

            HamsterProfile newHamProfile = new HamsterProfile(defaultProfile);
            newHamProfile.hVariant = variant;
            newHamProfile.hStats.statSpeed = (int)(variant.startingSpeedStatFrac * statMultiplier);
            newHamProfile.hStats.statStamina = (int)(variant.startingStaminaStatFrac * statMultiplier);
            newHamProfile.hStats.statPower = (int)(variant.startingPowerStatFrac * statMultiplier);
            
            Debug.Log(variant);

            this.npcParticipants.Add(newHamProfile);
        }
        this.trackType = "Standard";
        this.trackStraightsMultiplier = UnityEngine.Random.Range((int)1, 3) * Mathf.Ceil(progressFactor/3F);
    }
}
