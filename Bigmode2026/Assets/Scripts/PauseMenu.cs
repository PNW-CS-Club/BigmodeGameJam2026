using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PauseMenu : MonoBehaviour
{
    private InputSystem_Actions InputSystem_Actions;
    private InputAction Menu;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private bool isPaused;

    void Awake()
    {
        InputSystem_Actions = new InputSystem_Actions();
    }
    void Update()
    {

    }
    private void OnEnable()
    {
        Menu = InputSystem_Actions.Menu.Pause;
        Menu.Enable();

        Menu.performed += Pause;
    }
    private void OnDisable()
    {
        Menu.Disable();
    }
    void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }
    void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseUI.SetActive(true);

    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseUI.SetActive(false);
        isPaused = false;
    }
    public void MoveToMenu(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
        Time.timeScale = 1;
    }
}