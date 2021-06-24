using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Queue Settings")]
    [SerializeField] private QueueDynamicController queueController;
    [SerializeField] private Ball ballPrefab = null;
    [SerializeField] private int quantityBallsLevel;
    [SerializeField] private int ballPointValue = 10;
    [SerializeField] private float movingTime;

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

    //private float timeCounter = 0f;
    private TimeSpan timeInSeconds;
	
	private DBController database;
    private IGrafosManager grafosManager;

    private void Start()
    {
        GameManager.instance.CurrentLevel = currentLevel;
        GameManager.instance.NextLevel = nextLevel;
        GameManager.instance.NumberLevel = numberLevel;
        //timeCounter = gameDuration;

        grafosManager = gameObject.GetComponent<IGrafosManager>();
        queueController.Initialize(ballPrefab, movingTime, quantityBallsLevel, grafosManager, ballPointValue);
        queueController.PowerUpSettings(powerUpPrefab, playWithPowerUp, ballsToOrder, checkColorCountToPowerUp);

		database = DBController.Instance;
    }

    private void Update()
    {
        //timeCounter -= Time.deltaTime;
        timeInSeconds += TimeSpan.FromSeconds(Time.deltaTime); ;

        //ACTUALIZACION DEL HUD
        hudManager.SetCurrentStack(canonStack.GetIndex());
        hudManager.SetScore(GameManager.instance.CurrentScore);
        hudManager.SetTimer(timeInSeconds);


        if (queueController.IsEmpty()) //Condicion Victoria: si la cola queda vacia
        {
            Victory();
        }
    }

    private void Victory()
    {
        InsertPlayerInRanking(GameManager.instance.CurrentScore, numberLevel, timeInSeconds); //Lo insertamos en el ranking -  TEMPORALMENTE COMENTADO
        GameManager.instance.Victory();
    }

    public void InsertPlayerInRanking(int score, int level, TimeSpan ts) //Esto se haria cuando se termina un nivel
    {
        PlayerGlobal.Instance.Level = level;
        PlayerGlobal.Instance.Score = score;
        PlayerGlobal.Instance.Time = string.Format("{0:00}:{1:00}", (int)ts.TotalMinutes, (int)ts.Seconds);

        var player = new Player(PlayerGlobal.Instance.Name, PlayerGlobal.Instance.Level, PlayerGlobal.Instance.Score);
        player.Time = PlayerGlobal.Instance.Time;
        database.InsertRanking(player);
        PlayerGlobal.Instance.RankingId = database.GetLatestRanking().RankingId; //Nos guardamos el ID de esa tabla para buscar más facil
    }
}
