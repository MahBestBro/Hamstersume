using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class BigCursor : MonoBehaviour
{
    [SerializeField]
    Vector2 hotspotOffset;
    [SerializeField]
    Sprite cursorTexture;
    [SerializeField]
    Sprite cursorLeftClickTexture;

    Image renderer;
    Vector2 screenBoundaryBuffer = Vector2.one * 100;

    private void Start()
    {
        renderer = GetComponent<Image> ();
        Cursor.visible = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3)hotspotOffset, 8.0f);
    }


    public void Goto(Vector2 cursorPos)
    {
        if (this.CheckInScreen(cursorPos))
        {
            this.transform.position = cursorPos + hotspotOffset;
            Cursor.visible = false;
        } else
        {
            Cursor.visible = true;
        }
        
    }


    public void OnLeftClickDown()
    {
        this.renderer.sprite = cursorLeftClickTexture;
    }

    public void OnLeftClickUp()
    {
        this.renderer.sprite = cursorTexture;
    }

    public bool CheckInScreen(Vector2 cursorPos)
    {
        if (Application.isFocused && (cursorPos.x <= Screen.width+screenBoundaryBuffer.x && cursorPos.x >= -screenBoundaryBuffer.x) && (cursorPos.y <= Screen.height+screenBoundaryBuffer.y && cursorPos.y  >= -screenBoundaryBuffer.y))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
