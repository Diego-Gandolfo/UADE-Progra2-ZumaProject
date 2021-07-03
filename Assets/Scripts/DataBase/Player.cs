using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Name { get; set; }
    public int Id { get; set; }
    public int RankingId { get; set; }
    public int Level { get; set; }
    public int Score { get; set; }
    public string Time { get; set; }

    public Player(string name, int level, int score)
    {
        this.Name = name;
        this.Level = level;
        this.Score = score;
    }

    public Player()
    {
        var nameCollection = new NamesBaseCollection();
        this.Name = nameCollection.GetRandomName();
        //this.Level = Random.Range(1, 2);
        this.Level = 1;
        this.Score = Random.Range(0, 1000000);
        this.Time = Random.Range(10, 100).ToString();
    }
}
