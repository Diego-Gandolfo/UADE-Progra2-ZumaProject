using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("AllMenus Settings")]
    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private GameObject creditsMenu = null;

    [Header("MainMenu Settings")]
    [SerializeField] private Button playButton = null;
    [SerializeField] private Button creditsButton = null;
    [SerializeField] private Button exitButton = null;
    [SerializeField] private GameObject inputBox = null;

    [Header("Name Settings")]
    [SerializeField] private ScoreManager scoreManager = null;
    [SerializeField] private Button confirmButton = null;
    [SerializeField] private InputField inputField = null;
    [SerializeField] private GameObject nameBox = null;

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

        database = DBController.Instance; //Necesario para conectar a la base
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenuCheck) //Esto es porque si no estan en uno de los sub menus, pueden volver para atras con Escape
        {
            OnGoBackHandler();
        }
    }

    private void OnEnable()
    {
        CheckPlayerName();
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
            scoreManager.InsertPlayer(inputField.text);
            CheckPlayerName(); //Nos fijamos si tenemos que desaparecer la caja de nombres
        }
    }

    private void CheckPlayerName()
    {
        if (PlayerGlobal.Instance.Name == null)
        {
            nameBox.SetActive(false);
            inputBox.SetActive(true);
        }
        else
        {
            nameBox.GetComponent<Text>().text = PlayerGlobal.Instance.Name;
            nameBox.SetActive(true);
            inputBox.SetActive(false);
        }
    }
}
