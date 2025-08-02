using System;
using UnityEngine;
using UnityEngine.UI;

public class Microwave : MonoBehaviour
{
    public static Action<int> increaseElectricity;

    Animator anim;

    [Range(0, 100)]
    public int electricity;

    public HamsterTracker hamsterTracker;
    public HamsterManager hamsterManager;

    [SerializeField]
    Text electricityText;

    [SerializeField]
    GameObject sunflowerSeedPrefab;
    [SerializeField]
    GameObject brocolliPrefab;
    [SerializeField]
    GameObject carrotPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        increaseElectricity += IncreaseEletricity;
    }

    // Update is called once per frame
    void Update()
    {
        electricityText.text = electricity.ToString();
    }


    void IncreaseEletricity(int electricityGain)
    {
        electricity += electricityGain;  
    }

    void CookFood(GameObject foodPrefab)
    {
        Food food = foodPrefab.GetComponent<Food>();

        bool canPayForFood = electricity >= food.electricityCost;
        electricity -= food.electricityCost * Convert.ToInt32(canPayForFood);
        
        if (canPayForFood)
        {
            anim.SetTrigger("Trigger");

            float spawnX = UnityEngine.Random.Range(
                hamsterManager.hamsterWalkArea.min.x,
                hamsterManager.hamsterWalkArea.max.x
            );

            float floorHeight = UnityEngine.Random.Range(
                hamsterManager.hamsterWalkArea.center.y, 
                hamsterManager.hamsterWalkArea.max.y
            );
            
            GameObject foodObj = Instantiate(foodPrefab, Vector3.zero, Quaternion.identity);
            foodObj.GetComponent<Food>().DropAt(spawnX * Vector3.right + 5.0f * Vector3.up, null, floorHeight);
        }
    }


    public void CookSunflowerSeed()
    {
        CookFood(sunflowerSeedPrefab);
    }

    public void CookCarrot()
    {
        CookFood(carrotPrefab);
    }

    public void CookBrocolli()
    {
        CookFood(brocolliPrefab);
    }
}
