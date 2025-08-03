using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    float GRAVITY_ACCEL = 9.81F * 0.1F;
    Vector2 _prevPos;
    Vector2 _velocity;
    public Bounds physicsBounds = new Bounds(Vector2.zero, new Vector2(18, 20));
    protected float physicsBoundsBuffer = 1F;

    protected Collider2D _collider;
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    protected Vector2 grabbedOffset;
    [SerializeField]
    protected LayerMask interactableLayermask;
    public bool isGrabbed = false;
    public bool isGrabbable = true;
    public bool isHovered = false;
    Interactable hoveredInteractable;
    protected Transform sortingAnchor;
    public float SortingPriority { 
        get
        {
            return sortingAnchor.position.y;
        }
    }
    protected float floorHeight = 1000F;

    InputAction mousePos;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(physicsBounds.center, physicsBounds.size);
    }

    protected void Start()
    {
        this._collider = GetComponent<Collider2D>();
        this.AnchorSortingToSprite();

        this.mousePos = InputSystem.actions.FindAction("Mouse Pos");
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
        if (!this.isGrabbed)
        {
            bool resetPosition = 
                (transform.position.y > physicsBounds.max.y + physicsBoundsBuffer ||
                transform.position.y < physicsBounds.min.y - physicsBoundsBuffer ||
                transform.position.x > physicsBounds.max.x + physicsBoundsBuffer ||
                transform.position.x < physicsBounds.min.x - physicsBoundsBuffer);
            
            if (!resetPosition) {
                if (this.transform.position.y > this.floorHeight)
                {
                    // Bounce off walls
                    if (transform.position.y > physicsBounds.max.y)
                    {
                        this._velocity.y = Mathf.Max(0F, -this._velocity.y);
                    }
                    if (transform.position.x > physicsBounds.max.x ||
                        transform.position.x < physicsBounds.min.x)
                    {
                        this._velocity.x *= -0.9F;
                    }
                    // Gravity
                    this._velocity += (Vector2.down * GRAVITY_ACCEL) * fixedDeltaTime;
                    // Compute Position
                    Vector3 newPos = (transform.position + (Vector3)this._velocity);
                    if (newPos.y < this.floorHeight) // If at floor, stop
                    {
                        newPos.y = this.floorHeight;
                        this.floorHeight = physicsBounds.max.y * 2F;
                    }
                    this.transform.position = newPos;
                    this._velocity = (Vector2)newPos - this._prevPos;
                    this._prevPos = newPos;
                }
            } else
            {
                this.transform.position = new Vector2(physicsBounds.center.x, physicsBounds.max.y - physicsBoundsBuffer);
                this._prevPos = this.transform.position;
                this._velocity = Vector2.zero;
                this.OnPhysicsReset();
            }
           
        }
    }
    public virtual void OnPhysicsReset() { }
    public virtual void OnHoverEnter()
    {
        this.isHovered = true;
    }
    public virtual void OnHoverExit()
    {
        this.isHovered = false;
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

    public Interactable CheckHoverInteractable(Vector2 hoverPos)
    {
        ContactFilter2D interactableFilter = new ContactFilter2D();
        interactableFilter.SetLayerMask(interactableLayermask);
        List<Collider2D> touchingInteractables = new List<Collider2D>();
        int numInteractables = Physics2D.OverlapPoint(hoverPos, interactableFilter, touchingInteractables);
        Transform targetInteractableTransform = null;
        Interactable newHoveredInteractable;
        if (numInteractables > 0)
        {
            targetInteractableTransform = touchingInteractables[0].transform;
            foreach (Collider2D overlappedCol in touchingInteractables)
            {
                //if (collided.gameObject == this.hoveredInteractable?.gameObject)
                //{
                //    return this.hoveredInteractable;
                //}
                if (overlappedCol.transform.position.y < targetInteractableTransform.position.y)
                {
                    targetInteractableTransform = overlappedCol.transform;
                }
            }
            newHoveredInteractable = targetInteractableTransform.GetComponent<Interactable>();
        }else
        {
            newHoveredInteractable = null;
        }

        if (newHoveredInteractable == null || newHoveredInteractable != hoveredInteractable)
        {
            if (this.hoveredInteractable != null) this.OnHoverInteractableExit(hoveredInteractable);
            this.hoveredInteractable = newHoveredInteractable;
            if (hoveredInteractable != null) this.OnHoverInteractableEnter(hoveredInteractable);
        }
        return hoveredInteractable;
    }

    public virtual void OnHoverInteractableEnter(Interactable hoverInteractable) { }
    public virtual void OnHoverInteractableExit(Interactable hoverInteractable) { }

    public virtual void OnCaptured(GrabbableCapturer capturer) { }

    public void DropAt(Vector2 dropPos, Transform interactable, float floorHeight)
    {
        this.isGrabbed = false;
        this.floorHeight = floorHeight;
        this._prevPos = this.transform.position;
        if (this.hoveredInteractable)
        {
            this.OnHoverInteractableExit(this.hoveredInteractable);
            this.hoveredInteractable.ReceiveDropped(this);
            this.hoveredInteractable = null;
        }
        this.OnDrop(interactable);
    }

    virtual protected void OnDrop(Transform interactable) { }

    public void RaiseFloorHere()
    {
        this.floorHeight = this.transform.position.y;
    }
}
