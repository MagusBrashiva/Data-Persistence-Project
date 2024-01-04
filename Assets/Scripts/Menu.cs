using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

//[DefaultExecutionOrder(1000)]
public class Menu : MonoBehaviour
{
    public TMP_InputField nameField;
    public static string Player_Name;

    private void Start()
    {
        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        GameObject startButton = GameObject.Find("Start Button");
        GameObject resumeButton = GameObject.Find("Resume Button");

        resumeButton.SetActive(false);

        
        if (MainManager.m_Points > 0)
        {
            startButton.SetActive(false);
            resumeButton.SetActive(true);
        }
    }

    public void LoadGame()
    {
        SceneManager.UnloadSceneAsync(0);
    }

    public void ResumeGame()
    {
        SceneManager.UnloadSceneAsync(0);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void SaveName()
    {        
        Player_Name = nameField.text;
    }
}
