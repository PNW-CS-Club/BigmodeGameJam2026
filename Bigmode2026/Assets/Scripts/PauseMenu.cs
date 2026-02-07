using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PauseMenu : MonoBehaviour
{
    private InputSystem_Actions InputSystem_Actions;
    private InputAction Menu;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    
    private bool isPaused;
    private bool isInOptions;
    private float prevTimeScale;

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
        if (isPaused) {
            if (isInOptions) {
                ReturnFromOptions();
            }
            else {
                DeactivateMenu();
            }
        }
        else {
            ActivateMenu();
        }
    }
    public void ActivateMenu()
    {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
        AudioListener.pause = true;
        
        isPaused = true;
        isInOptions = false;
        
        pausePanel.SetActive(true);
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }
    
    public void OpenOptions() {
        isInOptions = true;
        
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void ReturnFromOptions() {
        isInOptions = false;
        
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = prevTimeScale;
        AudioListener.pause = false;
        
        isPaused = false;
        pausePanel.SetActive(false);
    }
    public void MoveToMenu(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
        Time.timeScale = 1;
    }
}