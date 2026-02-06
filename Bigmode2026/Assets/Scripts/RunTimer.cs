using UnityEngine;

public class RunTimer : MonoBehaviour
{
    public float runTime { get; private set; }
    public bool isRunning = false;

    void Update()
    {
        if (isRunning)
        {
            runTime += Time.deltaTime; // In seconds
        }

    }

    public void StartRun()
    {
        if(isRunning) return; // Prevent accidental double calls
        Time.timeScale = 1f; // Normal time scale
        runTime = 0f;
        isRunning = true;
    }

    public void EndRun()
    {
        if(!isRunning) return; // Prevent accidental double calls
        Time.timeScale = 0f;
        isRunning = false;
        Debug.Log("Final Time: " + runTime);
    }

    public void PauseRun(){
        Time.timeScale = 0f;
    }

    public void ResumeRun(){
        Time.timeScale = 1f;
    }
}
