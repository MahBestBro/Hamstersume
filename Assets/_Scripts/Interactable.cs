using UnityEngine;

public class Interactable : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Material defaultHighlightMateral;
    Material originalMaterial;

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

    public virtual void DroppedOn(Grabbable droppedGrabbable)
    {
    }
}
