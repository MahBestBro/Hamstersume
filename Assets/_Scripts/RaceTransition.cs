using UnityEngine;
using UnityEngine.UI;

public class RaceTransition : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float transitionDurationSecs;
    public Vector2 endPosition;
    
    Vector2 startPosition;
    float elapsedTime = 0.0f;
    bool transitioning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
        {
            float fracTransitionCompleted = elapsedTime / transitionDurationSecs;
            float t = Mathf.Clamp(1.0f - Mathf.Pow(2.0f, -10.0f * fracTransitionCompleted), 0.0f, 1.0f);
            transform.position = Vector2.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= transitionDurationSecs)
            {
                transform.position = endPosition;
                elapsedTime = 0.0f;
                transitioning = false;
            }
        }
    }


    public void PlayRaceTransition()
    {
        transform.position = startPosition;
        elapsedTime = 0.0f;
        transitioning = true;
    }
}
