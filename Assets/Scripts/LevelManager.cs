using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string currentLevel;
    [SerializeField] private string nextLevel;
    [SerializeField] private Text textTimeCounter = null;
    [SerializeField] private float gameDuration = 0f;
    private float timeCounter = 0f;

    [SerializeField] private int numberLevel;
    private float currentTimer = 0f;
    private DBController database;

    private void Start()
    {
        GameManager.instance.CurrentLevel = currentLevel;
        GameManager.instance.NextLevel = nextLevel;
        GameManager.instance.NumberLevel = numberLevel;
        timeCounter = gameDuration;

        database = DBController.Instance;
    }

    private void Update()
    {
        timeCounter -= Time.deltaTime;
        currentTimer += Time.deltaTime;
        string msg = string.Format("{0:00.00}", timeCounter);
        textTimeCounter.text = msg;

        if (timeCounter <= 0)
        {
            Victory();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerGlobal.Instance.Id = 1; //TODO ESTO ES TEMPORAL, SOLO QUEDARYA EL VICTORY AL FINAL
            currentTimer = 100f; 
            GameManager.instance.CurrentScore = 7143;
            numberLevel = 1;
            Victory();
        }
    }

    private void Victory()
    {

        //InsertPlayerInRanking(GameManager.instance.CurrentScore, numberLevel, currentTimer); //Lo insertamos en el ranking

        GameManager.instance.Victory();
    }

    public void InsertPlayerInRanking(int score, int level, float time) //Esto se haria cuando se termina un nivel
    {
        PlayerGlobal.Instance.Level = level;
        PlayerGlobal.Instance.Score = score;
        PlayerGlobal.Instance.Time = time.ToString();

        var player = new Player(PlayerGlobal.Instance.Name, PlayerGlobal.Instance.Level, PlayerGlobal.Instance.Score);
        player.Time = PlayerGlobal.Instance.Time;
        database.InsertRanking(player);
        PlayerGlobal.Instance.RankingId = database.GetLatestRanking().RankingId; //Nos guardamos el ID de esa tabla para buscar más facil
    }
}
