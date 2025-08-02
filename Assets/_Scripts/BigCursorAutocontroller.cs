using UnityEngine;
using UnityEngine.InputSystem;

public class BigCursorAutocontroller : MonoBehaviour
{
    public BigCursor cursor;
    InputAction mousePos;
    InputAction pickUp;

    private void Start()
    {
        mousePos = InputSystem.actions.FindAction("Mouse Pos");
        pickUp = InputSystem.actions.FindAction("Pick Up");
    }
    private void Update()
    {
        Vector2 mousePosition = this.mousePos.ReadValue<Vector2>();
        cursor.Goto(mousePosition);

        if (pickUp.WasPressedThisFrame())
        {
            this.cursor.OnLeftClickDown();
        }
        else if (pickUp.WasReleasedThisFrame())
        {
            this.cursor.OnLeftClickUp();
        }
    }
}
