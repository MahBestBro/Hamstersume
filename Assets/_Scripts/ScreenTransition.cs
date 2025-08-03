using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public enum Easing
{
    ExponetialIn,
    ExponetialOut
}

public class ScreenTransition : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float transitionDurationSecs;
    public Vector2 endPosition;
    public Easing easeMode;
    public bool autoplayOnStart = false;

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

    void Awake()
    {
        startPosition = transform.position;
        if (autoplayOnStart) {
            this.Play();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            float t = elapsedTime / transitionDurationSecs;
            transform.position = Vector2.Lerp(startPosition, endPosition, Ease(t, easeMode));

            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime >= transitionDurationSecs)
            {
                transform.position = endPosition;
                elapsedTime = 0.0f;
                playing = false;
                _onTransitionEnd?.Invoke();
            }
        }
    }


    public void Play(UnityEvent onTransitionEnd = null)
    {
        transform.position = startPosition;
        elapsedTime = 0.0f;
        playing = true;
        _onTransitionEnd = onTransitionEnd;
    }

    float Ease(float x, Easing easing)
    {
        switch (easing)
        {
            case Easing.ExponetialIn:
                return MathHelpers.ExponentialEaseIn(x);

            case Easing.ExponetialOut:
                return MathHelpers.ExponentialEaseOut(x);

            default:
                return x;
        }
    }
}
