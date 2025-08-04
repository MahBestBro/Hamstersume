using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public enum Facing
{
    Left,
    Right
}

public class Racecourse : MonoBehaviour
{
    public RacingHamster racingHamsterPrefab;

    [Range(0.0f, 50.0f)]
    public float minCurveRadius;
    [Range(0.0f, 50.0f)]
    public float straightLength;
    [Range(0.0f, 50.0f)]
    public float laneWidth;
    [Range(1, 10)]
    public int numLanes;
    [Range(0.0f, 10.0f)]
    public float countdownTimeSecs;
    
    public RacingHamster[] hamsters;
    public int playerHamstersOffset = 0;
    public RacingHamster PlayerWinner
    {
        get
        {
            return playerWinner;
        }
    }

    [SerializeField]
    ScreenTransition startTransition;
    [SerializeField]
    ScreenTransition endTransition;

    RacingHamster playerWinner = null;
    Collider2D finishLineCollider;

    float raceElapsedTime = 0.0f;

    private bool isInitialised = false;
    public bool IsInitialised { get { return this.isInitialised; } }


    public RacingHamster[] SpawnRacingHamsters(RacingEventData raceData)
    {
        RacingHamster[] spawnedHamsters = new RacingHamster[raceData.TotalParticipants];
        int idx = 0;
        foreach (HamsterProfile npcRacer in raceData.npcParticipants)
        {
            GameObject spawnedHam = Instantiate(racingHamsterPrefab.gameObject);
            spawnedHamsters[idx] = spawnedHam.GetComponent<RacingHamster>();
            spawnedHamsters[idx].hamsterProfile = npcRacer;
            spawnedHamsters[idx].playerIndicator?.gameObject.SetActive(false);
            idx++;
        }

        playerHamstersOffset = idx;
        foreach (HamsterProfile playerRacer in raceData.playerParticipants)
        {
            GameObject spawnedHam = Instantiate(racingHamsterPrefab.gameObject);
            spawnedHamsters[idx] = spawnedHam.GetComponent<RacingHamster>();
            spawnedHamsters[idx].hamsterProfile = playerRacer;
            spawnedHamsters[idx].playerIndicator?.gameObject.SetActive(true);
            idx++;
        }
        
        return spawnedHamsters;
    }

    public RacingHamster[] GetDummyHamsters()
    {
        Transform hamsterParent = transform.Find("Hamsters");
        RacingHamster[] dummyHamsters = new RacingHamster[hamsterParent.childCount];
        for (int i = 0; i < hamsterParent.childCount; i++)
        {
            RacingHamster hamster = hamsterParent.GetChild(i).GetComponent<RacingHamster>();
            dummyHamsters[i] = hamster;
            hamster.laneNumber = i + 1;
            hamster.transform.position = StartingPosition(hamster.laneNumber);
        }
        return dummyHamsters;
    }

    public int[] GenerateShuffledIndexes(int n)
    {
        int[] indexes = new int[n];
        bool[] selectedIndexes = new bool[n];
        for (int i = 0; i < n; i++)
        {
            indexes[i] = UnityEngine.Random.Range(0, n);
            while(selectedIndexes[indexes[i]])
            {
                indexes[i] = (indexes[i] + 1) % n;
            }
            selectedIndexes[indexes[i]] = true;
        }
        return indexes;
    }

    public void InitialiseRacecourse(RacingEventData raceData)
    {
        this.hamsters = (raceData!=null) ? this.SpawnRacingHamsters(raceData) : this.GetDummyHamsters();
        int[] orderedLanes = GenerateShuffledIndexes(this.hamsters.Length);
        for (int i = 0; i < hamsters.Length; i++)
        {
            hamsters[i].InitialiseSelf(this);
            hamsters[i].laneNumber = orderedLanes[i] + 1;
            hamsters[i].transform.position = StartingPosition(hamsters[i].laneNumber);
        }

        Transform finishLineTransform = transform.Find("FinishLine");
        float finishLineCentreY = minCurveRadius + laneWidth * numLanes / 2.0f;
        finishLineTransform.position = transform.position + finishLineCentreY * Vector3.down;
        Vector3 newScale = finishLineTransform.localScale;
        newScale.y = laneWidth * numLanes;
        finishLineTransform.localScale = newScale;

        finishLineCollider = finishLineTransform.GetComponent<Collider2D>();

        isInitialised = true;
    }

