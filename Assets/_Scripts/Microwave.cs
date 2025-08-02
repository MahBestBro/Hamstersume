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

    [SerializeField]
    FoodStats sunflowerSeedStats;
    [SerializeField]
    FoodStats brocolliStats;
    [SerializeField]
    FoodStats carrotStats;

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

        bool canPayForFood = electricity >= food.Stats.electricityCost;
        electricity -= food.Stats.electricityCost * Convert.ToInt32(canPayForFood);
        
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

    //TODO: Figure out position and shid
    void ShowFoodStats(FoodStats foodStats)
    {
        Debug.Log($"Speed: +{foodStats.speedStatIncrease}");
        Debug.Log($"Speed: +{foodStats.staminaStatIncrease}");
        Debug.Log($"Speed: +{foodStats.powerStatIncrease}");

        Debug.Log($"Energy: +{foodStats.energyRestored}");
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

    public void ShowSunflowerSeedStats()
    {
        ShowFoodStats(sunflowerSeedStats);
    }

    public void ShowCarrotStats()
    {
        ShowFoodStats(carrotStats);
    }

    public void ShowBrocolliStats()
    {
        ShowFoodStats(brocolliStats);
    }
}
