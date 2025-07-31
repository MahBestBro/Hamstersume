using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    protected Collider2D collider;
    public SpriteRenderer spriteRenderer;
    protected Vector2 grabbedOffset;
    [SerializeField]
    protected LayerMask interactableLayermask;
    public bool pickedUp = false;

    protected void Start()
    {
        this.collider = GetComponent<Collider2D>();
    }

    public void GrabAt(Vector2 grabPos)
    {
        this.grabbedOffset = (Vector2)this.transform.position - grabPos;
        this.pickedUp = true;
    }

    public void DragTo(Vector2 dragPos)
    {
        this.transform.position = (Vector3)(dragPos + grabbedOffset);
    }

    public Transform HoverInteractable(Vector2 hoverPos)
    {
        ContactFilter2D interactableFilter = new ContactFilter2D();
        interactableFilter.SetLayerMask(interactableLayermask);
        List<Collider2D> touchingInteractables = new List<Collider2D>();
        int numInteractables = this.collider.Overlap(interactableFilter, touchingInteractables);
        Transform? targetInteractableTransform = null;
        //NOTE: This assumes wheels do not overlap, I don't know why they would anyways
        //Debug.Log("Hey");
        if (numInteractables > 0)
        {
            targetInteractableTransform = touchingInteractables[0].transform;
            return targetInteractableTransform;
        }

        //Vector2 lineEnd = hoverPos + MOUSE_CLICK_RAYCAST_DELTA * Vector2.right;
        //RaycastHit2D wheelHit = Physics2D.Linecast(hoverPos, lineEnd, mask);
        //if (targetWheel is Transform wheel) 
        //{
        //TODO: Hover animation/effect    
        //}
        return null;
    }

    public void DropAt(Vector2 dropPos)
    {
        this.pickedUp = false;
    }
}
