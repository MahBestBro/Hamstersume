using UnityEngine;

public class GrabbableCapturer : Interactable
{
    public Transform droppedGrabbablesParent;
    public Vector2 droppedScale;
    public bool rejectAll = false;

    private void Start()
    {
        if (droppedGrabbablesParent == null) droppedGrabbablesParent = this.transform;
    }
    protected override void DroppedOn(Grabbable droppedGrabbable)
    {
        base.DroppedOn(droppedGrabbable);
        if (!rejectAll)
        {
            droppedGrabbable.transform.SetParent(droppedGrabbablesParent);
            droppedGrabbable.OnCaptured(this);
        }
    }
    public bool ScaleDroppedTransform(Transform droppedTransform)
    {
        if (droppedScale != Vector2.zero)
        {
            droppedTransform.localScale = droppedScale;
            return true;
        }
        else
        {
            return false;
        }
    }
}
