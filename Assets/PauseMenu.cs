using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Allows for the game to be paused using SPACE or ESC
    /// </summary>
    public static bool gameIsPaused;
    public Canvas PauseController;
    public GameObject PauseOverlay;
    public GameObject ContButton;

    void Start()
    {
        //PauseController = GameObject.Find("PauseControlCanvas").GetComponent<Canvas>();
        //PauseController.enabled = false;
        PauseOverlay = GameObject.Find("Pause Background");
        PauseOverlay.SetActive(false);
        gameIsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseTheGame();
        }
    }

    public void PauseTheGame()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            Debug.Log("Game Paused");
            Time.timeScale = 0;
            PauseOverlay.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Debug.Log("Resume Game");
        Time.timeScale = 1;
        PauseOverlay.SetActive(false);
    
    }

    public static void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
        #else
        Application.Quit();
        #endif
    }
}
