using Assets.Scripts.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueDynamicController : MonoBehaviour
{
    [SerializeField] private int checkColorCountToPowerUp = 1;
    [SerializeField] private int ballsToOrder = 1;
    [SerializeField] private Ball ballPrefab = null;
    [SerializeField] private PowerUp powerUpPrefab = null;
    [SerializeField] private float ballSpawnCooldown = 0f;
    private int checkColorCount;
    private int counter = 0;
    private bool canInstantiatePowerUp = true;
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
        if (movingCountdown >= (movingTimer/10) && canInitializeMoving) //Esto es para inicializar el movimiento de la cola
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

        var currentballs = GetNumberOfCurrentBalls();

        if (movingCountdown >= movingTimer && !canInitializeMoving && currentballs > 0)
        {
            ShowQueue(currentballs);
            movingCountdown = 0f;
        }
    }

    public void Initialize(float timer, int maxQuantity, IGrafosManager grafosManager, int ballPointValue, bool playWithPowerUp)
    {
        this.maxQuantity = maxQuantity;
        this.grafosManager = grafosManager;
        this.ballPointValue = ballPointValue;
        this.movingTimer = timer;
        canInstantiatePowerUp = playWithPowerUp;
        path = grafosManager.GetDijkstra(0);
    }

    public Ball CreateBall() // Creamos una nueva instancia y nodo
    {
        var ball = Instantiate(this.ballPrefab); // instanciamos una nueva Sphere
        ball.name = $"QueueController Ball ({currentQuantity})"; // le cambiamos el nombre para diferenciarlas
        ball.SetQueueController(this);

        ball.BallSQ.InitializePath(path, false);
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

    public void EnqueueMiddleAfter(IBall newBall, IBall afterBall, bool hasToCheckColors = true)
    {
        queueDynamic.EnqueueMiddleAfter(newBall, afterBall);
        if (hasToCheckColors) newBall.BallSQ.GetTargetBallInfo(afterBall);
        EnqueueMiddleMain(newBall, hasToCheckColors);
        if (hasToCheckColors) newBall.BallSQ.MakeSpaceToRight();
    }

    public void EnqueueMiddleBefore(IBall newBall, IBall beforeBall, bool hasToCheckColors = true)
    {
        if (newBall is PowerUp) print($"{newBall.GetGameObject().name}");
        queueDynamic.EnqueueMiddleBefore(newBall, beforeBall);
        if (hasToCheckColors) newBall.BallSQ.GetTargetBallInfo(beforeBall);
        EnqueueMiddleMain(newBall, hasToCheckColors);
        if (hasToCheckColors) beforeBall.BallSQ.MakeSpaceToRight();
    }

    public void EnqueueMiddleMain(IBall newBall, bool hasToCheckColors = true) // son cosas que hacen ambos EnqueueMiddle, para no repetir codigo
    {
        if (hasToCheckColors) ShowQueue(GetNumberOfCurrentBalls());
        var node = FindNode(newBall);
        newBall.BallSQ.Node = node;
        if (hasToCheckColors) CheckColors(node);
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

    public IBall DesqueueMiddle(IBall targetBall)
    {
        NodeBall node = FindNode(targetBall);
        
        var aux = queueDynamic.DesqueueMiddle(targetBall);
        
        targetBall = null;
        return aux;
    }

    public List<IBall> DequeueList(IBall ball, int ballsToOrder)
    {
        List<IBall> ballsToDequeue = new List<IBall>();
        NodeBall node = FindNode(ball);
        var auxNodeRight = node.nextNode != null ? node.nextNode : null;
        var auxNodeLeft = node.previousNode != null ? node.previousNode : null;

        if (auxNodeRight != null) // Recorrido Nodo Derecho, si el nodo derecho es null, no hago nada
        {
            for (int i = 0; i < ballsToOrder; i++) 
            {
                    if (auxNodeRight != null)
                    {
                        IBall aux = queueDynamic.DesqueueMiddle(auxNodeRight.element);
                        auxNodeRight = auxNodeRight.nextNode;
                        ballsToDequeue.Add(aux);
                    }
                    else break;
            }
        }

        if(auxNodeLeft != null) // Recorrido Nodo Izquierdo
        {
            for (int i = 0; i < ballsToOrder; i++) 
            {
                if (auxNodeLeft != null)
                {
                    IBall aux = queueDynamic.DesqueueMiddle(auxNodeLeft.element);
                    auxNodeLeft = auxNodeLeft.previousNode;
                    ballsToDequeue.Add(aux);
                }
                else break;
            }
        }

        //ShowQueue();
        return ballsToDequeue;
    }

    // TODO: llevar a QueueDynamic
    public NodeBall FindNode(IBall ball) //RECIBE PELOTA Y BUSCA EL NODO
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

        var ballList = new List<IBall>(); //Lista de pelotas que coinciden color
        ballList.Add(auxNode.element); //Agregamos la original

        if (auxNode.nextNode != null)
        {
            var auxNodeSupp = auxNode.nextNode;

            if (auxNode.element is Ball && auxNodeSupp.element is Ball)
            {
                Ball auxBall = (Ball) auxNode.element;
                Ball auxBallSupp = (Ball) auxNodeSupp.element;

                //print($"auxBall.Color: {auxBall.IndexValue} - auxBallSupp.Color: {auxBallSupp.IndexValue}");

                while (auxBall.Color == auxBallSupp.Color && auxNodeSupp.nextNode != null)
                {
                    ballList.Add(auxNodeSupp.element);
                    auxNodeSupp = auxNodeSupp.nextNode;

                    if (auxNodeSupp.element is Ball)
                        auxBallSupp = (Ball) auxNodeSupp.element;
                    else
                        break;
                }

                if (auxBall.Color == auxBallSupp.Color)
                    ballList.Add(auxNodeSupp.element);
                else
                    nextNode = auxNodeSupp;
            }
        }

        if (auxNode.previousNode != null)
        {
            var auxNodeSupp = auxNode.previousNode;

            if (auxNode.element is Ball && auxNodeSupp.element is Ball)
            {
                Ball auxBall = (Ball)auxNode.element;
                Ball auxBallSupp = (Ball)auxNodeSupp.element;

                //print($"auxBall.Color: {auxBall.IndexValue} - auxBallSupp.Color: {auxBallSupp.IndexValue}");

                while (auxBall.Color == auxBallSupp.Color && auxNodeSupp.previousNode != null)
                {
                    ballList.Add(auxNodeSupp.element);
                    auxNodeSupp = auxNodeSupp.previousNode;

                    if (auxNodeSupp.element is Ball)
                        auxBallSupp = (Ball) auxNodeSupp.element;
                    else
                        break;
                }

                if (auxBall.Color == auxBallSupp.Color)
                    ballList.Add(auxNodeSupp.element);
                else
                    previousNode = auxNodeSupp;
            }
        }

        if (ballList.Count >= 3)
        {
            //print("Cuantos Colores hay: " + ballList.Count);
            if (canInstantiatePowerUp) //Por cada vuelta de checkcolors que explota, sumamos uno al contador
            {
                checkColorCount++;
            }

            for (int i = 0; i < ballList.Count; i++)
            {
                if (ballList[i] is Ball)
                {
                    var aux = DesqueueMiddle(ballList[i]);
                    Destroy(aux.GetGameObject());
                }
            }

            CalculatePoints(ballList.Count); //TODO: Incorporar con checkRecursivity en branch Arbol


            if (nextNode != null)
                nextNode.element.BallSQ.Regroup(ballList.Count);
        }

        Ball nextBall = nextNode != null ? nextNode.element as Ball : null;
        Ball previousBall = previousNode != null ? previousNode.element as Ball : null;

        if (previousNode != null && nextNode != null) //Si AMBOS nodos existen
        {
            if (previousBall.Color == nextBall.Color) //Y si los colores coinciden
            {
                CheckColors(previousNode); //RECURSIVIDAD!!!
            }
            else //Si no coinciden el color -> no hay recursividad
            {
                if (checkColorCount >= checkColorCountToPowerUp) //chequeo si llega al powerup
                    InstantiatePowerUp(previousNode.element);
            }
        }
        else //Si una de las dos (o las dos) es nula...
        {
            if (checkColorCount >= checkColorCountToPowerUp) // Y si da para hacer un power up...
            {
                if (previousNode != null) //Me fijo si esta es Nula.. si no lo es, Instancio desde acá
                    InstantiatePowerUp(previousNode.element);
                else if (nextNode != null) //Si la anterior es nula, pruebo con este. Si ambos lo son, no deberia haber una QueueDynamic
                    InstantiatePowerUp(nextNode.element);
            }
        }
    }

    private void InstantiatePowerUp(IBall ball)
    {
        if (canInstantiatePowerUp)
        {
            SetCanInstantiatePowerUp(false);
            var newPowerUp = Instantiate(this.powerUpPrefab); // instanciamos una nueva Sphere
            newPowerUp.SetQueueController(this); //Le seteamos el controller
            newPowerUp.SetBallsToOrder(ballsToOrder);
            EnqueueMiddleAfter(newPowerUp, ball); //Lo encolamos
            checkColorCount = 0; //Reseteamos el contador
        }
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

    public void SetCanInstantiatePowerUp(bool value)
    {
        canInstantiatePowerUp = value;
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
