using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float pageTransitionDuration = 1.0f;
    [SerializeField]
    Vector2 mainPageOffscreenCentre;
    [SerializeField]
    Vector2 nonMainPageOffscreenCentre;

    [SerializeField]
    Transform mainPage;
    [SerializeField]
    Transform guidePage;
    [SerializeField]
    Transform creditsPage;

    [SerializeField]
    ScreenTransition startTransition;
    [SerializeField]
    ScreenTransition playTransition;
    [SerializeField]
    BigCursor specialCursor;

    InputAction mousePos;
    InputAction pickUp;
    Transform previousPage;
    Transform currentPage;
    
    Vector2 currentPageOffscreenPos;
    Vector2 prevPageOffscreenPos;

    bool switchingPage = false; 
    float pageSwitchElapsedTime = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mousePos = InputSystem.actions.FindAction("Mouse Pos");
        pickUp = InputSystem.actions.FindAction("Pick Up");

        mainPage = transform.Find("MainPage");
        guidePage = transform.Find("GuidePage");
        creditsPage = transform.Find("CreditsPage");

        guidePage.position = nonMainPageOffscreenCentre;
        creditsPage.position = nonMainPageOffscreenCentre;

        currentPage = mainPage;
        currentPageOffscreenPos = nonMainPageOffscreenCentre;
        prevPageOffscreenPos = mainPageOffscreenCentre;

        startTransition.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = this.mousePos.ReadValue<Vector2>();
        specialCursor.Goto(mousePosition);

        if (pickUp.WasPressedThisFrame())
        {
            this.specialCursor.OnLeftClickDown();
        } 
        else if (pickUp.WasReleasedThisFrame())
        {
            this.specialCursor.OnLeftClickUp();
        }
            
        Vector2 screenCentre = Camera.main.WorldToScreenPoint(Vector2.zero);
        
        if (switchingPage)
        {
            float t = pageSwitchElapsedTime / pageTransitionDuration; 
            currentPage.position = Vector2.Lerp(
                currentPageOffscreenPos, 
                screenCentre, 
                MathHelpers.ExponentialEaseOut(t)
            );
            previousPage.position  = Vector2.Lerp(
                screenCentre, 
                prevPageOffscreenPos, 
                MathHelpers.ExponentialEaseOut(t)
            );

            pageSwitchElapsedTime += Time.deltaTime;
        }

        if (pageSwitchElapsedTime >= pageTransitionDuration)
        {
            currentPage.position = screenCentre; 
            previousPage.position = prevPageOffscreenPos;
            switchingPage = false;
            pageSwitchElapsedTime = 0.0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mainPageOffscreenCentre, 5.0f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(nonMainPageOffscreenCentre, 5.0f);

    }


    public void SwitchToPage(Transform page, Vector2 offscreenPos)
    {
        previousPage = currentPage;
        prevPageOffscreenPos = offscreenPos;
        
        currentPage = page;
        currentPageOffscreenPos = currentPage.position;
        
        switchingPage = true;
        pageSwitchElapsedTime = 0.0f;
    }
    
    
    public void SwitchToMainPage()
    {
        SwitchToPage(mainPage, nonMainPageOffscreenCentre);
    }

    public void SwitchToGuidePage()
    {
        SwitchToPage(guidePage, mainPageOffscreenCentre);
    }

    public void SwitchToCreditsPage()
    {
        SwitchToPage(creditsPage, mainPageOffscreenCentre);
    }

    public void PlayGame()
    {
        UnityEvent onTransitionEnd = new UnityEvent();
        onTransitionEnd.AddListener(() => SceneManager.LoadScene("Hamsterville"));
        playTransition.Play(onTransitionEnd);
    }

    public void QuitGame()
    {
        Debug.Log("Game quit.");
        Application.Quit();
    }
}
