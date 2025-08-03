using System.Collections.Generic;
using UnityEngine;

public class HamsterSelectionBox : MonoBehaviour
{
    [SerializeField]
    GrabbableCapturer boxCapturer;
    public int selectionLimit = 1;
    HashSet<Hamster> selectedHamsters = new HashSet<Hamster>();
    public void OnDroppedInto(Grabbable droppedGrabbable)
    {
        if (droppedGrabbable is Hamster hamster)
        {
            selectedHamsters.Add(hamster);
            if (selectedHamsters.Count  >= selectionLimit)
            {
                boxCapturer.rejectAll = true;
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
            }
        }
    }
}
