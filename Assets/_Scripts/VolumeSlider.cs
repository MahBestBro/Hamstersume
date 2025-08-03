using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    Scrollbar slider;
    [SerializeField]
    Image fillMask;

    void Start()
    {
        slider = GetComponentInChildren<Scrollbar>();
        slider.value = AudioListener.volume;
        this.UpdateFillmask();
    }


    public void ChangeVolume()
    {
        Soundtrack.SetVolume(slider.value);
        this.UpdateFillmask();
    } 

    void UpdateFillmask()
    {
        if (fillMask )
        {
            this.fillMask.fillAmount = slider.value;
        }
    }
}
