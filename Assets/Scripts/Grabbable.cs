using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    float GRAVITY_ACCEL = 9.81F * 0.1F;
    Vector2 _prevPos;
    Vector2 _velocity;

    protected Collider2D _collider;
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    protected Vector2 grabbedOffset;
    [SerializeField]
    protected LayerMask interactableLayermask;
    public bool pickedUp = false;
    protected Transform sortingAnchor;
    public float SortingPriority { 
        get
        {
            return sortingAnchor.position.y;
        }
    }
    float floorHeight = 1000F;

    protected void Start()
    {
        this._collider = GetComponent<Collider2D>();
        this.AnchorSortingToSprite();
    }

    protected void FixedUpdate()
    {
        this.ProcessFalling(Time.fixedDeltaTime);
    }

    private void ProcessFalling(float fixedDeltaTime)
    {
        if (!this.pickedUp && this.transform.position.y > this.floorHeight)
        {
            if (transform.position.y > 10)
            {
                this._velocity.y = Mathf.Max(0F, -this._velocity.y);
            }
            if (Mathf.Abs(transform.position.x) > 9)
            {
                this._velocity.x *= -0.9F;
            } 
            this._velocity += (Vector2.down * GRAVITY_ACCEL) * fixedDeltaTime;
            Vector3 newPos = (transform.position + (Vector3)this._velocity);
            if (newPos.y < this.floorHeight)
            {
                if (Mathf.Abs(transform.position.x) > 9) {
                    newPos = Vector3.up * 10F;
                    this._prevPos = newPos;
                } else
                {
                    newPos.y = this.floorHeight;
                    this.floorHeight = 1000;
                }
            }
            this.transform.position = newPos;
            this._velocity = (Vector2)newPos - this._prevPos;
            this._prevPos = newPos;
        }
    }

    public bool AnchorSortingToSprite()
    {
        this.sortingAnchor = this.spriteRenderer?.transform;
        return (this.sortingAnchor != null);
    }

    public void GrabAt(Vector2 grabPos)
    {
        this.grabbedOffset = (Vector2)this.transform.position - grabPos;
        this.pickedUp = true;
        this._velocity = Vector2.zero;
    }

    public void DragTo(Vector2 dragPos)
    {
        this.transform.position = (Vector3)(dragPos + grabbedOffset);
        this._velocity = ((Vector2)this.transform.position - this._prevPos) * 0.5F;
        this._prevPos = this.transform.position;
    }

    public Transform HoverInteractable(Vector2 hoverPos)
    {
        ContactFilter2D interactableFilter = new ContactFilter2D();
        interactableFilter.SetLayerMask(interactableLayermask);
        List<Collider2D> touchingInteractables = new List<Collider2D>();
        int numInteractables = this._collider.Overlap(interactableFilter, touchingInteractables);
        Transform targetInteractableTransform = null;
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

    public void DropAt(Vector2 dropPos, Transform interactable, float floorHeight)
    {
        this.pickedUp = false;
        this.floorHeight = floorHeight;
        this._prevPos = this.transform.position;
        this.OnDrop(interactable);
    }

    virtual protected void OnDrop(Transform interactable) { }

    public void RaiseFloorHere()
    {
        this.floorHeight = this.transform.position.y;
    }
}
