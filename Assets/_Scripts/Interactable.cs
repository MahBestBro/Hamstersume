using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Material defaultHighlightMateral;
    Material originalMaterial;

    public UnityEvent<Grabbable> onDroppedOnEvent;

    void Start()
    {
        if (spriteRenderer)
        {
            originalMaterial = spriteRenderer.material;
        }
    }

    public void Highlight(Material outlineMaterial = null)
    {
        if(spriteRenderer)
        {
            spriteRenderer.material = outlineMaterial ?? defaultHighlightMateral;
        }
    }

    public void Unhighlight()
    {
        if (spriteRenderer)
        {
            spriteRenderer.material = originalMaterial;
        }
    }

    public void ReceiveDropped(Grabbable droppedGrabbable)
    {
        this.DroppedOn(droppedGrabbable);
        onDroppedOnEvent.Invoke(droppedGrabbable);
    }
    protected virtual void DroppedOn(Grabbable droppedGrabbable)
    {
    }
}
