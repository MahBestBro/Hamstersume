using UnityEngine;

public class GrabbableCapturer : Interactable
{
    public Transform droppedGrabbablesParent;

    private void Start()
    {
        if (droppedGrabbablesParent == null) droppedGrabbablesParent = this.transform;
    }
    public override void DroppedOn(Grabbable droppedGrabbable)
    {
        base.DroppedOn(droppedGrabbable);
        droppedGrabbable.transform.SetParent(droppedGrabbablesParent);
        droppedGrabbable.OnCaptured();
    }
}
