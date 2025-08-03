using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    Scrollbar slider;

    void Start()
    {
        slider = GetComponentInChildren<Scrollbar>();
        slider.value = Soundtrack.Volume;
    }


    public void ChangeVolume()
    {
        Soundtrack.SetVolume(slider.value);
    } 
}
