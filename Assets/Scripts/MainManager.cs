using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    public static int m_Points;
    public static int highScore;
    public static string Leader_Name;
    private string previousName;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Awake()
    {
        LoadScore();        

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Start()
    {
        HighScoreText.gameObject.SetActive(false);
        HighScore();
        m_Points = 0;
    }

    private void Update()
    {
        if (Menu.Player_Name != previousName)
        {
            m_Points = 0;
            CurrentScore();
        }

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space) && SceneManager.sceneCount == 1)
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
        }

        //Enable only for testing
        //ResetHighScore(); 
    }

    void AddPoint(int point)
    {
        m_Points += point;
        CurrentScore();
    }

    public void CurrentScore()
    {
        if (Menu.Player_Name == null)
        {
            Menu.Player_Name = "TBD";
        }
        ScoreText.text = $"Player : {Menu.Player_Name}" + "  " + $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (highScore == 0 || highScore < m_Points)
        {
            highScore = m_Points;
            Leader_Name = Menu.Player_Name;
            HighScore();
        }

        Menu.Player_Name = previousName;
        SaveScore();

        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    private void HighScore()
    {
        if (highScore > 0) 
        {
            HighScoreText.gameObject.SetActive(true);
            HighScoreText.text = "Leader : " + Leader_Name + "  High Score : " + highScore;
        }        
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string Leader_Name;
    }

    public void SaveScore()
    {
        SaveData saveData = new SaveData();
        saveData.highScore = MainManager.highScore;
        saveData.Leader_Name = MainManager.Leader_Name;

        string Json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/SaveData.json", Json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            MainManager.highScore = saveData.highScore;
            MainManager.Leader_Name = saveData.Leader_Name;
        }
    }

    public void ResetHighScore()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            highScore = 0;
            Leader_Name = null;
            SaveScore();
            HighScore();
        }
    }
}
