using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


struct HamsterWheelIDPair
{
    public int hamsterInstanceID;
    public int wheelInstanceID;

    public HamsterWheelIDPair(int hamsterID, int wheelID)
    {
        hamsterInstanceID = hamsterID;
        wheelInstanceID = wheelID;
    }
}

public class Player : MonoBehaviour
{
    const float MOUSE_CLICK_RAYCAST_DELTA = 0.001f; 

    InputAction pickUp;
    InputAction mousePos;
    
    List<HamsterWheelIDPair> hamsterWheelMap = new List<HamsterWheelIDPair>();

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
            int mask = LayerMask.GetMask("Hamster");
            Vector2 lineEnd = mouseWorldPos + MOUSE_CLICK_RAYCAST_DELTA * Vector2.right;
            RaycastHit2D[] hits = Physics2D.LinecastAll(mouseWorldPos, lineEnd, mask);
            
            if (hits.Length > 0) 
            {
                Transform mostForwardHamster = hits[0].transform;
                foreach (RaycastHit2D hit in hits) 
                {
                    int currentSortingOrder = hit.transform.GetComponent<SpriteRenderer>().sortingOrder;
                    int maxSortingOrder = mostForwardHamster.GetComponent<SpriteRenderer>().sortingOrder;
                    if (currentSortingOrder < maxSortingOrder) 
                    {
                        mostForwardHamster = hit.transform;
                    }
                }

                Hamster newHeldHamster = mostForwardHamster.GetComponent<Hamster>();
                heldHamster = newHeldHamster;
                heldHamsterOffset = (Vector2)newHeldHamster.transform.position - mouseWorldPos; 
                UnmarkHamsterAsExercising(newHeldHamster);
                newHeldHamster.EnterState(HamsterState.PickedUp);
            }
        }

        if (heldHamster is Hamster hamster) 
        {
            hamster.transform.position = (Vector3)(mouseWorldPos + heldHamsterOffset);
        
            ContactFilter2D hamsterWheelFilter = new ContactFilter2D(); 
            hamsterWheelFilter.SetLayerMask(LayerMask.GetMask("HamsterWheel"));
            List<Collider2D> touchingWheels = new List<Collider2D>();
            int numWheels = hamster.collider2D_.Overlap(hamsterWheelFilter, touchingWheels);
            Transform? targetWheel = null;
            //NOTE: This assumes wheels do not overlap, I don't know why they would anyways
            //Debug.Log("Hey");
            if (numWheels > 0) 
            {
                targetWheel = touchingWheels[0].transform; 
            }

            //Vector2 lineEnd = mouseWorldPos + MOUSE_CLICK_RAYCAST_DELTA * Vector2.right;
            //RaycastHit2D wheelHit = Physics2D.Linecast(mouseWorldPos, lineEnd, mask);
            //if (targetWheel is Transform wheel) 
            //{
                //TODO: Hover animation/effect    
            //}

            if (pickUp.WasReleasedThisFrame()) 
            {
                if (targetWheel is Transform wheel)
                {
                    int? wheelMapIndex = OccupiedHamsterWheelMapIndex(wheel);
                    if (wheelMapIndex == null)
                    {
                        hamsterWheelMap.Add(new HamsterWheelIDPair(
                            hamster.gameObject.GetInstanceID(), 
                            wheel.gameObject.GetInstanceID()
                        ));

                        //TODO: This state change is dodgy, incorporate into state code or hamster somehow
                        hamster.transform.position = wheel.transform.position; 
                        hamster.EnterState(HamsterState.Exercising);
                    }
                    else
                    {
                        //TODO: Kick other hamster out
                        hamster.EnterState(HamsterState.Waiting);
                    }
                }
                else
                {
                    hamster.EnterState(HamsterState.Waiting);
                }
                
                heldHamster = null;
            }
        }
    }


    void UnmarkHamsterAsExercising(Hamster hamster)
    {
        int id = hamster.gameObject.GetInstanceID();
        int? hamsterWheelPairToRemoveIndex = null;
        for (int i = 0; i < hamsterWheelMap.Count; i++)
        {
            if (hamsterWheelMap[i].hamsterInstanceID == id)
            {
                hamsterWheelPairToRemoveIndex = i;
                break;
            }
        }

        if (hamsterWheelPairToRemoveIndex is int indexToRemove)
        {
            hamsterWheelMap[indexToRemove] = hamsterWheelMap[hamsterWheelMap.Count - 1];
            hamsterWheelMap.RemoveAt(hamsterWheelMap.Count - 1);
        }
    }

    int? OccupiedHamsterWheelMapIndex(Transform wheel)
    {
        int id = wheel.gameObject.GetInstanceID();
        for (int i = 0; i < hamsterWheelMap.Count; i++)
        {
            if (hamsterWheelMap[i].wheelInstanceID == id)
            {
                return i;
            }
        }

        return null;
    }
}
