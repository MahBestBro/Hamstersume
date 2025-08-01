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

    RacingHamster[] hamsters;
    int winnerIndex = -1;
    Vector2 initialCameraPosition;
    float initialCameraSize;
    float elapsedTime = 0.0f;

    Vector2 velocity = Vector2.zero; 
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform hamsterParent = racecourse.transform.Find("Hamsters");
        hamsters = new RacingHamster[hamsterParent.childCount];
        for (int i = 0; i < hamsterParent.childCount; i++)
        {
            hamsters[i] = hamsterParent.GetChild(i).GetComponent<RacingHamster>();
        }

        initialCameraSize = Camera.main.orthographicSize;
        initialCameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float[] trackCompletions = hamsters.Select(h => h.RaceCompletion).ToArray();
        for (int i = 0; i < hamsters.Length && winnerIndex < 0; i++)
        {
            RacingHamster hamster = hamsters[i];

            //TODO: This track completion boolean is dodgy. Ideally, track completions would properly
            //track total distance but for some reason it does not. Either fix track completion or
            //clean 
            ContactFilter2D finishLineFilter = new ContactFilter2D();
            finishLineFilter.useTriggers = true;
            finishLineFilter.useLayerMask = true;
            finishLineFilter.SetLayerMask(LayerMask.GetMask("FinishLine"));
            List<Collider2D> _ = new List<Collider2D>();
            bool intersectedFinishLine = hamster.collider2D_.Overlap(finishLineFilter, _) > 0;
            if (trackCompletions[i] >= 0.2f && intersectedFinishLine)
            {
                winnerIndex = i;
                OnRaceEnd();
                break;
            }
        }

        
        if (winnerIndex >= 0)
        {
            float t = Mathf.Min(elapsedTime / victoryZoomDurationSecs, 1.0f);
            
            Vector2 targetPos = hamsters[winnerIndex].transform.position;
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
            float[] weights = trackCompletions.Select(x => x + 1.0f).ToArray();
            Vector2 totalWeightedHamsterPos = Vector2.zero;
            for (int i = 0; i < hamsters.Length; i++) 
            {
                totalWeightedHamsterPos += weights[i] * (Vector2)hamsters[i].transform.position;
            }
            targetPos = totalWeightedHamsterPos / weights.Sum();

            float largestDistance = -1.0f;
            for (int i = 0; i < hamsters.Length; i++) 
            {
                for (int j = i; j < hamsters.Length; j++)
                {
                    Vector2 posA = hamsters[i].transform.position;
                    Vector2 posB = hamsters[j].transform.position;

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


    void OnRaceEnd()
    {
        initialCameraPosition = transform.position;
        initialCameraSize = Camera.main.orthographicSize;
    }
}
