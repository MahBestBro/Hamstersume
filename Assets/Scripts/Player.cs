using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputAction pickUp;
    InputAction mousePos;
    
    Transform? heldHamster = null;
    Vector2 heldHamsterOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickUp = InputSystem.actions.FindAction("Pick Up");
        mousePos = InputSystem.actions.FindAction("Mouse Pos");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos.ReadValue<Vector2>());

        if (pickUp.WasPressedThisFrame()) {
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, 0.001f * Vector2.right);
            if (hit) {
                heldHamster = hit.transform;
                heldHamsterOffset = (Vector2)hit.transform.position - mouseWorldPos; 
            } 
        }

        if (heldHamster is Transform hamster) {
            hamster.position = (Vector3)(mouseWorldPos + heldHamsterOffset);
        }

        if (pickUp.WasReleasedThisFrame()) {
            heldHamster = null;
        }
    }
}
