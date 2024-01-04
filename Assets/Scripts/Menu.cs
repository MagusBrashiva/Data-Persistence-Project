using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    public TMP_InputField nameField;
    public static string Player_Name;
    public GameObject startButton;
    public GameObject resumeButton;

    private void Start()
    {
        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        startButton = GameObject.Find("Start Button");
        resumeButton = GameObject.Find("Resume Button");

        resumeButton.SetActive(false);        
    }

    public void Update()
    {
        if (MainManager.m_Points > 0 && MainManager.m_GameOver == false)
        {
            startButton.SetActive(false);
            resumeButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(true);
            resumeButton.SetActive(false);
        }
    }

    public void LoadGame()
    {
        MainManager.m_Name = Player_Name;
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
