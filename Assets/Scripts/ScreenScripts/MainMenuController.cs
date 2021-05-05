using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("AllMenus Settings")]
    [SerializeField] private GameObject mainMenu;
    //[SerializeField] private GameObject helpMenu;
    [SerializeField] private GameObject creditsMenu;

    [Header("MainMenu Settings")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    private bool mainMenuCheck;
    private string level01 = "Level01";

    [Header("Credits Settings")]
    [SerializeField] private Button goBackCreditsButton;


    void Start()
    {
        playButton.onClick.AddListener(OnPlayHandler);
        creditsButton.onClick.AddListener(OnCreditsHandler);
        goBackCreditsButton.onClick.AddListener(OnGoBackHandler);
        exitButton.onClick.AddListener(OnQuitHandler);

        OnGoBackHandler(); //Si o si, aseguramos que inicie en el MenuInicial
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
}
