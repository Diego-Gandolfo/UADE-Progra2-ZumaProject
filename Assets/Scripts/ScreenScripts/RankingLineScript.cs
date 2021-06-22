using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Orders
{
    First,
    Second,
    Third,
    Fourth,
    Fifth
}

public class RankingLineScript : MonoBehaviour
{
    [SerializeField] private Orders order;
    [SerializeField] private int number;
    [SerializeField] private Text nickname;
    [SerializeField] private Text puesto;
    [SerializeField] private Text score;
    [SerializeField] private Text time;
    [SerializeField] private Image bg;
    [SerializeField] private Color newColor;
    private string position;

    public void SetPosition(int value = 0)
    {
        //switch (order)
        //{
        //    case Orders.First:
        //        position = "1";
        //        break;
        //    case Orders.Second:
        //        position = "2";
        //        break;
        //    case Orders.Third:
        //        position = "3";
        //        break;
        //    case Orders.Fourth:
        //        position = "4";
        //        break;
        //    case Orders.Fifth:
        //        position = "5";
        //        break;
        //}
            puesto.text = value.ToString();
    }

    public void SetNickname(string name)
    {
        nickname.text = name;
    }

    public void SetScore(int number)
    {
        score.text = number.ToString();
    }

    public void SetTime(string time)
    {
        this.time.text = time + "s";
    }

    public void SetRanking (int number)
    {
        puesto.text = number.ToString();
    }

    public void ChangeBackground()
    {
        bg.color = newColor;
    }
}
