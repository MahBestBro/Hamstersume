using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TrainingTimer : MonoBehaviour
{
    [SerializeField]
    float traningDurationSecs;
    [SerializeField]
    RaceTransition transition;

    RectTransform timerMask;
    
    [SerializeField]
    float elapsedTimeSecs = 0.0f;
    float originalBarWidth;
    bool timerStarted = false;
    bool timerFinished = false;
    public bool isTimerStarted {  get { return timerStarted; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerMask = transform.Find("BarMask").GetComponent<RectTransform>();
        originalBarWidth = timerMask.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted && !timerFinished)
        {
            float fracTimeRemaining = Mathf.Max(1.0f - elapsedTimeSecs / traningDurationSecs, 0.0f);
            timerMask.SetInsetAndSizeFromParentEdge(
                RectTransform.Edge.Left, 
                0.0f,
                fracTimeRemaining * originalBarWidth
            );

            elapsedTimeSecs += Time.deltaTime;
            if (elapsedTimeSecs >= traningDurationSecs)
            {
                OnTimerCompletion();
            }
        }
    }

    public bool StartTimer() // (called in HamsterTracker)
    {
        if (timerStarted) return false;
        timerStarted = true;
        return true;
    }

    void OnTimerCompletion()
    {
        timerFinished = true;
        UnityEvent onTransitionEnd = new UnityEvent();
        onTransitionEnd.AddListener(SwitchToRaceScene); 
        transition.PlayRaceTransition(onTransitionEnd);
    }

    void SwitchToRaceScene()
    {
        SceneManager.LoadScene("Racing");
    }
}
