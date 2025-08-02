using System.Collections.Generic;
using UnityEngine;

public class HamsterDataPocket : MonoBehaviour
{
	public static HamsterDataPocket instance;

    [SerializeField]
    public RacingCircuit raceCircuit;
    [SerializeField]
    public TrainingTimer trainingTimer
    {
        get { return _trainingTimer; }
        set {
            this._trainingTimer = value;
            this._trainingTimer.onTimerEnded.AddListener(OnTrainingPhaseEnded);
        }
    }
    private TrainingTimer _trainingTimer;
    [SerializeField]
	public HamsterManager hamsterManager;
    [SerializeField]
    public List<HamsterProfile> hamsters = new List<HamsterProfile>();

	void Awake()
	{
        if (instance)
        {
            instance.trainingTimer = this.trainingTimer;
            instance.hamsterManager = this.hamsterManager;
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
	}

    private void Start()
    {
        if (hamsterManager)
		{
			Hamster[] managedHamsters = hamsterManager.GetManagedHamsters();
			hamsters.Capacity += managedHamsters.Length;
			foreach (Hamster hamster in managedHamsters)
			{
				hamsters.Add(hamster.hamsterProfile);
            }
        }
    }

    void OnTrainingPhaseEnded()
    {
        // TEMP: Assign random racer/s
        raceCircuit.CurrentRace.playerParticipants = this.GetRandomHamsters(raceCircuit.CurrentRace.numberPlayerParticipants);
    }

    List<HamsterProfile> GetRandomHamsters(int num)
    {
        List<HamsterProfile> selectableHamsters = new List<HamsterProfile>(this.hamsters);
        List<HamsterProfile> selectedHamsters = new List<HamsterProfile>(num);
        for (int i = 0; i < num; i++)
        {
            if (selectableHamsters.Count <= 0) { break; }
            int index = Random.Range(0, selectableHamsters.Count);
            selectedHamsters.Add(this.hamsters[index]);
            selectableHamsters.RemoveAt(index);
        }
        return selectedHamsters;
    }

    HamsterProfile GetRandomHamster()
    {
        return hamsters[Random.Range(0, hamsters.Count)];
    }
}
