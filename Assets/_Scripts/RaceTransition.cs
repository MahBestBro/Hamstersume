using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public enum Easing
{
    ExponetialIn,
    ExponetialOut
}

public class RaceTransition : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float transitionDurationSecs;
    public Vector2 endPosition;
    public Easing easeMode;

    public bool IsPlaying
    {
        get
        {
            return playing;
        }
    }
    
    Vector2 startPosition;
    float elapsedTime = 0.0f;
    bool playing = false;
    UnityEvent _onTransitionEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            float t = elapsedTime / transitionDurationSecs;
            transform.position = Vector2.Lerp(startPosition, endPosition, Ease(t, easeMode));

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= transitionDurationSecs)
            {
                transform.position = endPosition;
                elapsedTime = 0.0f;
                playing = false;
                _onTransitionEnd?.Invoke();
            }
        }
    }


    public void PlayRaceTransition(UnityEvent onTransitionEnd)
    {
        transform.position = startPosition;
        elapsedTime = 0.0f;
        playing = true;
        _onTransitionEnd = onTransitionEnd;
    }

    float Ease(float x, Easing easing)
    {
        float unclamped = 0.0f;
        switch (easing)
        {
            case Easing.ExponetialIn:
                unclamped = Mathf.Pow(2.0f, 10.0f * (x - 1.0f));
                break;

            case Easing.ExponetialOut:
                unclamped = 1.0f - Mathf.Pow(2.0f, -10.0f * x);
                break;
        }
        
        return Mathf.Clamp(unclamped, 0.0f, 1.0f);
    }
}
