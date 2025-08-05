using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;


public enum FoodKind
{
    SunflowerSeed,
    Carrot,
    Broccoli
}

public class MicrowaveButton : MonoBehaviour
{
    const float MOUSE_CLICK_RAYCAST_DELTA = 0.001f; 

    public UnityEvent onClick;
    public bool isHamsterButton = false;
    public FoodKind foodKind;

    Collider2D collider2D_;
    InputAction pickUp; //TODO: Rename so it's just left click tbh
    InputAction mousePos;
    Hoverable hover;
    GameObject infoDisplay;

    Microwave microwave;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickUp = InputSystem.actions.FindAction("Pick Up");
        mousePos = InputSystem.actions.FindAction("Mouse Pos");
        
        collider2D_ = GetComponent<Collider2D>();
        hover = GetComponent<Hoverable>();
        microwave = transform.parent.parent.GetComponent<Microwave>();
        infoDisplay = transform.Find("Display").gameObject;

        if (!isHamsterButton)
        {
            FoodStats foodStats = microwave.sunflowerSeedStats;
            switch (foodKind)
            {
                case FoodKind.SunflowerSeed:
                    foodStats = microwave.sunflowerSeedStats;
                    break;

                case FoodKind.Carrot:
                    foodStats = microwave.carrotStats;
                    break;

                case FoodKind.Broccoli:
                    foodStats = microwave.brocolliStats;
                    break;
            }

            Transform foodInfo = infoDisplay.transform.Find("FoodInfo");
            foodInfo.Find("Speed").GetComponentInChildren<TMP_Text>().text = $"+{foodStats.speedStatIncrease}";
            foodInfo.Find("Stamina").GetComponentInChildren<TMP_Text>().text = $"+{foodStats.staminaStatIncrease}";
            foodInfo.Find("Power").GetComponentInChildren<TMP_Text>().text = $"+{foodStats.powerStatIncrease}";
            foodInfo.Find("Energy").GetComponentInChildren<TMP_Text>().text = $"+{foodStats.energyRestored}";
            
            transform.Find("CostDisplay").GetComponentInChildren<TMP_Text>().text = $"{foodStats.electricityCost}";
        }
        else
        {
            infoDisplay.transform.Find("HamsterIcon").gameObject.SetActive(true);
            infoDisplay.transform.Find("FoodInfo").gameObject.SetActive(false);
            
            transform.Find("CostDisplay").GetComponentInChildren<TMP_Text>().text = $"{microwave.hamsterCost}";
        }


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos.ReadValue<Vector2>());
        
        if (pickUp.WasReleasedThisFrame())
        {
            Vector2 lineEnd = mouseWorldPos + MOUSE_CLICK_RAYCAST_DELTA * Vector2.right;
            if (collider2D_.OverlapPoint(mouseWorldPos))
            {
                onClick.Invoke();
            }
        }

        //TODO: This only half works, Fix.
        bool isMicrowaveAnimatorPlaying = microwave.Anim.GetCurrentAnimatorStateInfo(0).length > microwave.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        infoDisplay.SetActive(hover.isHovered && !isMicrowaveAnimatorPlaying);
    }
}
