using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    CanvasGroup canvasGroup;
    InputAction pause;

    bool paused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pause = InputSystem.actions.FindAction("Pause");
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        canvasGroup.blocksRaycasts = paused;
        canvasGroup.alpha = Convert.ToSingle(paused);
        Time.timeScale = Convert.ToSingle(!paused);

        if (pause.WasPressedThisFrame())
        {
            TogglePause();
        }
    }


    public void TogglePause()
    {
        paused = !paused;   
    }

    public void QuitGame()
    {
        Debug.Log("Game quit.");
        Application.Quit();
    }
}
