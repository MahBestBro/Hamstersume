using UnityEngine;

public class PelletBowl : MonoBehaviour
{
    [SerializeField]
    HamsterManager hamsterManager;
    [SerializeField]
    Food pelletPrefab;
    [SerializeField]
    Vector2 pelletSpawnOffset;

    [HideInInspector]
    public Collider2D collider2D_;
    
    void Start()
    {
        collider2D_ = GetComponent<Collider2D>();
    }


    public Food SpawnPellet(Vector2 spawnAt)
    {
        Food pellet = Instantiate(pelletPrefab, spawnAt + pelletSpawnOffset, Quaternion.identity);
        return pellet;
        //float spawnX = UnityEngine.Random.Range(
        //    hamsterManager.hamsterWalkArea.min.x,
        //    hamsterManager.hamsterWalkArea.max.x
        //);

        //float floorHeight = UnityEngine.Random.Range(
        //    hamsterManager.hamsterWalkArea.center.y, 
        //    hamsterManager.hamsterWalkArea.max.y
        //);

        //GameObject pellet = Instantiate(pelletPrefab, Vector3.zero, Quaternion.identity);
        //pellet.GetComponent<Food>().DropAt(spawnX * Vector3.right + 5.0f * Vector3.up, null, floorHeight);
    }    
}
