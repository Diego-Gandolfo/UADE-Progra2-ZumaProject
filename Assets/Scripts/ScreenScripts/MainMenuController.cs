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


    [Header("Name Settings")]
    [SerializeField] private InsertPlayerManager dbManager = null;
    [SerializeField] private Button confirmButton = null;
    [SerializeField] private GameObject inputBox = null;
    [SerializeField] private InputField inputField = null;
    [SerializeField] private GameObject nameBox = null;
    [SerializeField] private Text nameText = null;

    private bool firstTime = true;
    private bool mainMenuCheck;
    [SerializeField] private int level01 = 1;
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
            OnGoBackHandler();
    }

    private void OnEnable()
    {
        CheckPlayerName();
    }

    private void OnPlayHandler()
    {
        AudioManager.instance.PlaySound(SoundClips.MouseClick);
        SceneManager.LoadScene(level01);
    }

    private void OnCreditsHandler()
    {
        AudioManager.instance.PlaySound(SoundClips.MouseClick);
        mainMenu.SetActive(false);
        //helpMenu.SetActive(false);
        creditsMenu.SetActive(true);
        mainMenuCheck = false;
    }

    private void OnGoBackHandler()
    {
        if (!firstTime) AudioManager.instance.PlaySound(SoundClips.MouseClick);
        mainMenu.SetActive(true);
        //helpMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenuCheck = true;
        firstTime = false;
    }

    private void OnQuitHandler()
    {
        AudioManager.instance.PlaySound(SoundClips.MouseClick);
        Application.Quit();
        Debug.Log("Se cierra el juego");
    }

    private void OnConfirmHandler()
    {
        AudioManager.instance.PlaySound(SoundClips.MouseClick);
        if (inputField.text != null)
        {
            dbManager.InsertPlayer(inputField.text);
            CheckPlayerName(); //Nos fijamos si tenemos que desaparecer la caja de nombres
        }
    }

    private void CheckPlayerName()
    {
        if (PlayerGlobal.Instance.Name == null)
        {
            nameBox.SetActive(false);
            inputBox.SetActive(true);
            playButton.interactable = false; ;
        }
        else
        {
            nameText.text = PlayerGlobal.Instance.Name;
            playButton.interactable = true;
            nameBox.SetActive(true);
            inputBox.SetActive(false);
        }
    }
}
