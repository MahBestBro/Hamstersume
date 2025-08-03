using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float InitialVolume;

    static Soundtrack instance;
    static float volume;
    
    public static float Volume
    {
        get
        {
            return volume;
        }
    }

    AudioSource audioPlayer;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            volume = InitialVolume;
            audioPlayer = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        audioPlayer.volume = volume;
    }


    public static void SetVolume(float newVolume)
    {
        volume = newVolume;
    }
}
