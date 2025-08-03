using System.Collections.Generic;
using UnityEngine;

public class RacingPreview : MonoBehaviour
{
    [SerializeField]
    public GameObject popup;
    [SerializeField]
    public GameObject racerList;
    [SerializeField]
    public HamsterProfileDisplay racerProfileEntryPrefab;

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
