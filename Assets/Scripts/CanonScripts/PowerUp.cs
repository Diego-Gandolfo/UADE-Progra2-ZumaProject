using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, IBall
{
    private QueueDynamicController queueDynamic;
    [SerializeField] private int ballToOrder;
    private TDA_ABB ballTree;

    public PowerUp(QueueDynamicController queueDynamic)
    {
        this.queueDynamic = queueDynamic;
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
                ReSortBalls(ball); //Reordenamos las pelotas
                queueDynamic.DesqueueMiddle(ball); //Sacamos el powerup del queue
                Destroy(collision.gameObject); //Destruimos el proyectil
                Destroy(gameObject); //Destruimos el powerup
            }
        }
    }

    public void SetQueueController(QueueDynamicController queueController)
    {
        this.queueDynamic = queueController;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void ReSortBalls(Ball ball)
    {
        var resortList = queueDynamic.DequeueList(ball, ballToOrder); //Se trae las pelotas en una lista.

        foreach (var item in resortList) //recorre la lista y las agrega una por una al arbol
        {
            ballTree.AgregarElem(ref ballTree.raiz, item as Ball);
        }

        ballTree.inOrder(ballTree.raiz, queueDynamic, ball); //acá las ordeno por color
    }
}
