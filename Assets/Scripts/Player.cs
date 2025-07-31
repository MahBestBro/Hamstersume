using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputAction pickUp;
    InputAction mousePos;
    
    Hamster? heldHamster = null;
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

        if (pickUp.WasPressedThisFrame()) 
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(mouseWorldPos, mouseWorldPos + 0.001f * Vector2.right);
            
            //NOTE: We consider hamsters that are further down the y-axis to be in front
            if (hits.Length > 0) 
            {
                Transform mostForwardHamster = hits[0].transform;
                foreach (RaycastHit2D hit in hits) {
                    if (hit.transform.position.y < mostForwardHamster.position.y) {
                        mostForwardHamster = hit.transform;
                    }
                }

                heldHamster = mostForwardHamster.GetComponent<Hamster>();
                heldHamsterOffset = (Vector2)heldHamster.transform.position - mouseWorldPos; 
                heldHamster.EnterState(HamsterState.PickedUp);
            }
        }

        if (heldHamster is Hamster hamster) 
        {
            hamster.transform.position = (Vector3)(mouseWorldPos + heldHamsterOffset);
        
            if (pickUp.WasReleasedThisFrame()) 
            {
                hamster.EnterState(HamsterState.Waiting);
                heldHamster = null;
            }
        }
    }
}
