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
    [SerializeField] private Text nickname;
    [SerializeField] private Text puesto;
    [SerializeField] private Text score;
    [SerializeField] private Text time;
    private string position;

    private void Start()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        switch (order)
        {
            case Orders.First:
                position = "1st";
                break;
            case Orders.Second:
                position = "2nd";
                break;
            case Orders.Third:
                position = "3rd";
                break;
            case Orders.Fourth:
                position = "4th";
                break;
            case Orders.Fifth:
                position = "5th";
                break;
        }
        puesto.text = position;
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
        this.time.text = time;
    }
}
