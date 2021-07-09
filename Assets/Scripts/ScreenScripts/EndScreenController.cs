using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum ScreenType
{
    Victory, 
    GameOver
}

public class EndScreenController : MonoBehaviour
{
    [SerializeField] private ScreenType screen;

    [Header("Buttons")]
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;

    void Start()
    {
        PlayerGlobal.Instance.Score = 0; //Sea lo que sea reseteemos el puntaje del player.
        GameManager.instance.CurrentScore = 0; //Más limpieza del puntaje
        playAgainButton.onClick.AddListener(OnPlayAgainHandler);
        menuButton.onClick.AddListener(OnMenuHandler);
        exitButton.onClick.AddListener(OnQuitHandler);

        if (screen == ScreenType.Victory)
        {
            nextLevelButton.onClick.AddListener(OnNextLevelHandler);

            if (GameManager.instance.NextLevel == string.Empty)
            {
                nextLevelButton.interactable = false;
            }
        }

    }

    private void OnPlayAgainHandler()
    {
        AudioManager.instance.PlaySound(SoundClips.MouseClick);
        SceneManager.LoadScene(GameManager.instance.CurrentLevel);
    }

    private void OnMenuHandler()
    {
        AudioManager.instance.PlaySound(SoundClips.MouseClick);
        SceneManager.LoadScene("MainMenu");
    }

    private void OnNextLevelHandler()
    {
        AudioManager.instance.PlaySound(SoundClips.MouseClick);
        SceneManager.LoadScene(GameManager.instance.NextLevel);
    }

    private void OnQuitHandler()
    {
        AudioManager.instance.PlaySound(SoundClips.MouseClick);
        print("Se cierra el juego");
        Application.Quit();
    }
}
