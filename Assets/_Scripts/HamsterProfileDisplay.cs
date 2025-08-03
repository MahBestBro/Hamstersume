using UnityEngine;
using UnityEngine.UI;

public class HamsterProfileDisplay : MonoBehaviour
{
    [SerializeField]
    public HamsterStatDisplay statDisplay;
    [SerializeField]
    HamsterProfile hamsterProfile;
    [SerializeField]
    Image hamsterIconImage;

    public void AssignProfile(HamsterProfile profile)
    {
        this.hamsterProfile = profile;
        this.UpdateDisplay();
    }
    public void UpdateDisplay() {
        if (this.hamsterProfile != null)
        {
            statDisplay.UpdateStatDisplay(this.hamsterProfile.hStats);
            hamsterIconImage.sprite = this.hamsterProfile.hVariant.hamsterIdle;
        }
    }
}
