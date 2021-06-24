using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTriggerScript : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null && !ball.IsProjectile)
        {
            print("perdi");
            gameManager.GameOver();
        }
    }
}
