using UnityEngine;
using Unity.Profiling;
using TMPro;
using System;
using System.Collections;

public class StatisticManager : MonoBehaviour
{
    [SerializeField] Canvas statsCanvas;
    [SerializeField] TextMeshProUGUI FPSText;
    [SerializeField] TextMeshProUGUI MemoryUsageText;
    
    float fps;
    ProfilerRecorder totalUsedMemoryRecorder;

    // retrieves the total used memory for the scene
    private void OnEnable()
    {
        totalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
    }
    private void OnDisable()
    {
        totalUsedMemoryRecorder.Dispose();
    }
    private void Start()
    {
        StartCoroutine(DisplayFPS());
    }

    // Updates the displayed FPS every second so to make it more readable
    IEnumerator DisplayFPS()
    {
        FPSText.text = "FPS: " + Mathf.RoundToInt(fps);
        yield return new WaitForSeconds(1);
        StartCoroutine(DisplayFPS());

    }
    
    void Update()
    {
        // Can toggle stats on and off using the p key
        if(Input.GetKeyDown(KeyCode.P))
        {
            statsCanvas.enabled = !statsCanvas.enabled;
        }

        CalculateFPS();
        DisplayMemory();
    }

    // Calculates the number of frames per second
    private void CalculateFPS()
    {
        fps = 1 / Time.unscaledDeltaTime;
    }

    // if valid memory, the current used memory is displayed in megabytes
    private void DisplayMemory()
    {
        if (totalUsedMemoryRecorder.Valid)
        {
            long memoryUsed = totalUsedMemoryRecorder.LastValue;
            float memoryInMB = memoryUsed / (1024 * 1024);
            MemoryUsageText.text = "Memory Used: " + memoryInMB + "MB";
        }  
    }
}
