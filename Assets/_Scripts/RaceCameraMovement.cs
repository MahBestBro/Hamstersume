using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceCameraMovement : MonoBehaviour
{
    [Range(0.0f, 5.0f)]
    public float transitionTimeSecs;
    [Range(1.0f, 20.0f)]
    public float minCameraSize;
    [Range(1.0f, 20.0f)]
    public float maxCameraSize;
    [Range(0.0f, 10.0f)]
    public float victoryCameraSize;
    [Range(0.0f, 10.0f)]
    public float victoryZoomDurationSecs;
    
    public Racecourse racecourse;

    Vector2 initialCameraPosition;
    float initialCameraSize;
    float elapsedTime = 0.0f;

    Vector2 velocity = Vector2.zero; 
    bool focusingOnWinner = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialCameraSize = Camera.main.orthographicSize;
        initialCameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (racecourse.Winner != null)
        {
            if (!focusingOnWinner)
            {
                initialCameraPosition = transform.position;
                initialCameraSize = Camera.main.orthographicSize;
                elapsedTime = 0.0f;
                focusingOnWinner = true;
            }

            float t = Mathf.Min(elapsedTime / victoryZoomDurationSecs, 1.0f);
            
            Vector2 targetPos = racecourse.Winner.transform.position;
            Vector2 newPosition = Vector2.Lerp(initialCameraPosition, targetPos, t);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

            Camera.main.orthographicSize = Mathf.Lerp(initialCameraSize, victoryCameraSize, t);

            if (elapsedTime <= victoryZoomDurationSecs)
            {
                elapsedTime += Time.deltaTime;
            }
        }
        else
        {
            Vector2 targetPos = Vector2.zero;
            float[] weights = racecourse.RaceCompletions().Select(x => x + 1.0f).ToArray();
            Vector2 totalWeightedHamsterPos = Vector2.zero;
            for (int i = 0; i < racecourse.hamsters.Length; i++) 
            {
                totalWeightedHamsterPos += weights[i] * (Vector2)racecourse.hamsters[i].transform.position;
            }
            targetPos = totalWeightedHamsterPos / weights.Sum();

            float largestDistance = -1.0f;
            for (int i = 0; i < racecourse.hamsters.Length; i++) 
            {
                for (int j = i; j < racecourse.hamsters.Length; j++)
                {
                    Vector2 posA = racecourse.hamsters[i].transform.position;
                    Vector2 posB = racecourse.hamsters[j].transform.position;

                    largestDistance = Mathf.Max(Vector2.Distance(posA, posB), largestDistance);
                } 
            }

            //TODO: Adjust zoom dynamically
            Camera.main.orthographicSize = Mathf.Clamp(largestDistance, minCameraSize, maxCameraSize);

            Vector2 newPosition = Vector2.SmoothDamp(
                (Vector2)transform.position, 
                targetPos,
                ref velocity,
                transitionTimeSecs
            );
            
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
        
    }
}
