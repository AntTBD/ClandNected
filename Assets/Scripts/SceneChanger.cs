using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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
        LoadMap("CableTest");
    }

    public void Credits()
    {
        LoadMap("Credits");
    }

    public void BackToMenu()
    {
        LoadMap("Menu");
    }
    
}
