using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueDynamicController : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab = null;
    [SerializeField] private float ballSpawnCooldown = 0f;
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
        if(hasToCheck)
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

    public Ball DesqueueMiddle(Ball targetBall)
    {
        NodeBall node = FindNode(targetBall);
        
        var aux = queueDynamic.DesqueueMiddle(targetBall);
        
        ShowQueue();
        targetBall = null;
        return aux;
    }

    public List<Ball> DequeueList(Ball ball, int index)
    {
        NodeBall node = FindNode(ball);
        var auxNodeRight = node.nextNode;
        var auxNodeLeft = node.previousNode;
        List<Ball> ballsToDequeue = new List<Ball>();
        if(auxNodeRight != null)
        {
            for (int i = 0; i < index; i++) // Recorrido Nodo Derecho
            {
                    var auxNodeSupp = auxNodeRight.nextNode;
                    if (auxNodeSupp != null)
                    {
                        Ball aux = queueDynamic.DesqueueMiddle(auxNodeRight.element);
                        auxNodeRight = auxNodeSupp;
                        ballsToDequeue.Add(aux);
                    }
                    else break;
               
            }
        }


        for (int i = 0; i < index; i++) // Recorrido Nodo Izquierdo
        {
            var auxNodeSupp = node.previousNode;
            if (auxNodeLeft != null)
            {
                var aux = queueDynamic.DesqueueMiddle(auxNodeLeft.element);
                auxNodeLeft = auxNodeSupp;
                ballsToDequeue.Add(aux);
            }
            else break;
        }

        return ballsToDequeue;
    }
    // TODO: llevar a QueueDynamic
    public NodeBall FindNode(Ball ball) //RECIBE PELOTA Y BUSCA EL NODO
    {
        var auxNode = queueDynamic.rootNode;

        while (auxNode.element != ball && auxNode.nextNode != null)
        {
            auxNode = auxNode.nextNode;
        }
        print(auxNode.element.name);
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

    public Ball GetRootNode()
    {
        var auxNode = queueDynamic.rootNode;
        return auxNode.element;
    }

}
