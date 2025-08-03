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
    public RacingPreview racingPreview;
    [SerializeField]
    public HamsterSelectionBox hamsterBox;
    [SerializeField]
    public List<HamsterProfile> hamsters = new List<HamsterProfile>();

    int numRounds = 0;
    public int NumRounds
    {
        get
        {
            return numRounds;
        }
    }

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
        CollectStartingHamsters();
        this.OnEnterNewScene();
    }

    void EncounterSceneInstance(HamsterDataPocket sceneInstance) 
    {
        this.trainingTimer = sceneInstance.trainingTimer;
        this.hamsterManager = sceneInstance.hamsterManager;
        this.racingPreview = sceneInstance.racingPreview;
        this.hamsterBox = sceneInstance.hamsterBox;
        this.raceCourse = sceneInstance.raceCourse;
        this.OnEnterNewScene();
    }

    void OnEnterNewScene()
    {
        numRounds += 1;

        if (this.hamsterManager)
        {
            this.hamsterManager.NukeManagedHamsters();
            foreach (HamsterProfile profile in hamsters)
            {
                this.hamsterManager.CreateHamster(profile);
            }
        }
        if (this.trainingTimer)
        {
            this.trainingTimer.onTimerEnded.AddListener(this.OnTrainingPhaseEnded);
        }
        if (this.racingPreview)
        {
            this.racingPreview.PopulateRacerList(this.raceCircuit.CurrentRace.npcParticipants);
        }
        if (this.hamsterBox)
        {
            this.hamsterBox.onSelectionConfirmed.AddListener(this.OnRacersSelected);
        }
        if (this.raceCourse)
        {
            this.raceCourse.InitialiseRacecourse(this.raceCircuit?.CurrentRace);
            this.raceCourse.StartRace();
        }
    }

    void CollectStartingHamsters()
    {
        if (hamsterManager)
        {
            Hamster[] managedHamsters = hamsterManager.GetManagedHamsters();
            hamsters.Capacity += managedHamsters.Length;
            foreach (Hamster hamster in managedHamsters)
            {
                hamster.InitialiseNewHamster();
                hamsters.Add(hamster.hamsterProfile);
            }

        }
    }

    void RecollectManagedHamsters()
    {
        if (hamsterManager)
		{
			Hamster[] managedHamsters = hamsterManager.GetManagedHamsters();
            hamsters.Clear();
			hamsters.Capacity += managedHamsters.Length;
			foreach (Hamster hamster in managedHamsters)
			{
				hamsters.Add(hamster.hamsterProfile);
            }
            
        }
    }

    void OnTrainingPhaseEnded()
    {
        this.RecollectManagedHamsters();

        // TEMP: Assign random racer/s
        // raceCircuit.CurrentRace.playerParticipants = this.GetRandomHamsters(raceCircuit.CurrentRace.numberPlayerParticipants);
    }

    void OnRacersSelected(List<HamsterProfile> selectedHamsters)
    {
        raceCircuit.CurrentRace.playerParticipants = selectedHamsters;
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
