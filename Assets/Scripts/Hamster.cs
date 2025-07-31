using UnityEngine;

public class Hamster : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int screenY = (int)Camera.main.WorldToScreenPoint(transform.position).y; 
        spriteRenderer.sortingOrder = Screen.height - screenY; 
    }
}
