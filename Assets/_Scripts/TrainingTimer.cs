using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TrainingTimer : MonoBehaviour
{
    [SerializeField]
    float traningDurationSecs;
    [SerializeField]
    ScreenTransition startTransition;
    [SerializeField]
    ScreenTransition endTransition;

    RectTransform timerMask;
    
    [SerializeField]
    float elapsedTimeSecs = 0.0f;
    float originalBarWidth;
    bool timerStarted = false;
    bool timerFinished = false;
    public bool isTimerStarted {  get { return timerStarted; } }

    public UnityEvent onTimerEnded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerMask = transform.Find("BarMask").GetComponent<RectTransform>();
        originalBarWidth = timerMask.rect.width;

        UnityEvent onTransitionEnd = new UnityEvent();
        //onTransitionEnd.AddListener(() => StartTimer());
        startTransition.Play(onTransitionEnd);
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
        onTimerEnded.Invoke();

        // Transition to next scene
        UnityEvent onTransitionEnd = new UnityEvent();
        onTransitionEnd.AddListener(() => SceneManager.LoadScene("HamsterSelection")); 
        endTransition.Play(onTransitionEnd);
    }

}
