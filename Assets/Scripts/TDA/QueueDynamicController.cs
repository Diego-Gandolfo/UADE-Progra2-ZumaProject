using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueDynamicController : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab = null;
    private QueueDymamic queueDynamic = null;
    private IGrafosManager grafosManager;
    private Transform[] path;

    private int ballPointValue;

    private int maxQuantity;
    private int currentQuantity;
    private int currentIndex = 1;

    private bool canInitializeMoving = true;

    private float ballSpawnTimer = 0f;
    private float movingTimer;
    private float movingCountdown = 0f;

    private void Awake()
    {
        queueDynamic = gameObject.GetComponent<QueueDymamic>();
    }

    private void Update() 
    {
        if (currentQuantity < maxQuantity) //Esto se usa en el inicio para rapidamente crear la cola y luego nunca más.
        {
            ballSpawnTimer += Time.deltaTime;
            CreateAllQueue();
        }

        movingCountdown += Time.deltaTime;
        if (movingCountdown >= movingTimer && canInitializeMoving) //Esto es para inicializar el movimiento de la cola
        {
            if(currentIndex != maxQuantity)
            {
                ShowQueue(currentIndex);
                currentIndex++;
                movingCountdown = 0f;
            } else
            {
                canInitializeMoving = false;
            }
        }

        if (movingCountdown >= movingTimer && !canInitializeMoving)
        {
            ShowQueue(GetNumberOfCurrentBalls());
            movingCountdown = 0f;
        }

    }

    public void Initialize(float timer, int maxQuantity, IGrafosManager grafosManager, int ballPointValue)
    {
        this.maxQuantity = maxQuantity;
        this.grafosManager = grafosManager;
        this.ballPointValue = ballPointValue;
        this.movingTimer = timer;
        path = grafosManager.GetDijkstra(0);
        print(path.Length);
    }

    public Ball CreateBall() // Creamos una nueva instancia y nodo
    {
        var ball = Instantiate(this.ballPrefab); // instanciamos una nueva Sphere
        ball.name = $"QueueController Ball ({currentQuantity})"; // le cambiamos el nombre para diferenciarlas
        ball.SetQueueController(this);

        var ballShowQueue = ball.GetComponent<BallShowQueue>();
        ballShowQueue.InitializePath(path, false);
        return ball; // devolvemos el clone creado
    }

    public void CreateAllQueue()
    {
        if (ballSpawnTimer >= (0.01))
        {
            if (queueDynamic.IsEmpty())
            {
                var ball = CreateBall();
                queueDynamic.Initialize(ball);
                var node = FindNode(ball);
                ball.BallSQ.Node = node;
            }
            else
            {
                EnqueueTop();
            }
            ballSpawnTimer = 0;
            currentQuantity++;
        }
    }

    public void ShowQueue(int index)
    {
        int auxIndex = index; 
        
        var auxNodeSupp = GetRootNode();
        while (auxNodeSupp.nextNode != null) //esto es para obtener el primero de la cola dinamica. 
        {
            auxNodeSupp = auxNodeSupp.nextNode;
        }

        for (int i = 0; i < auxIndex; i++)
        {
            auxNodeSupp.element.BallSQ.Move();
            auxNodeSupp = auxNodeSupp.previousNode; // guardamos el anterior en auxNode y repetimos
        }
    }

    public void ShowQueueOLD()
    {
        //if (!queueDynamic.IsEmpty())
        //{
        //    NodeBall auxNode = queueDynamic.rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode
        //    int index = 0; // iniciamos el index

        //    // Para mostrar el Nodo Raíz
        //    if (auxNode != null) // si el auxNode es distinto de null
        //    {
        //        auxNode.element.transform.position = (transform.right * index) + transform.position; // lo movemos en x según el valor del index
        //        index++; // aumentamos el index
        //    }

        //    // Para mostrar el resto de los Nodos
        //    while (auxNode.nextNode != null) // nos fijamos si es el ultimo
        //    {
        //        auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
        //        auxNode.element.transform.position = (transform.right * index) + transform.position; // lo movemos en x según el valor del index
        //        index++; // aumentamos el index
        //    }
        //}
    }

    public void EnqueueTop()
    {
        var ball = CreateBall();
        queueDynamic.EnqueueTop(ball);
        var node = FindNode(ball);
        ball.BallSQ.Node = node;
    }

    public void EnqueueMiddleAfter(Ball newBall, Ball afterBall)
    {
        queueDynamic.EnqueueMiddleAfter(newBall, afterBall);
        newBall.BallSQ.GetTargetBallInfo(afterBall);
        EnqueueMiddleMain(newBall);
        newBall.BallSQ.MakeSpaceToRight();
    }

    public void EnqueueMiddleBefore(Ball newBall, Ball beforeBall)
    {
        queueDynamic.EnqueueMiddleBefore(newBall, beforeBall);
        newBall.BallSQ.GetTargetBallInfo(beforeBall);
        EnqueueMiddleMain(newBall);
        beforeBall.BallSQ.MakeSpaceToRight();
    }

    public void EnqueueMiddleMain(Ball newBall) // son cosas que hacen ambos EnqueueMiddle, para no repetir codigo
    {
        ShowQueue(GetNumberOfCurrentBalls());
        var node = FindNode(newBall);
        newBall.BallSQ.Node = node;
        CheckColors(node);
    }

    public void EnqueueBottom()
    {
        queueDynamic.EnqueueBottom(CreateBall());
    }

    public void DesqueueTop()
    {
        queueDynamic.DesqueueTop();
    }

    public void DesqueueBottom()
    {
        queueDynamic.DesqueueBottom();
    }

    public Ball DesqueueMiddle(Ball targetBall)
    {
        NodeBall node = FindNode(targetBall);
        
        var aux = queueDynamic.DesqueueMiddle(targetBall);
        
        targetBall = null;
        return aux;
    }

    // TODO: llevar a QueueDynamic
    public NodeBall FindNode(Ball ball) //RECIBE PELOTA Y BUSCA EL NODO
    {
        var auxNode = queueDynamic.rootNode;

        while (auxNode.element != ball && auxNode.nextNode != null)
        {
            auxNode = auxNode.nextNode;
        }
        
        if (auxNode.element == ball) //SI LO ENCUENTRA, COMPRUEBA COLOR
            return auxNode;
        else
            return null;
    }

    public void CheckColors(NodeBall auxNode)
    {
        NodeBall nextNode = null; //Se guarda la pelota siguiente de la ultima de la lista que coincide color
        NodeBall previousNode = null;
        var ballList = new List<Ball>(); //Lista de pelotas que coinciden color
        ballList.Add(auxNode.element); //Agregamos la original

        if (auxNode.nextNode != null)
        {
            var auxNodeSupp = auxNode.nextNode;

            while (auxNode.element.Color == auxNodeSupp.element.Color && auxNodeSupp.nextNode != null)
            {
                ballList.Add(auxNodeSupp.element);
                auxNodeSupp = auxNodeSupp.nextNode;
            }

            if (auxNode.element.Color == auxNodeSupp.element.Color)
                ballList.Add(auxNodeSupp.element);
            else
                nextNode = auxNodeSupp;
        }

        if (auxNode.previousNode != null)
        {
            var auxNodeSupp = auxNode.previousNode;
            while (auxNode.element.Color == auxNodeSupp.element.Color && auxNodeSupp.previousNode != null)
            {
                ballList.Add(auxNodeSupp.element);
                auxNodeSupp = auxNodeSupp.previousNode;
            }

            if (auxNode.element.Color == auxNodeSupp.element.Color)
                ballList.Add(auxNodeSupp.element);
            else
                previousNode = auxNodeSupp;
        }

        if (ballList.Count >= 3)
        {
            CalculatePoints(ballList.Count); //TODO: Incorporar con checkRecursivity en branch Arbol

            ballList[0].GetComponent<BallMovement>()?.SetNextNodesCanMove(false);

            for (int i = 0; i < ballList.Count; i++)
            {
                var aux = DesqueueMiddle(ballList[i]);
                Destroy(aux.gameObject);
            }
        }

        // SI EL NODO PREVIO Y NODO SIGUIENTE COINDICEN COLOR, le paso el CHECKCOLOR de uno para que haga de nuevo toda la lista 
        if (previousNode != null && nextNode != null)
            if(previousNode.element.Color == nextNode.element.Color)
                CheckColors(previousNode); //RECURSIVIDAD!!!
    }

    public NodeBall GetRootNode()
    {
        var auxNode = queueDynamic.rootNode;
        return auxNode;
    }

    public bool IsEmpty()
    {
        return queueDynamic.IsEmpty();
    }

    public void CalculatePoints(int ballsQuantity, int checkColorsRecursivityRound = 1)
    {
        GameManager.instance.CurrentScore += (ballPointValue * ballsQuantity * checkColorsRecursivityRound);
        //print("Current Score: " + GameManager.instance.CurrentScore);
    }

    public int GetNumberOfCurrentBalls()
    {
        if (!IsEmpty())
        {
            int result = 1;
            var auxNodeSupp = GetRootNode();
            while (auxNodeSupp.nextNode != null)
            {
                auxNodeSupp = auxNodeSupp.nextNode;
                result++;
            }
            return result;
        } else
        {
            return 0;
        }
    }
}
