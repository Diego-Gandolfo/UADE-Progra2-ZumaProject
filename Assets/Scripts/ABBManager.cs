using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABBManager : MonoBehaviour
{
    [SerializeField] private QueueDynamicController queueDynamic;
    [SerializeField] private int ballToOrder;
    private TDA_ABB ballTree;
    public Ball ballPrefab;


    [Header("Raycast Settings")]
    [SerializeField] SpriteRenderer currentBall;
    private Transform raycastPoint;
    private Vector2 actualPositionMouse;
    private RaycastHit2D hit2D;
    private Vector2 direction;

    private void Start()
    {
        raycastPoint = currentBall.gameObject.transform;
        ballTree = new TDA_ABB();
        ballTree.InicializarArbol();
    }
    public Ball CreateBall() // Creamos una nueva instancia y nodo
    {
        var ball = Instantiate(this.ballPrefab); // instanciamos una nueva Sphere
        return ball; // devolvemos el clone creado
    }
    private void Update()
    {
        actualPositionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = actualPositionMouse - (Vector2)raycastPoint.position;
        direction.Normalize();
        transform.up = direction;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hit2D = Physics2D.Raycast(raycastPoint.position, direction);

            if (hit2D)
                ReSortBalls(hit2D.collider.GetComponent<Ball>());
        }
    }

    private void ReSortBalls(Ball ball)
    {
        var resortList = queueDynamic.DequeueList(ball, ballToOrder); //Se trae las pelotas en una lista.

        foreach (var item in resortList) //recorre la lista y las agrega una por una al arbol
        {
            ballTree.AgregarElem(ref ballTree.raiz, item);
        }

        ballTree.inOrder(ballTree.raiz, queueDynamic, ball); //acá las ordeno por color

        //foreach (var item in resortList) // recorro la lista y las vuelvo a meter en la cola una por una
        //{
        //    queueDynamic.EnqueueMiddleAfter(item, ball,false);
        //}  
    }

    //private void ReSortBalls(Ball ball)
    //{
    //    var resortList = queueDynamic.DequeueList(ball, ballToOrder); //Se trae las pelotas en una lista.

    //    foreach (var item in resortList) //recorre la lista y las agrega una por una al arbol
    //    {
    //        ballTree.AgregarElem(ref ballTree.raiz, item);
    //    }

    //    ballTree.inOrder(ballTree.raiz, resortList); //acá las ordeno por color

    //    foreach (var item in resortList) // recorro la lista y las vuelvo a meter en la cola una por una
    //    {
    //        queueDynamic.EnqueueMiddleAfter(item, ball,false);
    //    }  
    //}
}
