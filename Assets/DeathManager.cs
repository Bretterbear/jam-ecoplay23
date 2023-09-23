using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    private SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        sceneLoader = GameObject.Find("SceneController").GetComponent<SceneLoader>();
    }

    public void Retry()
    {
        sceneLoader.LoadLevel();
    }

    public void Quit()
    {
        SceneLoader.Quit();
    }

    public void MainMenuCaller()
    {
        sceneLoader.GoToTitle();
    }
}
