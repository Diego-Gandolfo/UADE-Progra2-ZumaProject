using Assets.Scripts.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueDynamicController : MonoBehaviour
{
    [SerializeField] private int checkColorCountToPowerUp = 1;
    [SerializeField] private Ball ballPrefab = null;
    [SerializeField] private PowerUp powerUpPrefab = null;
    [SerializeField] private float ballSpawnCooldown = 0f;
    private int checkColorCount;
    private float ballSpawnTimer = 0f;

    private QueueDymamic queueDynamic = null;
    
    private int counter = 0;

    private void Start()
    {
        queueDynamic = gameObject.GetComponent<QueueDymamic>();
        queueDynamic.Initialize(CreateBall());
        ShowQueue();
    }

    private void Update()
    {
        ballSpawnTimer += Time.deltaTime;

        if(ballSpawnTimer >= ballSpawnCooldown)
        {
            if (queueDynamic.IsEmpty())
            {
                queueDynamic.Initialize(CreateBall());
                ShowQueue();
            }
            else
            {
                EnqueueTop();
            }
            ballSpawnTimer = 0;
        }
    }

    public Ball CreateBall() // Creamos una nueva instancia y nodo
    {
        var ball = Instantiate(this.ballPrefab); // instanciamos una nueva Sphere
        ball.name = $"QueueController Ball ({counter})"; // le cambiamos el nombre para diferenciarlas
        ball.SetQueueController(this);
        counter++; // aumentamos el contador
        return ball; // devolvemos el clone creado
    }

    public void ShowQueue(bool mustDebug = false)
    {
        if (!queueDynamic.IsEmpty())
        {
            NodeBall auxNode = queueDynamic.rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode
            int index = 0; // iniciamos el index

            // Para mostrar el Nodo Raíz
            if (auxNode != null) // si el auxNode es distinto de null
            {
                auxNode.element.GetGameObject().transform.position = (transform.right * index) + transform.position; // lo movemos en x según el valor del index
                if (mustDebug) print($"ShowQueue: {auxNode.element.GetGameObject().name} - Index: {index} - Time: {Time.time}");
                index++; // aumentamos el index
            }

            // Para mostrar el resto de los Nodos
            while (auxNode.nextNode != null) // nos fijamos si es el ultimo
            {
                auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
                auxNode.element.GetGameObject().transform.position = (transform.right * index) + transform.position; // lo movemos en x según el valor del index
                if (mustDebug) print($"ShowQueue: {auxNode.element.GetGameObject().name} - Index: {index} - Time: {Time.time}");
                index++; // aumentamos el index
            }
        }
    }

    public void EnqueueTop()
    {
        queueDynamic.EnqueueTop(CreateBall());
        ShowQueue();
    }

    public void EnqueueMiddleAfter(Ball newBall, Ball afterBall, bool hasToCheck = true)
    {
        queueDynamic.EnqueueMiddleAfter(newBall, afterBall);
        var node = FindNode(newBall);
        if(hasToCheck)
            CheckColors(node);
        ShowQueue();
    }

    public void EnqueueMiddleBefore(Ball newBall, Ball beforeBall, bool hasToCheck = true)
    {
        queueDynamic.EnqueueMiddleBefore(newBall, beforeBall);
        var node = FindNode(newBall);
        if (hasToCheck)
            CheckColors(node);
        ShowQueue();
    }

    public void EnqueueBottom()
    {
        queueDynamic.EnqueueBottom(CreateBall());
        ShowQueue();
    }

    public void DesqueueTop()
    {
        queueDynamic.DesqueueTop();
        ShowQueue();
    }

    public void DesqueueBottom()
    {
        queueDynamic.DesqueueBottom();
        ShowQueue();
    }

    public IBall DesqueueMiddle(IBall targetBall)
    {
        NodeBall node = FindNode(targetBall);
        
        var aux = queueDynamic.DesqueueMiddle(targetBall);
        
        ShowQueue();
        targetBall = null;
        return aux;
    }

    public List<IBall> DequeueList(IBall ball, int index)
    {
        List<IBall> ballsToDequeue = new List<IBall>();
        NodeBall node = FindNode(ball);
        var auxNodeRight = node.nextNode;
        var auxNodeLeft = node.previousNode;

        if (auxNodeRight != null) // Recorrido Nodo Derecho, si el nodo derecho es null, no hago nada
        {
            for (int i = 0; i < index; i++) 
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
            for (int i = 0; i < index; i++) 
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

        ShowQueue();
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
        //print(auxNode.element.name);
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
                    auxBallSupp = (Ball) auxNodeSupp.element;
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
                    auxBallSupp = (Ball) auxNodeSupp.element;
                }

                if (auxBall.Color == auxBallSupp.Color)
                    ballList.Add(auxNodeSupp.element);
                else
                    previousNode = auxNodeSupp;
            }
        }

        if (ballList.Count >= 3)
        {
            checkColorCount++;
            for (int i = 0; i < ballList.Count; i++)
            {
                var aux = DesqueueMiddle(ballList[i]);
                Destroy(aux.GetGameObject());
            }
        }



        Ball nextBall = nextNode != null ? nextNode.element as Ball : null;
        Ball previousBall = previousNode != null ? previousNode.element as Ball : null;

        // SI EL NODO PREVIO Y NODO SIGUIENTE COINDICEN COLOR, le paso el CHECKCOLOR de uno para que haga de nuevo toda la lista 
        if (previousNode != null && nextNode != null)
        {
            if (previousBall.Color == nextBall.Color)
            {
                CheckColors(previousNode); //RECURSIVIDAD!!!
            }
        }
        else
        {
            if (checkColorCount >= checkColorCountToPowerUp)
            {
                print("Hola2");
                if (previousNode != null)
                    InstantiatePowerUp(previousNode.element);
                else if (nextNode != null)
                    InstantiatePowerUp(nextNode.element);
            }
        }
    }

    private void InstantiatePowerUp(IBall ball)
    {
        print("Hola1");
        var newBall = Instantiate(this.powerUpPrefab); // instanciamos una nueva Sphere
        newBall.SetQueueController(this);
        queueDynamic.EnqueueMiddleAfter(newBall, ball);
    }

    public IBall GetRootNode()
    {
        var auxNode = queueDynamic.rootNode;
        return auxNode.element;
    }
}
