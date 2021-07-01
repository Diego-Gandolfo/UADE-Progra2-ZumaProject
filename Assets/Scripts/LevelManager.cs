using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Queue Settings")]
    [SerializeField] private QueueDynamicController[] queueControllers = new QueueDynamicController[0];
    [SerializeField] private Ball ballPrefab = null;
    [SerializeField] private int quantityBallsLevel;
    [SerializeField] private int ballPointValue = 10;
    [SerializeField] private float movingTime;
    int currentNumber = 1;
    int numberOfEmptyQueues = 0;

    [Header("PowerUp Settings")]
    [SerializeField] private bool playWithPowerUp = false;
    [SerializeField] private PowerUp powerUpPrefab = null;
    [SerializeField] private int checkColorCountToPowerUp = 1;
    [SerializeField] private int ballsToOrder = 1;

    [Header("HUD Settings")]
    //[SerializeField] private float gameDuration = 0f;
    [SerializeField] private HUDManager hudManager = null;
    [SerializeField] private CanonStack canonStack = null;

    [Header("Level Settings")]
    [SerializeField] private int numberLevel = 1;
    [SerializeField] private string currentLevel;
    [SerializeField] private string nextLevel;

    private float timeCounter = 0f;
    private TimeSpan timeInSeconds;
	
	private DBController database;
    private IGrafosManager grafosManager;

    private void Start()
    {
        GameManager.instance.CurrentLevel = currentLevel;
        GameManager.instance.NextLevel = nextLevel;
        GameManager.instance.NumberLevel = numberLevel;

        grafosManager = gameObject.GetComponent<IGrafosManager>();

        foreach (var queueController in queueControllers)
        {

            queueController.Initialize(ballPrefab, movingTime, quantityBallsLevel, grafosManager, currentNumber, ballPointValue);
            queueController.PowerUpSettings(powerUpPrefab, playWithPowerUp, ballsToOrder, checkColorCountToPowerUp);
            queueController.OnEmpty.AddListener(OnEmptyCheckVictory);
            currentNumber++;
        }

		database = DBController.Instance;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        timeInSeconds += TimeSpan.FromSeconds(Time.deltaTime);

        //ACTUALIZACION DEL HUD
        hudManager.SetCurrentStack(canonStack.GetIndex());
        hudManager.SetScore(GameManager.instance.CurrentScore);
        hudManager.SetTimer(timeInSeconds);


        if (Input.GetKeyDown(KeyCode.Space))
            Victory();
    }

    private void Victory()
    {
        if(PlayerGlobal.Instance.Id == 0)
        {
            PlayerGlobal.Instance.Id = UnityEngine.Random.Range(1, 20);
            GameManager.instance.CurrentScore = UnityEngine.Random.Range(200, 500);
            timeInSeconds = TimeSpan.FromSeconds(UnityEngine.Random.Range(1, 120));
        }

        InsertPlayerInRanking(GameManager.instance.CurrentScore, numberLevel, timeInSeconds); //Lo insertamos en el ranking -  TEMPORALMENTE COMENTADO
        GameManager.instance.Victory();
    }

    public void InsertPlayerInRanking(int score, int level, TimeSpan ts) //Esto se haria cuando se termina un nivel
    {
        PlayerGlobal.Instance.Level = level;
        PlayerGlobal.Instance.Score = score;
        PlayerGlobal.Instance.Time = string.Format("{0:00}:{1:00}", (int)ts.TotalMinutes, (int)ts.Seconds);
        Player player = new Player(PlayerGlobal.Instance.Name, PlayerGlobal.Instance.Level, PlayerGlobal.Instance.Score);
        player.Time = PlayerGlobal.Instance.Time;
        player.Id = PlayerGlobal.Instance.Id;

        //print($"ID: {player.Id}, {player.Name}, time {player.Time}, score {player.Score}, level {player.Level} ");

        database.InsertRanking(player);
        PlayerGlobal.Instance.RankingId = database.GetLatestRanking().RankingId; //Nos guardamos el ID de esa tabla para buscar más facil
        print("Ranking Id: " + PlayerGlobal.Instance.RankingId);
    }

    private void OnEmptyCheckVictory()
    {
        numberOfEmptyQueues++;
        if (numberOfEmptyQueues == queueControllers.Length)
            Victory();
    }
}
