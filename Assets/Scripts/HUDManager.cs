using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject[] stackList = new GameObject[5]; 

    public void SetTimer(TimeSpan ts)
    {
        timerText.text = string.Format("{0:00}:{1:00}", (int)ts.TotalMinutes, (int)ts.Seconds);
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetCurrentStack(int number)
    {
        foreach (var stack in stackList) //Primero vaciamos
        {
            stack.SetActive(false);
        }

        for (int i = 0; i < number; i++) //Y ahora activamos la cantidad correcta
        {
            stackList[i].SetActive(true);
        }
    }

}
