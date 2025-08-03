using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float InitialVolume;

    static Soundtrack instance;
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            AudioListener.volume = InitialVolume;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }


    public static void SetVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}
