using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject endMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    
 

    private float prevTimeScale;

  
    public void ActivateMenu()
    {
        
        //AudioListener.pause = true;
        
        
        endPanel.SetActive(true);
        endMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }
    
    public void OpenOptions() {       
        endMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void ReturnFromOptions() {
        
        endMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void NewRun(){
        gameManager.ResetRun();
        endPanel.SetActive(false);
    }

    public void MoveToMenu(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
        Time.timeScale = 1;
    }
}
