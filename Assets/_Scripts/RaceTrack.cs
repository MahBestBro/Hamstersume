using System;
using UnityEngine;

[Serializable]
public class RaceTrack
{
    // Maybe make this a monobehaviour prefab to share data; or a scriptableobject but will have to prevent modifying values on the scriptableobject asset
    [Range(0.0f, 50.0f)]
    public float minCurveRadius = 2.2F;
    [Range(0.0f, 100.0f)]
    public float straightLength = 14.3F;
    [Range(0.0f, 50.0f)]
    public float laneWidth = 1.27F;
    [Range(1, 10)]
    public int numLanes = 5;

    public float CalcTrackDistance(int laneNumber)
    {
        float totalStraightDistance = 2.0f * this.straightLength;
        float curveRadius = this.minCurveRadius + this.laneWidth * ((float)laneNumber - 1.0f);
        float totalCurveDistance = 2.0f * Mathf.PI * curveRadius;
        float trackDistance = totalStraightDistance + totalCurveDistance;

        return trackDistance;
    }

    public Vector2 ResizeRacecourse(float sizeFactor, Vector2 racetrackSize)
    {
        float maxCurveRadius = this.minCurveRadius + ((numLanes + 1) * laneWidth);
        float scaledStraightLength = this.straightLength * sizeFactor;
        //Vector2 racetrackSize = raceTrackSprite.size;
        float trackspriteLength = racetrackSize.x;
        float trackspriteStraightLength = (straightLength * trackspriteLength) / (straightLength + (2F * maxCurveRadius));
        float trackspriteNonstraightLength = trackspriteLength - trackspriteStraightLength;
        racetrackSize.x = trackspriteStraightLength * sizeFactor + trackspriteNonstraightLength;

        this.straightLength = scaledStraightLength;
        //raceTrackSprite.size = racetrackSize;
        return racetrackSize;
    }
}
