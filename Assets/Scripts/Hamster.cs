using UnityEngine;

public enum HamsterState 
{
    Waiting,
    Walking,
    PickedUp
}

public class Hamster : MonoBehaviour
{
    [HideInInspector]
    public HamsterState state;


    [HideInInspector]
    public float minIdleTimeSecs;
    [HideInInspector]
    public float maxIdleTimeSecs;
    [HideInInspector]
    public float walkSpeed; 
    [HideInInspector]
    public Bounds walkArea;
    //TODO: Walk min and max "radius"

    SpriteRenderer spriteRenderer;

    float idleElapsedTime = 0.0f;
    float idleDuration;
    Vector2 walkDestination = Vector2.zero;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnterState(HamsterState.Waiting);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int screenY = (int)Camera.main.WorldToScreenPoint(transform.position).y; 
        spriteRenderer.sortingOrder = Screen.height - screenY;

        HandleCurrentState(state);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere((Vector3)walkDestination, 0.25f);
    }


    void HandleCurrentState(HamsterState state)
    {
        switch (state)
        {
            case HamsterState.Waiting:
                idleElapsedTime += Time.deltaTime;
                if (idleElapsedTime >= idleDuration) 
                {
                    EnterState(HamsterState.Walking);
                }
                break;

            case HamsterState.Walking:
                Vector2 toTravel = walkDestination - (Vector2)transform.position; 
                if (toTravel.magnitude >= walkSpeed * Time.deltaTime)
                {
                    Vector2 direction = toTravel.normalized;
                    transform.position += (Vector3)(walkSpeed * direction * Time.deltaTime);
                }
                else 
                {
                    transform.position = walkDestination;
                    EnterState(HamsterState.Waiting);
                }
                break;

            case HamsterState.PickedUp: break;
        }
    }

    public void EnterState(HamsterState newState) 
    {
        switch (newState)
        {
            case HamsterState.Waiting:
                idleElapsedTime = 0.0f;
                idleDuration = Random.Range(minIdleTimeSecs, maxIdleTimeSecs);
                break;

            case HamsterState.Walking: 
                walkDestination = new Vector2(
                    Random.Range(walkArea.min.x, walkArea.max.x), 
                    Random.Range(walkArea.min.x, walkArea.max.x)
                );
                break;

            case HamsterState.PickedUp: break;
        }

        state = newState;
    }

    

    //void ExitCurrentState() 
    //{
    //    switch (state)
    //    {
    //        case HamsterState.Waiting: break;
//
    //        case HamsterState.Walking: break;
//
    //        case HamsterState.PickedUp: break;
    //    }
    //}
}
