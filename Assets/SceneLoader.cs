using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{

    static GameObject sceneController;
    // Start is called before the first frame update
    void Start()
    {
        // Only have 1 scene controller
        if (sceneController != null)
        {
            Destroy(this.gameObject);
            return;
        }

        sceneController = GameObject.Find("SceneController");
        DontDestroyOnLoad(sceneController);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.End))
        {
            GameOver();
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }

    public void Winner()
    {
        SceneManager.LoadScene(3);
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
