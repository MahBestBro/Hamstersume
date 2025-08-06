using UnityEngine;
using UnityEngine.InputSystem;

public class BigCursorAutocontroller : MonoBehaviour
{
    public BigCursor cursor;
    InputAction mousePos;
    InputAction pickUp;
    Hoverable hovered = null;

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

        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(mouseWorldPosition, Camera.main.orthographicSize/5F, Vector2.zero);
        Hoverable hoverable = null;
        foreach(RaycastHit2D hit in hits)
        {
            Hoverable curHoverable = hit.collider?.gameObject.GetComponent<Hoverable>();
            if (curHoverable == null) continue;
            if ( hoverable == null || curHoverable.spriteRenderer.sortingOrder > hoverable.spriteRenderer.sortingOrder || curHoverable == this.hovered)
            {
                hoverable = curHoverable;
            }
        }
        if (hovered == null || hoverable != hovered)
        {
            hovered?.OnHoverExit();
            hoverable?.OnHoverEnter();
            hovered = hoverable;
        }
    }
}
