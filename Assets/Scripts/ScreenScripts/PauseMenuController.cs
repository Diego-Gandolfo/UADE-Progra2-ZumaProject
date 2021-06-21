using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("AllMenus Settings")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private string currentLevel;
    
    /* SE VAN A DESCOMENTAR CUANDO ESTEN ARMADOS */
    //[SerializeField] private GameManager gameManager;
    //[SerializeField] private AudioSource musicLevel = null;
    //[SerializeField] private float lowerVolume = 1f;

    [Header("PauseMenu Settings")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    [Header("Hidables Settings")]
    [SerializeField] private GameObject timerObject;

    //Extras
    private bool isActive;

    void Start()
    {
        resumeButton.onClick.AddListener(OnResumeHandler);
        restartButton.onClick.AddListener(OnRestartHandler);
        mainMenuButton.onClick.AddListener(OnMenuHandler);
        quitButton.onClick.AddListener(OnQuitHandler);
        ExitMenu();

        //print("ESC para PauseMenu, F2 para GameOver");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActive)
            {
                Pause();
            }
            else
            {
                ExitMenu();
            }
        }

        //if (Input.GetKeyDown(KeyCode.F1))
        //    SceneManager.LoadScene("Victory");

        if (Input.GetKeyDown(KeyCode.F2))
            SceneManager.LoadScene("GameOver");

    }

    private void Pause()
    {
        Time.timeScale = 0;
        //gameManager.isFreeze = true;
        isActive = true;
        pauseMenu.SetActive(true);
        //musicLevel.volume -= lowerVolume;
        timerObject.SetActive(false);
    }

    private void ExitMenu()
    {
        Time.timeScale = 1;
        //gameManager.isFreeze = false;
        isActive = false;
        pauseMenu.SetActive(false);
        //musicLevel.volume += lowerVolume;
        timerObject.SetActive(true);
    }

    private void OnResumeHandler()
    {
        ExitMenu();
    }

    private void OnRestartHandler()
    {
        SceneManager.LoadScene(currentLevel);
    }

    private void OnMenuHandler()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnQuitHandler()
    {
        Application.Quit();
        Debug.Log("Se cierra el juego");
    }

}
