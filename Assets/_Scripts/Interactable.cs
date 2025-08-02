using UnityEngine;

public class Interactable : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    Material originalMaterial;

    void Start()
    {
        originalMaterial = spriteRenderer.material;
    }

    public void Highlight(Material outlineMaterial)
    {
        spriteRenderer.material = outlineMaterial;
    }

    public void Unhighlight()
    {
        spriteRenderer.material = originalMaterial;
    }
}
