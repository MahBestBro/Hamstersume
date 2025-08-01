using UnityEngine;

public class RacingHamster : MonoBehaviour
{
    Racecourse racecourse;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        racecourse = transform.parent.GetComponent<Racecourse>();
        spriteRenderer = transform.Find("HamsterSprite").GetComponent<SpriteRenderer>();

        transform.position = racecourse.transform.position + racecourse.curveRadius * Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = racecourse.NextPosOnRaceCourse(transform.position, 3.0f);
        spriteRenderer.flipX = racecourse.GetRaceFacing(transform.position) == Facing.Right;
    }


}
