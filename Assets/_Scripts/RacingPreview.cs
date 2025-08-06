using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RacingPreview : MonoBehaviour
{
    [SerializeField]
    public GameObject popup;
    [SerializeField]
    public Text trackLabel;
    [SerializeField]
    public GameObject racerList;
    [SerializeField]
    public HamsterProfileDisplay racerProfileEntryPrefab;

    public RaceTrack _racetrack;

    public void SetRacingPreview(RacingEventData raceData)
    {
        _racetrack.straightLength *= raceData.trackStraightsMultiplier; // yucky
        const float roundingFactor = 100F;
        float raceDist = _racetrack.CalcTrackDistance(1);
        raceDist = Mathf.Round(raceDist * roundingFactor) / roundingFactor;
        trackLabel.text = raceDist + "cm " + raceData.trackType.ToUpper();
        _racetrack.straightLength /= raceData.trackStraightsMultiplier;
        this.PopulateRacerList(raceData.npcParticipants);
    }

    public void PopulateRacerList(List<HamsterProfile> racerProfiles)
    {
        bool popupClosed = (popup && !popup.activeInHierarchy);
        if (popupClosed) { TogglePopup(); }
        foreach(HamsterProfile profile in racerProfiles)
        {
            HamsterProfileDisplay profileDisplay = Instantiate(racerProfileEntryPrefab, racerList.transform);
            profileDisplay.AssignProfile(profile);
        }
        if (popupClosed) { TogglePopup(); }
    }

    public void TogglePopup()
    {
        if (popup != null)
        {
            popup.SetActive(!popup.activeInHierarchy);
        }
    }
}
