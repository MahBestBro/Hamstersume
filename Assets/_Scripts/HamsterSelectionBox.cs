using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HamsterSelectionBox : MonoBehaviour
{
    [SerializeField]
    GrabbableCapturer boxCapturer;
    [SerializeField]
    public Text selectionCounterText;
    public int selectionLimit = 1;
    HashSet<Hamster> selectedHamsters = new HashSet<Hamster>();

    [SerializeField]
    ScreenTransition endTransition;
    public UnityEvent<List<HamsterProfile>> onSelectionConfirmed;

    private void Start()
    {
        UpdateCounterDisplay();
    }

    public void OnDroppedInto(Grabbable droppedGrabbable)
    {
        if (droppedGrabbable is Hamster hamster)
        {
            if (!boxCapturer.rejectAll)
            {
                selectedHamsters.Add(hamster);
                this.UpdateCounterDisplay();
                if (selectedHamsters.Count >= selectionLimit)
                {
                    boxCapturer.rejectAll = true;
                }
            }
        }
    }

    public void OnDroppedOutof(Grabbable droppedGrabbable)
    {
        if (droppedGrabbable is Hamster hamster && selectedHamsters.Contains(hamster))
        {
            if (selectedHamsters.Remove(hamster))
            {
                boxCapturer.rejectAll = false;
                this.UpdateCounterDisplay();
            }
        }
    }

    public List<HamsterProfile> GetSelectedHamsterProfiles()
    {
        List <HamsterProfile> hamsterProfiles = new List<HamsterProfile>();
        foreach (Hamster hamster in this.selectedHamsters)
        {
            hamsterProfiles.Add(hamster.hamsterProfile);
        }
        return hamsterProfiles;
    }

    public void ConfirmSelection()
    {
        Debug.Log(selectedHamsters.Count);
        if (selectedHamsters.Count < selectionLimit) return;

        onSelectionConfirmed.Invoke(GetSelectedHamsterProfiles());

        // Transition to next scene 
        UnityEvent onTransitionEnd = new UnityEvent();
        onTransitionEnd.AddListener(() => SceneManager.LoadScene("Racing"));
        endTransition.Play(onTransitionEnd);
    }

    public void UpdateCounterDisplay()
    {
        if(selectionCounterText) selectionCounterText.text = selectedHamsters.Count.ToString() + "/" + selectionLimit.ToString();
    }
}
