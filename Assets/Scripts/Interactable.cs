using UnityEngine;

public class Interactable : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    Material originalMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
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
