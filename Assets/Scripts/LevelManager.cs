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
    [SerializeField] private float gameDuration = 0f;
    [SerializeField] private Text textTimeCounter = null;

    [Header("Level Settings")]
    [SerializeField] private int numberLevel = 1;
    [SerializeField] private string currentLevel;
    [SerializeField] private string nextLevel;

    private float timeCounter = 0f;
	private float currentTimer = 0f;
	
	private DBController database;
    private IGrafosManager grafosManager;

    private void Start()
    {
		//GameManager.instance.CurrentLevel = currentLevel;
        //GameManager.instance.NextLevel = nextLevel;
        //GameManager.instance.NumberLevel = numberLevel;
        //timeCounter = gameDuration;

        grafosManager = gameObject.GetComponent<IGrafosManager>();
        queueController.Initialize(ballPrefab, movingTime, quantityBallsLevel, grafosManager, ballPointValue);
        queueController.PowerUpSettings(powerUpPrefab, playWithPowerUp, ballsToOrder, checkColorCountToPowerUp);

		database = DBController.Instance;
    }

    private void Update() //TEMPORALMENTE COMENTADO PARA TEST GRAFOS
    {
        //timeCounter -= Time.deltaTime;
        //currentTimer += Time.deltaTime;
        //string msg = string.Format("{0:00.00}", timeCounter);
        //textTimeCounter.text = msg;

        //if (timeCounter <= 0)  
        //{
        //    Victory();
        //}

        //if (Input.GetKeyDown(KeyCode.Space)) //ESTO ES TEMPORAL
        //{
        //    PlayerGlobal.Instance.Id = 1;
        //    currentTimer = 100f; 
        //    GameManager.instance.CurrentScore = 7143;
        //    numberLevel = 1;
        //    Victory();
        //}
    }

    private void Victory()
    {
        //InsertPlayerInRanking(GameManager.instance.CurrentScore, numberLevel, currentTimer); //Lo insertamos en el ranking -  TEMPORALMENTE COMENTADO

        PlayerGlobal.Instance.RankingId = 39; //TODO: Para temporal con lo que ya esta en la base de datos...
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
