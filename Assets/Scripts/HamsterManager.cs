using UnityEngine;

public class HamsterManager : MonoBehaviour
{
    [Range(0, 100.0f)]
    public float hamsterMinIdleTimeSecs;
    [Range(0, 100.0f)]
    public float hamsterMaxIdleTimeSecs;
    [Range(0, 100.0f)]
    public float hamsterWalkSpeed;
    public Bounds hamsterWalkArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            Hamster hamster = transform.GetChild(i).GetComponent<Hamster>();
            hamster.minIdleTimeSecs = hamsterMinIdleTimeSecs;
            hamster.maxIdleTimeSecs = hamsterMaxIdleTimeSecs;
            hamster.walkSpeed = hamsterWalkSpeed;
            hamster.walkArea = hamsterWalkArea;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hamsterWalkArea.center, hamsterWalkArea.size);
    }
}
