using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABBManager : MonoBehaviour
{
    [SerializeField] private int ballToOrder;
    private ABBBall ballTree;
    public Ball ballPrefab;
    [SerializeField]private List<ABBNode> abbList = new List<ABBNode>();
    [SerializeField]private List<Ball> ballList = new List<Ball>();
    [SerializeField]private QueueDynamicController queueDynamic;

    [Header("Raycast Settings")]
    [SerializeField] SpriteRenderer currentBall;
    [SerializeField] private float rayLenght = 5f;
    private Transform raycastPoint;
    private Vector2 actualPositionMouse;
    private RaycastHit2D hit2D;
    private LineRenderer laser;
    private Vector2 direction;

    private void Start()
    {
        raycastPoint = currentBall.gameObject.transform;
        ballTree = new ABBBall();
        ballTree.InicializarArbol();

        //for (int i = 0; i < 10; i++)
        //{

        //    ballTree.AgregarElem(ref ballTree.raiz, CreateBall());
            
        //}
        //
        //for (int i = 0; i < ballList.Count; i++)
        //{
        //    ballList[i].QueueController.EnqueueTop();
        //}
        //foreach (var item in abbList)
        //{
        //    print(item.info.ColorValue());
        //}
        //ShowQueue();
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
                BallDequeue(hit2D.collider.GetComponent<Ball>());
        }

    }
    private void BallDequeue(Ball ball)
    {
        var ballDequeue = queueDynamic.DequeueList(ball, ballToOrder);
        foreach (var item in ballDequeue)
        {
            ballTree.AgregarElem(ref ballTree.raiz, item);
        }
        ballTree.inOrder(ballTree.raiz, ballDequeue);
        foreach (var item in ballDequeue)
        {
            queueDynamic.EnqueueMiddleBefore(item, ball,false);
        }
        //todo: Cambiar nombre a otra cosa please.  
    }
}
