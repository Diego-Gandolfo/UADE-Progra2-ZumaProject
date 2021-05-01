using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private bool isGameOver;

    private string currentLevel;
    private string nextLevel;

    void Start()
    {
        playAgainButton.onClick.AddListener(OnPlayAgainHandler);
        menuButton.onClick.AddListener(OnMenuHandler);
        exitButton.onClick.AddListener(OnQuitHandler);

        if (!isGameOver)
            nextLevelButton.onClick.AddListener(OnNextLevelHandler);
    }

    private void OnPlayAgainHandler()
    {
        Debug.Log("Reload Level");
        //SceneManager.LoadScene(currentLevel);
    }

    private void OnMenuHandler()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnNextLevelHandler()
    {
        Debug.Log("Load Next Level");
        //SceneManager.LoadScene(nextLevel);
    }

    private void OnQuitHandler()
    {
        Application.Quit();
        Debug.Log("Se cierra el juego");
    }

}
