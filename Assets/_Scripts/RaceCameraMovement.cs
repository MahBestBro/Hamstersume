using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RaceCameraMovement : MonoBehaviour
{
    public float cameraZoomScale = 1.5F;
    [Range(0.0f, 5.0f)]
    public float posTransitionTimeSecs;
    [Range(0.0f, 5.0f)]
    public float zoomTransitionTimeSecs;
    [Range(1.0f, 20.0f)]
    public float minCameraSize;
    [Range(1.0f, 20.0f)]
    public float maxCameraSize;
    [Range(0.0f, 10.0f)]
    public float victoryCameraSize;
    [Range(0.0f, 10.0f)]
    public float victoryZoomDurationSecs;
    
    public Racecourse racecourse;
    public UnityEvent onWinTransitionEnd;

    Vector2 initialCameraPosition;
    float initialCameraSize;
    float elapsedTime = 0.0f;

    Vector2 velocity = Vector2.zero;
    float zoomVelocity;
    bool focusingOnWinner = false;
    bool triggerWinTransitionEnd = true;


    void Start()
    {
        // Initialise Camera
        initialCameraSize = Camera.main.orthographicSize;
        initialCameraPosition = transform.position;
    }

    void Update()
    {
        if (racecourse.IsInitialised){
            if (racecourse.PlayerWinner != null)
            {
                this.FocusOnPlayerWinner();
            }
            else
            {
                this.FollowAllHamsters();
            }
        }
    }

    void FollowAllHamsters()
    {
        float numHamsters = racecourse.hamsters.Length;
        Vector2 targetPos = Vector2.zero;
        float[] weights = racecourse.RaceCompletions().Select(x => x + 1.0f).ToArray();
        Vector2 totalWeightedHamsterPos = Vector2.zero;
        for (int i = 0; i < numHamsters; i++)
        {
            totalWeightedHamsterPos += weights[i] * (Vector2)racecourse.hamsters[i].transform.position;
        }
        targetPos = totalWeightedHamsterPos / weights.Sum();

        float largestOrthographicDistance = -1.0f;
        bool largestDistIsHorizontal = false;
        float aspect = Camera.main.aspect;
        for (int i = 0; i < racecourse.hamsters.Length; i++)
        {
            Vector2 posA = racecourse.hamsters[i].transform.position;
            Vector2 posB = Camera.main.transform.position;
            float horDist = Mathf.Abs(posA.x - posB.x);
            float vertDist = Mathf.Abs(posA.y - posB.y);
            float maxOrthoDist = Mathf.Max(horDist / aspect, vertDist);
            if (maxOrthoDist > largestOrthographicDistance)
            {
                largestOrthographicDistance = maxOrthoDist;
                largestDistIsHorizontal = vertDist > horDist;
            }
        }

        float newCameraSizeHeight = largestOrthographicDistance * cameraZoomScale;
        newCameraSizeHeight = Mathf.Clamp(newCameraSizeHeight, minCameraSize, maxCameraSize);

        //float zoomDiff = newCameraSizeHeight - Camera.main.orthographicSize;
        //zoomVelocity += zoomDiff / 10F;
        //if (zoomVelocity > zoomDiff) { zoomVelocity = zoomDiff; }
        //Camera.main.orthographicSize += zoomVelocity * Time.deltaTime;

        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, newCameraSizeHeight, ref zoomVelocity, zoomTransitionTimeSecs);
        //TODO: Adjust zoom dynamically



        Vector2 newPosition = Vector2.SmoothDamp(
            (Vector2)transform.position,
            targetPos,
            ref velocity,
            posTransitionTimeSecs
        );

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    void FocusOnPlayerWinner()
    {
        if (!focusingOnWinner)
        {
            initialCameraPosition = transform.position;
            initialCameraSize = Camera.main.orthographicSize;
            elapsedTime = 0.0f;
            focusingOnWinner = true;
        }

        float t = Mathf.Min(elapsedTime / victoryZoomDurationSecs, 1.0f);

        Vector2 targetPos = racecourse.PlayerWinner.transform.position;
        Vector2 newPosition = Vector2.Lerp(initialCameraPosition, targetPos, t);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        Camera.main.orthographicSize = Mathf.Lerp(initialCameraSize, victoryCameraSize, t);

        if (elapsedTime <= victoryZoomDurationSecs)
        {
            elapsedTime += Time.deltaTime;
        }
        else if (triggerWinTransitionEnd)
        {
            onWinTransitionEnd?.Invoke();
            triggerWinTransitionEnd = false;
        }
    }
}
