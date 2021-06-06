using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private DBController database;

    void Start()
    {
        database = DBController.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player player = new Player();
            print(player.Name);
            database.InsertPlayer(player);
        }
    }
}
