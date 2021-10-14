using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string SCENE_GAME = "CableTest";
    public string SCENE_MENU = "Menu";
    public string SCENE_CREDITS = "Credits";
    public string SCENE_END = "End";

    public void LoadMap(String sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void Play()
    {
        LoadMap(SCENE_GAME);
    }

    public void Credits()
    {
        LoadMap(SCENE_CREDITS);
    }

    public void BackToMenu()
    {
        LoadMap(SCENE_MENU);
    }
    public void GameOver()
    {
        GameObject dataSaver = GameObject.Find("DataSaver");
        if (dataSaver != null) dataSaver.GetComponent<DataSaver>().SaveValues();

        LoadMap(SCENE_END);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == SCENE_GAME)
        {
            if (Input.GetKeyDown(KeyCode.Escape))// echap = gameover
                GameOver();
        }
    }

}
