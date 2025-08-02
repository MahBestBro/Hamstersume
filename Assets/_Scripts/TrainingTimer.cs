using UnityEngine;

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
    bool timerFinished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerMask = transform.Find("BarMask").GetComponent<RectTransform>();
        originalBarWidth = timerMask.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerFinished)
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


    void OnTimerCompletion()
    {
        transition.PlayRaceTransition();
        timerFinished = true;
    }
}
