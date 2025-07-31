using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour
{
    const float MOUSE_CLICK_RAYCAST_DELTA = 0.001f; 

    public HamsterTracker hamsterTracker;
    public HamsterManager hamsterManager;

    [SerializeField]
    LayerMask grabbablesLayermask;

    InputAction pickUp;
    InputAction mousePos;

    Grabbable heldGrabbable = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickUp = InputSystem.actions.FindAction("Pick Up");
        mousePos = InputSystem.actions.FindAction("Mouse Pos");
    }

    // Update is called once per frame
    void Update()
    {
        this.CheckGrabbables();
    }


    void CheckGrabbables()
    {
        // Get Cursor Pos
        Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos.ReadValue<Vector2>());

        if (pickUp.WasPressedThisFrame())
        {
            this.OnTryGrab(mouseWorldPos);
        }

        if (heldGrabbable != null)
        {
            heldGrabbable.DragTo(mouseWorldPos);
            Transform dropInteractable = heldGrabbable.HoverInteractable(mouseWorldPos);

            if (pickUp.WasReleasedThisFrame())
            {
                this.ReleaseGrabbable(mouseWorldPos, dropInteractable);
            }

        }
    }

        void OnTryGrab(Vector2 mouseWorldPos)
    {
        // Fire Raycast at Cursor Pos
        Vector2 lineEnd = mouseWorldPos + MOUSE_CLICK_RAYCAST_DELTA * Vector2.right;
        RaycastHit2D[] hits = Physics2D.LinecastAll(mouseWorldPos, lineEnd, grabbablesLayermask);

        if (hits.Length > 0)
        {
            // Get most forward Grabbable
            Grabbable mostForwardGrabbable = null;
            foreach (RaycastHit2D hit in hits)
            {
                Grabbable currentGrabbable = hit.transform.GetComponent<Grabbable>();
                if (currentGrabbable && (mostForwardGrabbable==null||(currentGrabbable.SortingPriority < mostForwardGrabbable.SortingPriority)))
                {
                    mostForwardGrabbable = currentGrabbable;
                }
            }
            if (mostForwardGrabbable != null)
            {
                // Grab Grabbable
                heldGrabbable = mostForwardGrabbable;
                heldGrabbable.GrabAt(mouseWorldPos);

                Hamster grabbedHamster = heldGrabbable.GetComponent<Hamster>();
                if (grabbedHamster)
                {
                    this.OnGrabbedHamster(grabbedHamster);
                }
            } else
            {
                Debug.LogWarning("No Grabbables Found amongst detected objects");
            }
        }
    }

    void OnGrabbedHamster(Hamster grabbedHamster)
    {
        hamsterTracker.UnmarkExercisingHamster(grabbedHamster);
        grabbedHamster.TryEnterState(HamsterState.Waiting);
    }

    void OnReleasedHamster(Hamster droppedHamster, Transform dropInteractable)
    {
        if (dropInteractable != null && droppedHamster.state != HamsterState.Tired)
        {
            HamsterWheel wheel = dropInteractable.GetComponent<HamsterWheel>();
            if (wheel) {
                int? wheelMapIndex = hamsterTracker.OccupiedHamsterWheelMapIndex(wheel);
                int? originalHamsterInstanceID = hamsterTracker.PlaceHamsterInWheel(droppedHamster, wheel);
                if (originalHamsterInstanceID is int originalHamsterID)
                {
                    GameObject originalHamsterObj = (GameObject)Resources.InstanceIDToObject(
                        originalHamsterID
                    );
                    Hamster originalHamster = originalHamsterObj.GetComponent<Hamster>();
                    Vector2 kickedOutPosition = (Vector2)wheel.transform.position - 2.0f * Vector2.one;
                    originalHamster.transform.position = kickedOutPosition;
                    originalHamster.TryEnterState(HamsterState.Waiting);
                }

                //TODO: This state change is better, maybe make hamster method though?
                droppedHamster.wheel = wheel;
                droppedHamster.EnterState(HamsterState.Exercising);
            }
        }
        else if (droppedHamster.state != HamsterState.Tired)
        {
            droppedHamster.EnterState(HamsterState.Waiting);
        }
    }

    void ReleaseGrabbable(Vector2 releasePos, Transform dropInteractable) 
    {
        float floorHeight = releasePos.y;
        if ( floorHeight > hamsterManager.hamsterWalkArea.max.y )
        {
            floorHeight = Random.Range(hamsterManager.hamsterWalkArea.center.y, hamsterManager.hamsterWalkArea.max.y);
        }
        heldGrabbable.DropAt(releasePos, dropInteractable, floorHeight);
        

        Hamster grabbedHamster = heldGrabbable.GetComponent<Hamster>();
        if (grabbedHamster)
        {
            this.OnReleasedHamster(grabbedHamster, dropInteractable);
        }

        heldGrabbable = null;
    }

}
