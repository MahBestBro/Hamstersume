using UnityEngine;

public class ScreenButtonToggler : MonoBehaviour
{
    public GameObject microwaveScreenButtonObj;
    public GameObject wheelScreenButtonObj;

    public void TransitionScreens()
    {
        microwaveScreenButtonObj.SetActive(!microwaveScreenButtonObj.activeInHierarchy);
        wheelScreenButtonObj.SetActive(!wheelScreenButtonObj.activeInHierarchy);
    }
}
