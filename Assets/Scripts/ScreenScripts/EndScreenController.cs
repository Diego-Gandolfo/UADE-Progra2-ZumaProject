using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;

    [Header("Others")]
    [SerializeField] private bool isGameOver;

    [Header("Scenes")]
    [SerializeField] private string currentLevel = "Level01";
    [SerializeField] private string nextLevel = "Level02";

    void Start()
    {
        playAgainButton.onClick.AddListener(OnPlayAgainHandler);
        menuButton.onClick.AddListener(OnMenuHandler);
        exitButton.onClick.AddListener(OnQuitHandler);


        //if (GameManager.instance.NextLevel != string.Empty)
        if (!isGameOver)
            nextLevelButton.onClick.AddListener(OnNextLevelHandler);
    }

    private void OnPlayAgainHandler()
    {
        //SceneManager.LoadScene(currentLevel);
        SceneManager.LoadScene(GameManager.instance.CurrentLevel);
    }

    private void OnMenuHandler()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnNextLevelHandler()
    {
        //SceneManager.LoadScene(nextLevel);
        SceneManager.LoadScene(GameManager.instance.NextLevel);
    }

    private void OnQuitHandler()
    {
        Application.Quit();
        Debug.Log("Se cierra el juego");
    }
}
