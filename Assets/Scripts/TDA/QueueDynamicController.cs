using Assets.Scripts.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QueueDynamicController : MonoBehaviour
{
    private QueueDymamic queueDynamic = null;
    private IGrafosManager grafosManager;
    private Transform[] path = new Transform[0];
    private Ball ballPrefab = null;
    private int ballPointValue;
    private int pathNumber = 1;

    //QUANTITY
    private int maxQuantity;
    private int currentQuantity;
    private int numberOfRecursivity = 1;

    //MOVEMENT AND TIMERS
    private float movingTimer;
    private float movingCountdown = 0f;

    //POWER UP
    private PowerUp powerUpPrefab = null;
    private int checkColorCount;
    private bool canInstantiatePowerUp = true;
    private int checkColorCountToPowerUp = 1;
    private int ballsToOrder = 1;

    public UnityEvent OnEmpty = new UnityEvent();

    private void Awake()
    {
        queueDynamic = gameObject.GetComponent<QueueDymamic>();
    }

    private void Start()
    {
        CreateQueue();
    }

    private void Update()
    {
        movingCountdown += Time.deltaTime;

        if (movingCountdown >= movingTimer && GetNumberOfCurrentBalls() > 0)
        {
            ShowQueue();
            movingCountdown = 0f;
        }
    }

    public void Initialize(Ball ballPrefab, float timer, int maxQuantity, IGrafosManager grafosManager, int pathNumber, int ballPointValue)
    {
        this.ballPrefab = ballPrefab;
        this.maxQuantity = maxQuantity;
        this.grafosManager = grafosManager;
        this.ballPointValue = ballPointValue;
        this.movingTimer = timer;
        this.pathNumber = pathNumber;
    }

    public void PowerUpSettings(PowerUp powerUpPrefab, bool playWithPowerUp, int ballsToOrder, int checkColorCountToPowerUp)
    {
        canInstantiatePowerUp = playWithPowerUp;
        this.powerUpPrefab = powerUpPrefab;
        this.checkColorCountToPowerUp = checkColorCountToPowerUp;
        this.ballsToOrder = ballsToOrder;
    }

    public Ball CreateBall() // Creamos una nueva instancia y nodo
    {
        var ball = Instantiate(this.ballPrefab); // instanciamos una nueva Sphere
        ball.name = $"QueueController{pathNumber} - Ball ({currentQuantity})"; // le cambiamos el nombre para diferenciarlas
        ball.SetQueueController(this);

        ball.BallSQ.InitializePath(path, false);
        return ball; // devolvemos el clone creado
    }

    public void CreateQueue()
    {
        path = grafosManager.GetDijkstra(0, pathNumber);

        var ball = CreateBall();
        queueDynamic.Initialize(ball);
        var node = FindNode(ball);
        ball.BallSQ.Node = node;
        ball.BallSQ.CurrentPosition = maxQuantity - currentQuantity;
        currentQuantity++;

        for (int i = 0; i < maxQuantity - 1; i++)
        {
            EnqueueTop();
            currentQuantity++;
        }
    }

    private void ShowQueue()
    {
        var auxNodeSupp = queueDynamic.rootNode;

        if (auxNodeSupp != null)
            auxNodeSupp.element.BallSQ.Move();

        while (auxNodeSupp.nextNode != null)
        {
            auxNodeSupp = auxNodeSupp.nextNode;
            auxNodeSupp.element.BallSQ.Move();
        }
    }

    private void EnqueueTop()
    {
        var ball = CreateBall();
        queueDynamic.EnqueueTop(ball);
        var node = FindNode(ball);
        ball.BallSQ.Node = node;
        ball.BallSQ.CurrentPosition = maxQuantity - currentQuantity;
    }

    public void EnqueueMiddleAfter(IBall newBall, IBall afterBall, bool isPowerUp = false)
    {
        queueDynamic.EnqueueMiddleAfter(newBall, afterBall);
        if (!isPowerUp) newBall.BallSQ.GetTargetBallInfo(afterBall);
        EnqueueMiddleMain(newBall, isPowerUp);
        if (!isPowerUp) newBall.BallSQ.MakeSpaceToRight();
    }

    public void EnqueueMiddleBefore(IBall newBall, IBall beforeBall, bool isPowerUp = false)
    {
        queueDynamic.EnqueueMiddleBefore(newBall, beforeBall);
        if (!isPowerUp) newBall.BallSQ.GetTargetBallInfo(beforeBall);
        EnqueueMiddleMain(newBall, isPowerUp);
        if (!isPowerUp) beforeBall.BallSQ.MakeSpaceToRight();
    }

    private void EnqueueMiddleMain(IBall newBall, bool isPowerUp = false) // son cosas que hacen ambos EnqueueMiddle, para no repetir codigo
    {
        if (!isPowerUp) ShowQueue();
        var node = FindNode(newBall);
        newBall.BallSQ.Node = node;
        if (!isPowerUp) CheckColors(node);
    }

    public IBall DesqueueMiddle(IBall targetBall)
    {
        NodeBall node = FindNode(targetBall);
        var aux = queueDynamic.DesqueueMiddle(targetBall);
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

        if (auxNodeLeft != null) // Recorrido Nodo Izquierdo
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

        return ballsToDequeue;
    }

    private NodeBall FindNode(IBall ball) //Recibe una pelota y le busca el nodo
    {
        var auxNode = queueDynamic.rootNode;

        while (auxNode.element != ball && auxNode.nextNode != null)
        {
            auxNode = auxNode.nextNode;
        }
        if (auxNode.element == ball)
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
                Ball auxBall = (Ball)auxNode.element;
                Ball auxBallSupp = (Ball)auxNodeSupp.element;

                while (auxBall.Color == auxBallSupp.Color && auxNodeSupp.nextNode != null)
                {
                    ballList.Add(auxNodeSupp.element);
                    auxNodeSupp = auxNodeSupp.nextNode;

                    if (auxNodeSupp.element is Ball)
                        auxBallSupp = (Ball)auxNodeSupp.element;
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

                while (auxBall.Color == auxBallSupp.Color && auxNodeSupp.previousNode != null)
                {
                    ballList.Add(auxNodeSupp.element);
                    auxNodeSupp = auxNodeSupp.previousNode;

                    if (auxNodeSupp.element is Ball)
                        auxBallSupp = (Ball)auxNodeSupp.element;
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
            if (canInstantiatePowerUp) //Por cada vuelta de checkcolors que explota, sumamos uno al contador
            {
                checkColorCount++;
            }

            numberOfRecursivity++;

            for (int i = 0; i < ballList.Count; i++)
            {
                if (ballList[i] is Ball)
                {
                    var aux = DesqueueMiddle(ballList[i]);
                    //Destroy(aux.GetGameObject(), 2f);
                    aux.GetGameObject().SetActive(false);
                }
            }

            CalculatePoints(ballList.Count, numberOfRecursivity);


            if (nextNode != null)
                nextNode.element.BallSQ.Regroup(ballList.Count);

            if (queueDynamic.IsEmpty()) //Chequea si la cola esta vacia.... Si esta avisale al resto
                OnEmpty?.Invoke();

            AudioManager.instance.PlaySound(SoundClips.Explosion);
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
                numberOfRecursivity = 1;
                if (checkColorCount >= checkColorCountToPowerUp) //chequeo si llega al powerup
                    InstantiatePowerUp(previousNode.element);
            }
        }
        else //Si una de las dos (o las dos) es nula...
        {
            numberOfRecursivity = 1;
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

    private void CalculatePoints(int ballsQuantity, int checkColorsRecursivityRound = 1)
    {
        GameManager.instance.CurrentScore += (ballPointValue * ballsQuantity * checkColorsRecursivityRound);
    }

    public void SetCanInstantiatePowerUp(bool value)
    {
        canInstantiatePowerUp = value;
    }

    public int GetNumberOfCurrentBalls()
    {
        if (!queueDynamic.IsEmpty())
        {
            int result = 1;
            var auxNodeSupp = queueDynamic.rootNode;
            while (auxNodeSupp.nextNode != null)
            {
                auxNodeSupp = auxNodeSupp.nextNode;
                result++;
            }
            return result;
        }
        else
        {
            return 0;
        }
    }
}
