using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public bool IsGameFreeze { get; set; }
    public int CurrentScore { get; set; }

    public int NumberLevel { get; set; }
    public string CurrentLevel { get; set; }
    public string NextLevel { get; set; }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        IsGameFreeze = false;
    }

    public void Victory()
    {
        SceneManager.LoadScene("Victory"); //Cargamos la escena
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
