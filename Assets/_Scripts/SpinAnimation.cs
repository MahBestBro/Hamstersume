using UnityEngine;

public class SpinAnimation : MonoBehaviour
{
    public bool spinning = false;
    public bool autoplayOnStart = true;
    public float spinSpeed = 10F;

    void Start()
    {
        if (autoplayOnStart) spinning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (spinning)
        {
            float spinRate = spinSpeed * Time.deltaTime;
            this.transform.Rotate(0, 0, -spinRate);
        }
    }
}
