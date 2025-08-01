using UnityEngine;


public enum Facing
{
    Left,
    Right
}

public class Racecourse : MonoBehaviour
{
    [Range(0.0f, 50.0f)]
    public float minCurveRadius;
    [Range(0.0f, 50.0f)]
    public float straightLength;
    [Range(0.0f, 50.0f)]
    public float laneWidth;
    [Range(1, 5)]
    public int numLanes;
    [Range(0.0f, 10.0f)]
    public float countdownTimeSecs;
    
    [HideInInspector]
    public Collider2D finishLineCollider;

    float elapsedTime;


    //NOTE: Mathf.Approximately doesn't give you control over epsilon grrrrr
    bool WithinEpsilon(float a, float b, float epsilon)
    {
        return Mathf.Abs(a - b) <= epsilon;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform hamsterParent = transform.Find("Hamsters");
        for (int i = 0; i < hamsterParent.childCount; i++)
        {
            RacingHamster racer = hamsterParent.GetChild(i).GetComponent<RacingHamster>();
            racer.laneNumber = i + 1;
            racer.transform.position = StartingPosition(racer.laneNumber);
        }

        Transform finishLineTransform = transform.Find("FinishLine");
        float finishLineCentreY = minCurveRadius + laneWidth * numLanes / 2.0f;
        finishLineTransform.position = transform.position + finishLineCentreY * Vector3.down; 
        Vector3 newScale = finishLineTransform.localScale;
        newScale.y = laneWidth * numLanes;
        finishLineTransform.localScale = newScale;

        finishLineCollider = finishLineTransform.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!RaceIsUnderway())
        {
            elapsedTime += Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 centre = (Vector2)transform.position;

        for (int laneNumber = 0; laneNumber <= numLanes; laneNumber++)
        {
            float curveRadius = minCurveRadius + (float)laneNumber * laneWidth;

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
    }


    public bool RaceIsUnderway()
    {
        return elapsedTime >= countdownTimeSecs;
    }

    //NOTE: This assumes the race goes anticlockwise around the course
    public Vector2 NextPosOnRaceCourse(Vector2 pos, float speed, int laneNumber, ref float distanceCovered)
    {
        Vector2 nextPos = pos;
        Vector2 racetrackCentre = (Vector2)transform.position;
        float intialDistance = speed * Time.deltaTime;
        float distanceToTravel = intialDistance;

        float curveRadius = minCurveRadius + ((float)laneNumber - 1.0f) * laneWidth;
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
            distanceToTravel -= Mathf.Deg2Rad * angleTravelled * curveRadius;
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

        distanceCovered = intialDistance - distanceToTravel;
        return nextPos;
    } 

    public Facing GetRaceFacing(Vector2 pos)
    {
        //If in the bottom half of the race course, face right, else face left
        return (pos.y <= transform.position.y) ? Facing.Right : Facing.Left;
    }


    //NOTE: Hamsters start at the middle of the bottom straight. This function also assumes hamsters will only
    //go around one curve. Any more is likely absurd anyways.
    Vector2 StartingPosition(int laneNumber)
    {
        float laneRank = (float)laneNumber - 1.0f;
        float curveRadius = minCurveRadius + laneRank * laneWidth;   
        float distanceAdjustment = 2.0f * Mathf.PI * laneRank * laneWidth;
        float straightDistanceAdjustment = Mathf.Min(distanceAdjustment, straightLength / 2.0f);
        float curveDistanceAdjustment = distanceAdjustment - straightDistanceAdjustment;
        
        Vector2 racetrackCentre = (Vector2)transform.position;
        Vector2 pos = racetrackCentre + new Vector2(
            straightDistanceAdjustment,
            -(minCurveRadius + laneWidth * laneRank)
        );

        if (!WithinEpsilon(curveDistanceAdjustment, 0.0f, 0.01f))
        {
            float curveAdjustmentAngle = Mathf.Rad2Deg * curveDistanceAdjustment / curveRadius;
            float angleOffset = Vector2.SignedAngle(Vector2.right, Vector2.down); 
            float angleRadians = Mathf.Deg2Rad * (curveAdjustmentAngle + angleOffset);
            Vector2 adjustmentRadial = curveRadius * (new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)));
            
            Vector2 curveCentre = racetrackCentre + straightLength / 2.0f * Vector2.right;
            pos = curveCentre + adjustmentRadial;
        }

        return pos;    
    }
}
