using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    /// <summary>
    /// Allows for the game to be paused using SPACE or ESC
    /// </summary>
    public static bool gameIsPaused;
    public Canvas PauseController;
    public GameObject PauseOverlay;

    void Start()
    {
        PauseController = GameObject.Find("PauseControlCanvas").GetComponent<Canvas>();
        PauseController.enabled = false;
        //PauseOverlay = GameObject.Find("PauseControlCanvas");
        //PauseOverlay.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseTheGame();
        }
    }

    void PauseTheGame()
    {
        if(gameIsPaused)
        {
            Debug.Log("Game Paused");
            Time.timeScale = 0;
            PauseController.enabled = true;
            //PauseOverlay.SetActive(true);
        }else
        {
            ResumeGame();
        }
     }

    void ResumeGame()
    {
        Debug.Log("Resume Game");
        Time.timeScale = 1;
        PauseController.enabled = false;
        //PauseOverlay.SetActive(false);
    }
}