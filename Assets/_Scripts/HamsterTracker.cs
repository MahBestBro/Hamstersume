using System.Collections.Generic;
using UnityEngine;

public class HamsterWheelIDPair
{
    public int hamsterInstanceID;
    public int wheelInstanceID;

    public HamsterWheelIDPair(int hamsterID, int wheelID)
    {
        hamsterInstanceID = hamsterID;
        wheelInstanceID = wheelID;
    }
}

public class HamsterTracker : MonoBehaviour
{
    public List<HamsterWheelIDPair> hamsterWheelMap;
    public TrainingTimer timer;

    void Start()
    {
        hamsterWheelMap = new List<HamsterWheelIDPair>();
    }

    //NOTE: Returns the Instance ID of hamster already in wheel
    public int? PlaceHamsterInWheel(Hamster hamster, HamsterWheel wheel)
    {
        if(!timer.isTimerStarted) timer.StartTimer(); // start timer when first hamster is placed in wheel

        int wheelID = wheel.gameObject.GetInstanceID();   
        int newHamsterID = hamster.gameObject.GetInstanceID();
        for (int i = 0; i < hamsterWheelMap.Count; i++)
        {
            if (hamsterWheelMap[i].wheelInstanceID == wheelID)
            {
                int replacedHamsterID = hamsterWheelMap[i].hamsterInstanceID;
                hamsterWheelMap[i].hamsterInstanceID = newHamsterID;
                return replacedHamsterID;
            }
        }

        hamsterWheelMap.Add(new HamsterWheelIDPair(newHamsterID, wheelID));
        return null;
    }

    public int? OccupiedHamsterWheelMapIndex(HamsterWheel wheel)
    {
        int id = wheel.gameObject.GetInstanceID();
        for (int i = 0; i < hamsterWheelMap.Count; i++)
        {
            if (hamsterWheelMap[i].wheelInstanceID == id)
            {
                return i;
            }
        }

        return null;
    }

    public void UnmarkExercisingHamster(Hamster hamster)
    {
        int id = hamster.gameObject.GetInstanceID();
        int? hamsterWheelPairToRemoveIndex = null;
        for (int i = 0; i < hamsterWheelMap.Count; i++)
        {
            if (hamsterWheelMap[i].hamsterInstanceID == id)
            {
                hamsterWheelPairToRemoveIndex = i;
                break;
            }
        }

        if (hamsterWheelPairToRemoveIndex is int indexToRemove)
        {
            hamsterWheelMap[indexToRemove] = hamsterWheelMap[hamsterWheelMap.Count - 1];
            hamsterWheelMap.RemoveAt(hamsterWheelMap.Count - 1);
        }
    }
}
