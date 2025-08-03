using System;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenshotCapture : MonoBehaviour
{
    #if UNITY_EDITOR

    InputAction takeScreenshot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        takeScreenshot = InputSystem.actions.FindAction("Take Screenshot");
    }

    // Update is called once per frame
    void Update()
    {
        if (takeScreenshot.WasPressedThisFrame())
        {
            StringBuilder nowStringBuilder = new StringBuilder(DateTime.Now.ToString());
            for (int i = 0; i < nowStringBuilder.Length; i++)
            {
                if (Char.IsPunctuation(nowStringBuilder[i]))
                {
                    nowStringBuilder[i] = '_';
                }
            }
            ScreenCapture.CaptureScreenshot($"Assets/Graphics/Screenshot_{nowStringBuilder.ToString()}.png");
        }   
    }

    #endif
}
