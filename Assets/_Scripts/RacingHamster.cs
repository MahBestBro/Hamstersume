using UnityEngine;

public class RacingHamster : MonoBehaviour
{
    public int laneNumber;

    [HideInInspector]
    public Collider2D collider2D_;

    Racecourse racecourse;
    SpriteRenderer spriteRenderer;
    float distanceTravelled;


    public float RaceCompletion
    {
        get 
        {
            float totalStraightDistance = 2.0f * racecourse.straightLength;
            float curveRadius = racecourse.minCurveRadius + racecourse.laneWidth * ((float)laneNumber - 1.0f);
            float totalCurveDistance = 2.0f * Mathf.PI * curveRadius;
            float trackDistance = totalStraightDistance + totalCurveDistance;

            return distanceTravelled / trackDistance;
        }
    }
    
    [SerializeField]
    float _raceCompletion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        racecourse = transform.parent.parent.GetComponent<Racecourse>();
        spriteRenderer = transform.Find("HamsterSprite").GetComponent<SpriteRenderer>();
        collider2D_ = transform.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (racecourse.RaceIsUnderway())
        {
            float distanceCovered = 0.0f;
            transform.position = racecourse.NextPosOnRaceCourse(
                transform.position, 
                3.0f, 
                laneNumber, 
                ref distanceCovered
            );
            distanceTravelled += distanceCovered; 
        }

        spriteRenderer.flipX = racecourse.GetRaceFacing(transform.position) == Facing.Right;

        _raceCompletion = RaceCompletion;
    }
}
