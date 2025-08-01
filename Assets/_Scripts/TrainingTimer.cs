using UnityEngine;

public class TrainingTimer : MonoBehaviour
{
    [SerializeField]
    float traningDurationSecs;

    RectTransform timerMask;
    
    [SerializeField]
    float elapsedTimeSecs = 0.0f;
    float originalBarWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerMask = transform.Find("BarMask").GetComponent<RectTransform>();
        originalBarWidth = timerMask.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        float fracTimeRemaining = Mathf.Max(1.0f - elapsedTimeSecs / traningDurationSecs, 0.0f);
        timerMask.SetInsetAndSizeFromParentEdge(
            RectTransform.Edge.Left, 
            0.0f,
            fracTimeRemaining * originalBarWidth
        );
    
        elapsedTimeSecs += Time.deltaTime;
    }
}
