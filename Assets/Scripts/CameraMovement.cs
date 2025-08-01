using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 microwaveScreenWorldCentre;
    public Vector2 wheelScreenWorldCentre;
    [Range(0.0f, 5.0f)]
    public float transitionTimeSecs;

    bool moving = false;
    Vector2 velocity = Vector2.zero;
    Vector2 targetPos; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = wheelScreenWorldCentre;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Vector2 newPosition = Vector2.SmoothDamp(
                (Vector2)transform.position, 
                targetPos,
                ref velocity,
                transitionTimeSecs
            );
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }

        if (!moving || Vector2.Distance((Vector2)transform.position, targetPos) < 0.001f)
        {
            transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
            StopMoving();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(microwaveScreenWorldCentre, 0.1f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(wheelScreenWorldCentre, 0.1f);
    }


    void StopMoving()
    {
        moving = false;
        velocity = Vector2.zero;
    }

    public void StartMovingCameraToWheelScreenPos()
    {
        StopMoving();
        targetPos = wheelScreenWorldCentre;
        moving = true;
    }

    public void StartMovingCameraToMicrowaveScreenPos()
    {
        StopMoving();
        targetPos = microwaveScreenWorldCentre;
        moving = true;
    }
}
