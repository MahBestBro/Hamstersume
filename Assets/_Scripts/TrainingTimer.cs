using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainingTimer : MonoBehaviour
{
    [SerializeField]
    float traningDurationSecs;
    [SerializeField]
    ScreenTransition startTransition;
    [SerializeField]
    ScreenTransition endTransition;
    [SerializeField]
    TextMeshProUGUI timeRemainingLabel;
    [SerializeField]
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
        originalBarWidth = timerMask.rect.width;
        this.UpdateLabel();

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

            this.UpdateLabel();

            elapsedTimeSecs += Time.deltaTime;
            if (elapsedTimeSecs >= traningDurationSecs)
            {
                OnTimerCompletion();
            }
        }
    }

    void UpdateLabel()
    {
        if (timeRemainingLabel)
        {
            timeRemainingLabel.text = Mathf.CeilToInt(traningDurationSecs - elapsedTimeSecs).ToString();
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
        this.UpdateLabel();
        onTimerEnded.Invoke();

        // Transition to next scene
        UnityEvent onTransitionEnd = new UnityEvent();
        onTransitionEnd.AddListener(() => SceneManager.LoadScene("HamsterSelection")); 
        endTransition.Play(onTransitionEnd);
    }

}
