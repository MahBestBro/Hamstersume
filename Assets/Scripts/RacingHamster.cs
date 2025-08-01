using UnityEngine;

public class RacingHamster : MonoBehaviour
{
    public int laneNumber;

    Racecourse racecourse;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        racecourse = transform.parent.GetComponent<Racecourse>();
        spriteRenderer = transform.Find("HamsterSprite").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (racecourse.RaceIsUnderway())
        {
            transform.position = racecourse.NextPosOnRaceCourse(transform.position, 3.0f, laneNumber);
        }
        spriteRenderer.flipX = racecourse.GetRaceFacing(transform.position) == Facing.Right;
    }


}
