using System.Collections.Generic;
using UnityEngine;

public class HamsterDataPocket : MonoBehaviour
{
	public static HamsterDataPocket instance;

    [SerializeField]
    public RacingCircuit raceCircuit;
    [SerializeField]
    public Racecourse raceCourse;
    [SerializeField]
    public TrainingTimer trainingTimer;
    [SerializeField]
	public HamsterManager hamsterManager;
    [SerializeField]
    public List<HamsterProfile> hamsters = new List<HamsterProfile>();

	void Awake()
	{
        if (instance)
        {
            instance.EncounterSceneInstance(this);
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.OnEnterNewScene();
    }

    void EncounterSceneInstance(HamsterDataPocket sceneInstance) {
        this.trainingTimer = sceneInstance.trainingTimer;
        this.hamsterManager = sceneInstance.hamsterManager;
        this.raceCourse = sceneInstance.raceCourse;
        this.OnEnterNewScene();
    }

    void OnEnterNewScene()
    {
        if (this.trainingTimer)
        {
            this.trainingTimer.onTimerEnded.AddListener(this.OnTrainingPhaseEnded);
        }
        if (this.raceCourse)
        {
            this.raceCourse.InitialiseRacecourse(this.raceCircuit?.CurrentRace);
            this.raceCourse.StartRace();
        }
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
