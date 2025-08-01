using UnityEngine;


public enum Facing
{
    Left,
    Right
}

public class Racecourse : MonoBehaviour
{
    [Range(0.0f, 50.0f)]
    public float curveRadius;
    [Range(0.0f, 50.0f)]
    public float straightLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 centre = (Vector2)transform.position;

        Vector2 bottomStraightStart = centre - new Vector2(straightLength / 2.0f, curveRadius);
        Vector2 bottomStraightEnd = bottomStraightStart + straightLength * Vector2.right;
        Gizmos.DrawLine(bottomStraightStart, bottomStraightEnd);
        
        Vector2 topStraightStart = centre + new Vector2(-straightLength / 2.0f, curveRadius);
        Vector2 topStraightEnd = topStraightStart + straightLength * Vector2.right;
        Gizmos.DrawLine(topStraightStart, topStraightEnd);


        Vector2 rightCurveCentre = centre + straightLength / 2.0f * Vector2.right;
        Gizmos.DrawWireSphere(rightCurveCentre, curveRadius);

        Vector2 leftCurveCentre = centre + straightLength / 2.0f * Vector2.left;
        Gizmos.DrawWireSphere(leftCurveCentre, curveRadius);
    }


    //NOTE: Mathf.Approximately doesn't give you control over epsilon grrrrr
    bool WithinEpsilon(float a, float b, float epsilon)
    {
        return Mathf.Abs(a - b) <= epsilon;
    }

    //NOTE: This assumes the race goes anticlockwise around the course
    public Vector2 NextPosOnRaceCourse(Vector2 pos, float speed)
    {
        Vector2 nextPos = pos;
        Vector2 racetrackCentre = (Vector2)transform.position;

        float distanceToTravel = speed * Time.deltaTime;

        float topStraightY = racetrackCentre.y + curveRadius; 
        float bottomStraightY = racetrackCentre.y - curveRadius; 
        float straightLeftSide = racetrackCentre.x - straightLength / 2.0f;
        float straightRightSide = racetrackCentre.x + straightLength / 2.0f;

        bool inStraight = nextPos.x >= straightLeftSide && nextPos.x <= straightRightSide;
        
        //We are in the bottom straight
        if (distanceToTravel > 0.0f && inStraight && WithinEpsilon(nextPos.y, bottomStraightY, 0.01f))
        {
            float remainingDistance = straightRightSide - nextPos.x; 
            float distanceTravelled = Mathf.Min(distanceToTravel, remainingDistance);
            
            nextPos.x += distanceTravelled;
            distanceToTravel -= distanceTravelled;
        }

        //We are in the right curve
        if (distanceToTravel > 0.0f && nextPos.x >= straightRightSide)
        {
            Vector2 curveCentre = new Vector2(straightRightSide, racetrackCentre.y);
            Vector2 startRadial = Vector2.down;
            Vector2 endRadial = Vector2.up;
            Vector2 toPosRadial = (nextPos - curveCentre).normalized;
            
            float angularSpeed = speed / curveRadius; 
            float angleToTravel = Mathf.Rad2Deg * angularSpeed * Time.deltaTime;
            float remainingAngle = Vector2.Angle(endRadial, toPosRadial) * curveRadius;
            float angleTravelled = Mathf.Min(angleToTravel, remainingAngle);

            float currentAngle = Vector2.Angle(startRadial, toPosRadial);
            float angleOffset = Vector2.SignedAngle(Vector2.right, startRadial); 
            float nextPosAngleRad = Mathf.Deg2Rad * (currentAngle + angleTravelled + angleOffset);
            
            nextPos = curveCentre + curveRadius * (new Vector2(Mathf.Cos(nextPosAngleRad), Mathf.Sin(nextPosAngleRad)));
            distanceToTravel -= angleTravelled / 360.0f * curveRadius;
        }

        //We are in the top straight
        if (distanceToTravel > 0.0f && inStraight && WithinEpsilon(nextPos.y, topStraightY, 0.01f))
        {
            float remainingDistance = nextPos.x - straightLeftSide; 
            float distanceTravelled = Mathf.Min(distanceToTravel, remainingDistance);
            
            nextPos.x -= distanceTravelled;
            distanceToTravel -= distanceTravelled;
        }

        //We are in the left curve
        if (distanceToTravel > 0.0f && nextPos.x <= straightLeftSide)
        {
            Vector2 curveCentre = new Vector2(straightLeftSide, racetrackCentre.y);
            Vector2 startRadial = Vector2.up;
            Vector2 endRadial = Vector2.down;
            Vector2 toPosRadial = (nextPos - curveCentre).normalized;
            
            float angularSpeed = speed / curveRadius; 
            float angleToTravel = Mathf.Rad2Deg * angularSpeed * Time.deltaTime;
            float remainingAngle = Vector2.Angle(endRadial, toPosRadial) * curveRadius;
            float angleTravelled = Mathf.Min(angleToTravel, remainingAngle);
            
            float currentAngle = Vector2.Angle(startRadial, toPosRadial);
            float angleOffset = Vector2.SignedAngle(Vector2.right, startRadial); 
            float nextPosAngle = Mathf.Deg2Rad * (currentAngle + angleTravelled + angleOffset);
            
            nextPos = curveCentre + curveRadius * (new Vector2(Mathf.Cos(nextPosAngle), Mathf.Sin(nextPosAngle)));
            distanceToTravel -= angleTravelled / 360.0f * curveRadius;
        }

        return nextPos;
    } 

    public Facing GetRaceFacing(Vector2 pos)
    {
        //If in the bottom half of the race course, face right, else face left
        return (pos.y <= transform.position.y) ? Facing.Right : Facing.Left;
    }
}
