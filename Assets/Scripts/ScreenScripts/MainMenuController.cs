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
    private PlayerGlobal player;
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

        database = DBController.instance;
        player = PlayerGlobal.instance;
        CheckPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenuCheck) //Esto es porque si no estan en uno de los sub menus, pueden volver para atras con Escape
        {
            OnGoBackHandler();
        }
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
            player.SetName(inputField.text);

            //INSERT PLAYER NO ME ACEPTA UN GLOBAL PLAYER... Y POR AHORA NO QUIERO CAMBIARLO
            Player player2 = new Player();
            player2.Name = player.Name;
            database.InsertPlayer(player2); //lo insertamos en la base
            player.SetId(database.GetLastPlayerId()); //Obtenemos el id del ultimo player insertado 
        }
        CheckPlayerName(); //Nos fijamos si tenemos que desaparecer la caja de nombres
    }

    private void CheckPlayerName()
    {
        if (player.Name == null)
        {
            nameBox.SetActive(false);
            inputBox.SetActive(true);
        }
        else
        {
            nameBox.GetComponent<Text>().text = player.Name;
            nameBox.SetActive(true);
            inputBox.SetActive(false);
        }
    }
}
