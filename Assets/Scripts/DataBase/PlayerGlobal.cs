using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobal : MonoBehaviour
{
    private static PlayerGlobal instance = null;
    static public PlayerGlobal Instance => instance;

    public int RankingId { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
    public int Level { get; set; }
    public int Score { get; set; }
    public string Time { get; set; }

    public void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}