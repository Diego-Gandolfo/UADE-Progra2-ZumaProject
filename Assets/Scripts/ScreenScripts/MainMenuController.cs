using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("AllMenus Settings")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsMenu;

    [Header("MainMenu Settings")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject inputBox;
    [SerializeField] private Button confirmButton;
    [SerializeField] private InputField inputField;
    [SerializeField] private GameObject nameBox;

    private bool mainMenuCheck;
    private string level01 = "Level01";
    private DBController database;

    [Header("Credits Settings")]
    [SerializeField] private Button goBackCreditsButton;


    void Start()
    {
        playButton.onClick.AddListener(OnPlayHandler);
        creditsButton.onClick.AddListener(OnCreditsHandler);
        goBackCreditsButton.onClick.AddListener(OnGoBackHandler);
        exitButton.onClick.AddListener(OnQuitHandler);
        confirmButton.onClick.AddListener(OnConfirmHandler);
        OnGoBackHandler(); //Si o si, aseguramos que inicie en el MenuInicial

        database = DBController.instance; //Necesario para conectar a la base
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenuCheck) //Esto es porque si no estan en uno de los sub menus, pueden volver para atras con Escape
        {
            OnGoBackHandler();
        }

        #region DATABASE_TESTING 
        //TODO: ESTO ES TEMPORAL, NO TIENE QUE IR ACÁ
        if (Input.GetKeyDown(KeyCode.F3))
        {
            var trial = new Player(PlayerGlobal.instance.Name, UnityEngine.Random.Range(1, 5), UnityEngine.Random.Range(1, 10000));
            trial.Time = UnityEngine.Random.Range(1, 1000).ToString();
            trial.Id = PlayerGlobal.instance.Id;

            database.InsertRanking(trial);

            var ranking = database.GetLatestRanking();
            print($"Ranking {ranking.Name}, Nivel: {ranking.Level} Score: {ranking.Score} Time: {ranking.Time}");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            print("Generando");
            var trial2 = new Player();
            database.InsertPlayer(trial2); //lo insertamos en la base
            trial2.Id = database.GetLastPlayerId(); //Obtenemos el id del ultimo player insertado 
            print($"{trial2.Id} {trial2.Name} {trial2.Level} {trial2.Score} {trial2.Time}");
            database.InsertRanking(trial2);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            print("AllRanking");
            var rankings = database.GetAllRankings();
            foreach (var ranking in rankings)
            {
                print($"Ranking {ranking.Name}, Nivel: {ranking.Level} Score: {ranking.Score} Time: {ranking.Time}");
            }
        }
        #endregion
    }

    private void OnPlayHandler()
    {
        SceneManager.LoadScene(level01);
    }

    private void OnCreditsHandler()
    {
        mainMenu.SetActive(false);
        //helpMenu.SetActive(false);
        creditsMenu.SetActive(true);
        mainMenuCheck = false;
    }

    private void OnGoBackHandler()
    {
        mainMenu.SetActive(true);
        //helpMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenuCheck = true;
    }

    private void OnQuitHandler()
    {
        Application.Quit();
        Debug.Log("Se cierra el juego");
    }

    private void OnConfirmHandler()
    {
        if(inputField.text != null)
        {
            PlayerGlobal.instance.Name = inputField.text;

            //TODO: INSERT PLAYER NO ME ACEPTA UN GLOBAL PLAYER... Y POR AHORA NO QUIERO CAMBIARLO
            Player player2 = new Player();
            player2.Name = PlayerGlobal.instance.Name;

            //TODO: CUANDO SE SOLUCIONE EL TEMA DEL SCORE EN GENERAL, SE DESCOMENTA ESTO ASI NO CREAMOS JUGADORES VACIOS
            //database.InsertPlayer(player2); //lo insertamos en la base
            //player.Id = database.GetLastPlayerId(); //Obtenemos el id del ultimo player insertado 
            print(PlayerGlobal.instance.Name + " " + PlayerGlobal.instance.Id);
            CheckPlayerName(); //Nos fijamos si tenemos que desaparecer la caja de nombres
        }
    }

    private void CheckPlayerName()
    {
        if (PlayerGlobal.instance.Name == null)
        {
            nameBox.SetActive(false);
            inputBox.SetActive(true);
        }
        else
        {
            nameBox.GetComponent<Text>().text = PlayerGlobal.instance.Name;
            nameBox.SetActive(true);
            inputBox.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CheckPlayerName();
        print(PlayerGlobal.instance.Name);
        print("probando");
    }
}