    public void StartRace()
    {
        if (!isInitialised)
        {
            Debug.LogWarning("Starting uninitialised Race!");
        }
        startTransition.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialised) return;

        bool racing = RaceIsUnderway();
        if (!startTransition.IsPlaying && !racing)
        {
            raceElapsedTime += Time.deltaTime;
        }
        
        //Check who's the winner out of the player's hamsters
        float[] raceCompletions = RaceCompletions();
        for (int i = playerHamstersOffset; i < hamsters.Length && playerWinner == null; i++)
        {
            RacingHamster hamster = hamsters[i];

            ContactFilter2D finishLineFilter = new ContactFilter2D();
            finishLineFilter.useTriggers = true;
            finishLineFilter.useLayerMask = true;
            finishLineFilter.SetLayerMask(LayerMask.GetMask("FinishLine"));
            List<Collider2D> _ = new List<Collider2D>();
            bool intersectedFinishLine = hamster.collider2D_.Overlap(finishLineFilter, _) > 0;
            
            //TODO: This track completion boolean is dodgy. Ideally, track completions would properly
            //track total distance but for some reason it does not. Either fix track completion or
            //clean 
            if (hamster.RaceCompletion >= 0.2f && intersectedFinishLine)
            {
                playerWinner = hamster;
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 centre = (Vector2)transform.position;

        for (int laneNumber = 0; laneNumber < numLanes; laneNumber++)
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
        return raceElapsedTime >= countdownTimeSecs;
    }

    public float[] RaceCompletions()
    {
        return hamsters.Select(h => h.RaceCompletion).ToArray();
    }

    //NOTE: This assumes the race goes anticlockwise around the course
    public Vector2 NextPosOnRaceCourse(Vector2 pos, float speed, int laneNumber, float deltaTime, ref float distanceCovered, bool coverFixedDistance = false)
    {
        Vector2 nextPos = pos;
        Vector2 racetrackCentre = (Vector2)transform.position;
        distanceCovered = 0;
        float timeRemaining = deltaTime;
        float distanceToTravel;
        float distanceTravelled;

        float curveRadius = minCurveRadius + ((float)laneNumber - 1.0f) * laneWidth;
        float curveCircumference = Mathf.PI * 2F * curveRadius;
        float topStraightY = racetrackCentre.y + curveRadius; 
        float bottomStraightY = racetrackCentre.y - curveRadius; 
        float straightLeftSide = racetrackCentre.x - straightLength / 2.0f;
        float straightRightSide = racetrackCentre.x + straightLength / 2.0f;

        bool inStraight = nextPos.x >= straightLeftSide && nextPos.x <= straightRightSide;

        int _currentTrackSection = 0;

        //We are in the bottom straight
        bool inBottomStraight = inStraight && MathHelpers.WithinEpsilon(nextPos.y, bottomStraightY, 0.01f);
        if (timeRemaining > 0.0f && inBottomStraight)
        {
            distanceToTravel = speed * timeRemaining;
            float remainingDistance = straightRightSide - nextPos.x;
            distanceTravelled = Mathf.Min(distanceToTravel, remainingDistance);
            nextPos.x += distanceTravelled;
            timeRemaining -= distanceTravelled / speed;
            distanceCovered += distanceTravelled;

            _currentTrackSection = 1;
        }

        //We are in the right curve
        if (timeRemaining > 0.0f && nextPos.x >= straightRightSide)
        {
            Vector2 curveCentre = new Vector2(straightRightSide, racetrackCentre.y);
            Vector2 startRadial = Vector2.down;
            Vector2 endRadial = Vector2.up;
            Vector2 toPosRadial = (nextPos - curveCentre).normalized;

            float angularSpeed = speed / curveRadius; 
            float angleToTravel = Mathf.Rad2Deg * angularSpeed * timeRemaining;
            float remainingAngle = Vector2.Angle(endRadial, toPosRadial);
            float angleTravelled = Mathf.Min(angleToTravel, remainingAngle);

            float currentAngle = Vector2.Angle(startRadial, toPosRadial);
            float angleOffset = Vector2.SignedAngle(Vector2.right, startRadial); 
            float nextPosAngleRad = Mathf.Deg2Rad * (currentAngle + angleTravelled + angleOffset);
            
            nextPos = curveCentre + curveRadius * (new Vector2(Mathf.Cos(nextPosAngleRad), Mathf.Sin(nextPosAngleRad)));
            distanceTravelled = (angleTravelled/360F) * curveCircumference;
            timeRemaining -= distanceTravelled / speed;
            distanceCovered += distanceTravelled;

            _currentTrackSection = 2;
        }
        
        //We are in the top straight
        bool inTopStraight = inStraight && MathHelpers.WithinEpsilon(nextPos.y, topStraightY, 0.01f);
        if (timeRemaining > 0.0f && inTopStraight)
        {
            distanceToTravel = speed * timeRemaining;
            float remainingDistance = nextPos.x - straightLeftSide;
            distanceTravelled = Mathf.Min(distanceToTravel, remainingDistance);
            nextPos.x -= distanceTravelled;
            timeRemaining -= distanceTravelled / speed;
            distanceCovered += distanceTravelled;

            _currentTrackSection = 3;
        }

        //We are in the left curve
        if (timeRemaining > 0.0f && nextPos.x <= straightLeftSide)
        {
            Vector2 curveCentre = new Vector2(straightLeftSide, racetrackCentre.y);
            Vector2 startRadial = Vector2.up;
            Vector2 endRadial = Vector2.down;
            Vector2 toPosRadial = (nextPos - curveCentre).normalized;
            
            float angularSpeed = (speed) / curveRadius; 
            float angleToTravel = Mathf.Rad2Deg * angularSpeed * timeRemaining;
            float remainingAngle = Vector2.Angle(toPosRadial, endRadial);
            float angleTravelled = Mathf.Min(angleToTravel, remainingAngle);
            
            float currentAngle = Vector2.Angle(startRadial, toPosRadial);
            float angleOffset = Vector2.SignedAngle(Vector2.right, startRadial); 
            float nextPosAngle = Mathf.Deg2Rad * (currentAngle + angleTravelled + angleOffset);
            
            nextPos = curveCentre + curveRadius * (new Vector2(Mathf.Cos(nextPosAngle), Mathf.Sin(nextPosAngle)));
            distanceTravelled = Mathf.Deg2Rad * angleTravelled * curveRadius;

            timeRemaining -= distanceTravelled / speed;
            distanceCovered += distanceTravelled;
            _currentTrackSection = 4;
        }

        return nextPos;
    } 

    public Facing GetRaceFacing(Vector2 pos)
    {
        //If in the bottom half of the race course, face right, else face left
        return (pos.y <= transform.position.y) ? Facing.Right : Facing.Left;
    }

    public void TransitionToHamsterville()
    {
        UnityEvent onTransitionEnd = new UnityEvent();
        onTransitionEnd.AddListener(OnRaceEnd);
        endTransition.Play(onTransitionEnd);
    }


    void OnRaceEnd()
    {
        float[] completions = RaceCompletions();
        RacingHamster[] hamstersCopy = new RacingHamster[hamsters.Length];
        hamsters.CopyTo(hamstersCopy, 0);
        Array.Sort(completions, hamstersCopy);
        int rank = hamsters.Length - Array.IndexOf(hamstersCopy, playerWinner);
        HamsterDataPocket.instance.electricityReward = 30 - 10 * Math.Max(rank - 1, 0);

        HamsterDataPocket.instance.raceCircuit.QueueNextRace();
        SceneManager.LoadScene("Hamsterville");
    }

    public float CalcTrackDistance(float laneNumber)
    {
        float totalStraightDistance = 2.0f * this.straightLength;
        float curveRadius = this.minCurveRadius + this.laneWidth * ((float)laneNumber - 1.0f);
        float totalCurveDistance = 2.0f * Mathf.PI * curveRadius;
        float trackDistance = totalStraightDistance + totalCurveDistance;

        return trackDistance;
    }

    Vector2 StartingPosition(int laneNumber)
    {
        float laneRank = (float)laneNumber - 1.0f;
        float curveRadius = minCurveRadius + laneRank * laneWidth;   
        float distanceAdjustment = this.CalcTrackDistance(laneNumber) - this.CalcTrackDistance(1F);
        
        Vector2 racetrackStartline = transform.position;
        racetrackStartline += (Vector2.down * curveRadius);
        float distanceOffset = 0F;
        Vector2 startingPos = NextPosOnRaceCourse(racetrackStartline, distanceAdjustment, laneNumber, 1F, ref distanceOffset, true);

        Debug.Log("LANE::" + laneNumber);
        Debug.Log("Distance Adjustment::" + distanceAdjustment);
        Debug.Log("Distance Offset::" + distanceOffset);
        Debug.Log("****************************************");

        return startingPos;    
    }
}
