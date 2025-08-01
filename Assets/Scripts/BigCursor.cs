using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class BigCursor : MonoBehaviour
{
    [SerializeField]
    Vector2 hotspotOffset;
    [SerializeField]
    Texture2D cursorTexture;
    [SerializeField]
    Texture2D cursorLeftClickTexture;

    Image renderer;
    Vector2 screenBoundaryBuffer = Vector2.one * 100;

    private void Start()
    {
        renderer = GetComponent<Image> ();
        Cursor.visible = false;
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
