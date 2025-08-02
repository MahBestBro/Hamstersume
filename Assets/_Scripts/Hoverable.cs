using UnityEngine;
using UnityEngine.Events;

public class Hoverable : MonoBehaviour
{
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    public bool isHovered = false;
    public bool isHoverable = true;

    [SerializeField]
    public SpriteRenderer spriteRenderer;

    protected Transform sortingAnchor;
    public float SortingPriority { 
        get
        {
            return sortingAnchor.position.y;
        }
    }


    private void Start()
    {
        this.AnchorSortingToSprite();
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
        int screenY = (int)Camera.main.WorldToScreenPoint(this.sortingAnchor?.position ?? this.transform.position).y;
        sortOrder = Screen.height - screenY;
        
        this.spriteRenderer.sortingOrder = sortOrder;
        return sortOrder;
    }

    
    public void OnHoverEnter()
    {
        this.isHovered = true;
        onHoverEnter?.Invoke();
    }
    
    public void OnHoverExit()
    {
        this.isHovered = false;
        onHoverExit?.Invoke();
    }

}
