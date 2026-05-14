using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private const float TRANSITION_DURATION = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;                  
        GameIsPaused = false;                 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;                    
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        StartCoroutine(FreezeAfterTransition());
    }

    private IEnumerator FreezeAfterTransition()
    {
        yield return new WaitForSecondsRealtime(TRANSITION_DURATION);
        Time.timeScale = 0f;
    }

    public void PauseButton()
    {
        if (!GameIsPaused) Pause();
        else Resume();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}