using System;
using UnityEngine;

[Serializable]
public class HamsterProfile
{
    [SerializeField]
    public HamsterStats hStats = new HamsterStats();
    [SerializeField]
    public HamsterVariant hVariant;

    public HamsterProfile()
    {
    }

    public HamsterProfile(HamsterProfile copyProfile)
    {
        //TODO: Implement. hStats is referenced and thus must manually deep copy
    }
}
