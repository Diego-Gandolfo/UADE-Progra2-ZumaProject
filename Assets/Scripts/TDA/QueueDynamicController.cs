using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueDynamicController : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab = null;

    private float ballSpeed;
    private int maxQuantity;
    private int currentQuantity;
    private int ballPointValue; 
    private QueueDymamic queueDynamic = null;
    private IGrafosManager grafosManager;
    private Transform[] path;

    private float ballSpawnTimer = 0f;
    private float startingTimer;
    private float startingCountdown = 0f;

    private float startMovingTimer;
    private float startMovingTimerCountdown = 0f;
    private int currentIndex = 1;


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

        startingCountdown += Time.deltaTime;
        if (startingCountdown >= startingTimer) //Esto es para inicializar el movimiento de la cola
        {
            StartMovingQueue(currentIndex);

            //if (currentIndex != maxQuantity)
            //    currentIndex++;
            //currentIndex = GetNumberOfBallsInQueue();

        }

    }

    public void Initialize(float speed, int maxQuantity, IGrafosManager grafosManager, int ballPointValue, float delayStartingTimer)
    {
        ballSpeed = speed;
        startMovingTimer = speed;
        this.maxQuantity = maxQuantity;
        this.grafosManager = grafosManager;
        this.ballPointValue = ballPointValue;
        this.startingTimer = delayStartingTimer;
        path = grafosManager.GetDijkstra(0);
    }

    public Ball CreateBall() // Creamos una nueva instancia y nodo
    {
        var ball = Instantiate(this.ballPrefab); // instanciamos una nueva Sphere
        ball.name = $"QueueController Ball ({currentQuantity})"; // le cambiamos el nombre para diferenciarlas
        ball.SetQueueController(this);

        var ballShowQueue = ball.GetComponent<BallShowQueue>();
        ballShowQueue.InitializePath(path, false, ballSpeed);
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
                ball.gameObject.GetComponent<BallMovement>().Node = node;
            }
            else
            {
                EnqueueTop();
            }
            ballSpawnTimer = 0;
            currentQuantity++;
        }
    }

    public void StartMovingQueue(int index)
    {
        int auxIndex = index; 
        
        var auxNodeSupp = GetRootNode();
        while (auxNodeSupp.nextNode != null)
        {
            auxNodeSupp = auxNodeSupp.nextNode;
        }

        for (int i = 0; i < auxIndex; i++)
        {
            auxNodeSupp.element.ballSQ.CanMove = true;
            auxNodeSupp = auxNodeSupp.previousNode; // guardamos el anterior en auxNode y repetimos
        }
    }

    public void ShowQueue()
    {
        if (!queueDynamic.IsEmpty())
        {
            NodeBall auxNode = queueDynamic.rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode
            int index = 0; // iniciamos el index

            // Para mostrar el Nodo Raíz
            if (auxNode != null) // si el auxNode es distinto de null
            {
                auxNode.element.transform.position = (transform.right * index) + transform.position; // lo movemos en x según el valor del index
                index++; // aumentamos el index
            }

            // Para mostrar el resto de los Nodos
            while (auxNode.nextNode != null) // nos fijamos si es el ultimo
            {
                auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
                auxNode.element.transform.position = (transform.right * index) + transform.position; // lo movemos en x según el valor del index
                index++; // aumentamos el index
            }
        }
    }

    public void EnqueueTop()
    {
        var ball = CreateBall();
        queueDynamic.EnqueueTop(ball);
        var node = FindNode(ball);
        //ball.GetComponent<BallMovement>().Node = node;
        //ball.GetComponent<BallShowQueue>().AddNode(node);
        ball.GetComponent<BallShowQueue>().Node = node;
    }

    public void EnqueueMiddleAfter(Ball newBall, Ball afterBall)
    {
        queueDynamic.EnqueueMiddleAfter(newBall, afterBall);
        EnqueueMiddleMain(newBall);
    }

    public void EnqueueMiddleBefore(Ball newBall, Ball beforeBall)
    {
        queueDynamic.EnqueueMiddleBefore(newBall, beforeBall);
        EnqueueMiddleMain(newBall);
    }

    public void EnqueueMiddleMain(Ball newBall) // son cosas que hacen ambos EnqueueMiddle, para no repetir codigo
    {
        var node = FindNode(newBall);
        var newBallMovement = newBall.GetComponent<BallMovement>();
        newBallMovement.Node = node;

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

    public int GetNumberOfBallsInQueue()
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
