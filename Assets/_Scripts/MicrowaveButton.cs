using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MicrowaveButton : MonoBehaviour
{
    const float MOUSE_CLICK_RAYCAST_DELTA = 0.001f; 

    public UnityEvent onClick;

    Collider2D collider2D_;
    InputAction pickUp; //TODO: Rename so it's just left click tbh
    InputAction mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickUp = InputSystem.actions.FindAction("Pick Up");
        mousePos = InputSystem.actions.FindAction("Mouse Pos");
        
        collider2D_ = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos.ReadValue<Vector2>());
        
        if (pickUp.WasReleasedThisFrame())
        {
            Vector2 lineEnd = mouseWorldPos + MOUSE_CLICK_RAYCAST_DELTA * Vector2.right;
            if (collider2D_.OverlapPoint(mouseWorldPos))
            {
                onClick.Invoke();
            }
        }
    }
}
