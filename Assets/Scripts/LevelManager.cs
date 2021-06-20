using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private QueueDynamicController queueController;
    [SerializeField] private int quantityBallsLevel;
    [SerializeField] private float speed;

    [SerializeField] private string currentLevel;
    [SerializeField] private string nextLevel;
    [SerializeField] private Text textTimeCounter = null;
    [SerializeField] private float gameDuration = 0f;
    private float timeCounter = 0f;
    private GameManager gameManager;
    private IGrafosManager grafosManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        //gameManager.CurrentLevel = currentLevel;
        //gameManager.NextLevel = nextLevel;
        //timeCounter = gameDuration;
        grafosManager = gameObject.GetComponent<IGrafosManager>();
        queueController.Initialize(speed, quantityBallsLevel, grafosManager);
    }

    private void Update()
    {
        //timeCounter -= Time.deltaTime;
        //string msg = string.Format("{0:00.00}", timeCounter);
        //textTimeCounter.text = msg;

        //if (timeCounter <= 0)
        //{
        //    gameManager.Victory();
        //}

        //TODO: Crear CONDICION DE VICTORIA cuando se explotaron todas las pelotas de la cola
    }
}
