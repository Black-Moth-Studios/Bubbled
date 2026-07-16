using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private PlayerInputActions input;

    private InputAction pauseAction;

    public GameObject pauseMenu;

    private bool isPaused;

    void Awake()
    {
        input = new PlayerInputActions();

        pauseAction = input.UI.Pause;
    }


    void OnEnable()
    {
        input.Enable();
    }


    void OnDisable()
    {
        input.Disable();
    }



    void Update()
    {
        if(pauseAction.WasPressedThisFrame())
        {
            TogglePause();
        }
    }



    void TogglePause()
    {
        if(isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;


        pauseMenu.SetActive(true);


        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;


        pauseMenu.SetActive(false);


        Time.timeScale = 1f;
    }
}
