using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, IBall
{
    private QueueDynamicController queueDynamic;
    private int ballToOrder;
    private TDA_ABB ballTree;

    private void Start()
    {
        ballTree = new TDA_ABB();
        ballTree.InicializarArbol();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            if (ball.IsProjectile)
            {
                ReSortBalls(this); //Reordenamos las pelotas
                queueDynamic.DesqueueMiddle(this); //Sacamos el powerup del queue
                Destroy(collision.gameObject); //Destruimos el proyectil
                queueDynamic.SetCanInstantiatePowerUp(true);
                Destroy(gameObject); //Destruimos el powerup
            }
        }
    }

    public void SetQueueController(QueueDynamicController queueController)
    {
        this.queueDynamic = queueController;
    }

    public void SetBallsToOrder(int number)
    {
        ballToOrder = number;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void ReSortBalls(IBall ball)
    {
        var resortList = queueDynamic.DequeueList(ball, ballToOrder); //Se trae las pelotas en una lista.

        foreach (var item in resortList) //recorre la lista y las agrega una por una al arbol
        {
            if (item is Ball)
                ballTree.AgregarElem(ref ballTree.raiz, item as Ball);
        }

        ballTree.inOrder(ballTree.raiz, queueDynamic, ball); //acá las ordeno por color
    }
}
