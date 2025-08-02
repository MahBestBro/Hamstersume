using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine.InputSystem;
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
    public bool isGrabbed = false;
    public bool isGrabbable = true;
    public bool isHovered = false;
    protected Transform sortingAnchor;
    public float SortingPriority { 
        get
        {
            return sortingAnchor.position.y;
        }
    }
    float floorHeight = 1000F;

    InputAction mousePos;

    protected void Start()
    {
        this._collider = GetComponent<Collider2D>();
        this.AnchorSortingToSprite();

        this.mousePos = InputSystem.actions.FindAction("Mouse Pos");
    }

    protected void Update()
    {
        if (this.isHovered)
        {
            HandleHover();
        }
    }

    protected void FixedUpdate()
    {
        if ( this.isGrabbable )
        {
            this.ProcessFalling(Time.fixedDeltaTime);
        }
    }

    private void ProcessFalling(float fixedDeltaTime)
    {
        if (!this.isGrabbed && this.transform.position.y > this.floorHeight)
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

    private void HandleHover()
    {
        Vector2 mousePosition = this.mousePos.ReadValue<Vector2>();
        Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePosition);
        if (this.isHovered && !_collider.OverlapPoint(mouseWorldPos))
        {
            this.isHovered = false;
        }
        else 
        {
            this.OnHover();
        }
    }

    public void EnterHover()
    {
        this.isHovered = true;
    }

    public bool AnchorSortingToSprite()
    {
        this.sortingAnchor = this.spriteRenderer?.transform;
        bool res = (this.sortingAnchor != null);
        this.ComputeSortOrderIndex();
        return res;
    }

    public int ComputeSortOrderIndex()
    {
        int sortOrder;
        if ( this.isGrabbed )
        {
            sortOrder = Screen.height;
        } else
        {
            int screenY = (int)Camera.main.WorldToScreenPoint(this.sortingAnchor?.position ?? this.transform.position).y;
            sortOrder = Screen.height - screenY;
        }
        this.spriteRenderer.sortingOrder = sortOrder;
        return sortOrder;
    }

    public bool GrabAt(Vector2 grabPos)
    {
        if (!isGrabbable) return false;
        this.grabbedOffset = (Vector2)this.transform.position - grabPos;
        this.isGrabbed = true;
        this._velocity = Vector2.zero;
        return true;
    }

    public void DragTo(Vector2 dragPos)
    {
        this.transform.position = (Vector3)(dragPos + grabbedOffset);
        // update sorting
        this.ComputeSortOrderIndex();
        // update physics trackers
        this._velocity = ((Vector2)this.transform.position - this._prevPos) * 0.5F;
        this._prevPos = this.transform.position;
    }

    public Interactable HoverInteractable(Vector2 hoverPos)
    {
        ContactFilter2D interactableFilter = new ContactFilter2D();
        interactableFilter.SetLayerMask(interactableLayermask);
        List<Collider2D> touchingInteractables = new List<Collider2D>();
        int numInteractables = this._collider.Overlap(interactableFilter, touchingInteractables);
        Transform targetInteractableTransform = null;
        //TODO: Handle overlapping case (e.g., food drop over two hamsters)
        if (numInteractables > 0)
        {
            targetInteractableTransform = touchingInteractables[0].transform;
            return targetInteractableTransform.GetComponent<Interactable>();
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
        this.isGrabbed = false;
        this.floorHeight = floorHeight;
        this._prevPos = this.transform.position;
        this.OnDrop(interactable);
    }

    virtual protected void OnHover() { }

    virtual protected void OnDrop(Transform interactable) { }

    public void RaiseFloorHere()
    {
        this.floorHeight = this.transform.position.y;
    }
}
