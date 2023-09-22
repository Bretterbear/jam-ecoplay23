using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    /// <summary>
    /// Allows for the game to be paused using SPACE or ESC
    /// </summary>
    public static bool gameIsPaused;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
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
        }else
        {
            ResumeGame();
        }
 
    }

    void ResumeGame()
    {
        Debug.Log("Resume Game");
        Time.timeScale = 1;
    }
}