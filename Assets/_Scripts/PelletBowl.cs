using UnityEngine;

public class PelletBowl : MonoBehaviour
{
    const float MOUSE_CLICK_RAYCAST_DELTA = 0.001f; 

    [SerializeField]
    HamsterManager hamsterManager;
    [SerializeField]
    GameObject pelletPrefab;

    [HideInInspector]
    public Collider2D collider2D_;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider2D_ = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnPellet()
    {
        float spawnX = UnityEngine.Random.Range(
            hamsterManager.hamsterWalkArea.min.x,
            hamsterManager.hamsterWalkArea.max.x
        );

        float floorHeight = UnityEngine.Random.Range(
            hamsterManager.hamsterWalkArea.center.y, 
            hamsterManager.hamsterWalkArea.max.y
        );

        GameObject pellet = Instantiate(pelletPrefab, Vector3.zero, Quaternion.identity);
        pellet.GetComponent<Food>().DropAt(spawnX * Vector3.right + 5.0f * Vector3.up, null, floorHeight);
    }    
}
