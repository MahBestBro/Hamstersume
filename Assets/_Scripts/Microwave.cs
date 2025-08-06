using System;
using UnityEngine;
using UnityEngine.UI;

public class Microwave : MonoBehaviour
{
    public static Action<int> increaseElectricity;

    public HamsterVariant[] variants;

    Animator anim;

    public Animator Anim 
    {
        get
        {
            return anim;
        }
    }

    [Range(0, 999)]
    public int electricity;
    [Range(0, 100)]
    public int hamsterCost;

    public HamsterTracker hamsterTracker;
    public HamsterManager hamsterManager;

    [SerializeField]
    Text electricityText;
    [SerializeField]
    SpriteRenderer cookDisplay;

    [SerializeField]
    GameObject sunflowerSeedPrefab;
    [SerializeField]
    GameObject brocolliPrefab;
    [SerializeField]
    GameObject carrotPrefab;
    [SerializeField]
    GameObject hamsterPrefab;

    public FoodStats sunflowerSeedStats;
    public FoodStats brocolliStats;
    public FoodStats carrotStats;

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

    void Cook(GameObject prefab, bool isFood = true)
    {
        int cost = isFood ? prefab.GetComponent<Food>().Stats.electricityCost : hamsterCost;
        bool canPay = electricity >= cost;
        
        if (canPay)
        {
            electricity -= cost;
            anim.SetTrigger("Trigger");

            float spawnX = UnityEngine.Random.Range(
                hamsterManager.hamsterWalkArea.min.x,
                hamsterManager.hamsterWalkArea.max.x
            );

            float floorHeight = UnityEngine.Random.Range(
                hamsterManager.hamsterWalkArea.center.y, 
                hamsterManager.hamsterWalkArea.max.y
            );

            Vector3 spawnPos = spawnX * Vector3.right + 5.0f * Vector3.up;

            GameObject obj;
            Hamster newHamster = null;
            if (!isFood)
            {
                int variantIndex = UnityEngine.Random.Range(0, variants.Length);
                newHamster = hamsterManager.CreateHamster(variants[variantIndex]);
                obj = newHamster.gameObject;
            }
            else
            {
                obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            }

            obj.transform.position = spawnPos;
            Grabbable grabbableObj = obj.GetComponent<Grabbable>();
            grabbableObj.DropAt(spawnPos, null, floorHeight);
            if (grabbableObj) cookDisplay.sprite = newHamster?.hamsterVariant.hamsterIdle ?? grabbableObj.spriteRenderer.sprite;
        }
    }

    //TODO: Figure out position and shid
    void ShowFoodStats(FoodStats foodStats)
    {
        //Debug.Log($"Speed: +{foodStats.speedStatIncrease}");
        //Debug.Log($"Speed: +{foodStats.staminaStatIncrease}");
        //Debug.Log($"Speed: +{foodStats.powerStatIncrease}");
//
        //Debug.Log($"Energy: +{foodStats.energyRestored}");
    }


    public void CookSunflowerSeed()
    {
        Cook(sunflowerSeedPrefab);
    }

    public void CookCarrot()
    {
        Cook(carrotPrefab);
    }

    public void CookBrocolli()
    {
        Cook(brocolliPrefab);
    }

    public void CookHamster()
    {
        Cook(hamsterPrefab, false);
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
