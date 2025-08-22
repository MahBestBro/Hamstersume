using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Image racetrackImage;

    public void SetRacingPreview(int raceDay, RacingEventData raceData)
    {
        Vector2 imgDims = racetrackImage.rectTransform.sizeDelta;
        Vector2 newImgDims = _racetrack.ResizeRacecourse(raceData.trackStraightsMultiplier, imgDims); // yucky
        float imgScaleFac = Mathf.Max((newImgDims.x / imgDims.x), (newImgDims.y / imgDims.y));
        racetrackImage.rectTransform.sizeDelta = newImgDims / imgScaleFac;
        Image racetrackImageChild = racetrackImage.rectTransform.GetChild(0)?.GetComponent<Image>();
        if (racetrackImageChild)
        {
            racetrackImageChild.pixelsPerUnitMultiplier *= imgScaleFac;
        }

        const float roundingFactor = 100F;
        float raceDist = _racetrack.CalcTrackDistance(1);
        raceDist = Mathf.Round(raceDist * roundingFactor) / roundingFactor;
        trackLabel.text = "Race #" + raceDay + "\n" +raceDist + "cm " + raceData.trackType.ToUpper();
        _racetrack.straightLength /= raceData.trackStraightsMultiplier; // yucky

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
