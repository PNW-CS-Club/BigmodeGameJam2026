using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EndMenu : MonoBehaviour
{
    private InputSystem_Actions InputSystem_Actions;
    private InputAction Menu;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject endMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    
 

    private float prevTimeScale;
    private bool isInOptions;

    void Awake()
    {
        InputSystem_Actions = new InputSystem_Actions();
    }
    
    private void OnEnable()
    {
        Menu = InputSystem_Actions.Menu.Pause;
        Menu.Enable();

        Menu.performed += OnEscape;
    }

    private void OnDisable()
    {
        Menu.Disable();
    }

    void OnEscape(InputAction.CallbackContext context) {
        
        if (isInOptions) {
            ReturnFromOptions();
        }
    }
  
    public void ActivateMenu()
    {
        
        //AudioListener.pause = true;
        
        isInOptions = false;
        
        endPanel.SetActive(true);
        endMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }
    
    public void OpenOptions() {
        isInOptions = true;
        
        endMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void ReturnFromOptions() {
        isInOptions = false;
        
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
