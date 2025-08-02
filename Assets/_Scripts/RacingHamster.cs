using UnityEngine;

public class RacingHamster : MonoBehaviour
{
    public HamsterProfile hamsterProfile;


    public int laneNumber;

    [HideInInspector]
    public Collider2D collider2D_;

    public Racecourse racecourse;
    [SerializeField]
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
        collider2D_ = transform.GetComponent<Collider2D>();
    }

    public void InitialiseSelf(Racecourse _racecourse)
    {
        this.racecourse = _racecourse;
        if (spriteRenderer == null) spriteRenderer = transform.Find("HamsterSprite").GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = hamsterProfile.hVariant.hamsterIdle;
    }

    // Update is called once per frame
    void Update()
    {
        if (racecourse == null) return;

        if (racecourse.RaceIsUnderway())
        {
            this.spriteRenderer.sprite = hamsterProfile.hVariant.hamsterRunning;
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
