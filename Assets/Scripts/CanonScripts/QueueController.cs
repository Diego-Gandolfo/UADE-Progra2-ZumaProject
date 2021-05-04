using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    [SerializeField] private Ball ball = null;
    [SerializeField] private float ballSpawnCooldown = 0f;
    private float ballSpawnTimer = 0f;

    private QueueDymamic queueDynamic = null;
    
    private int counter = 0;
    private GameManager gameManager = null;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        

        queueDynamic = gameObject.AddComponent<QueueDymamic>();
        gameManager.queueDynamic = queueDynamic;
        queueDynamic.Initialize(CreateClone());
       
        ShowQueue();
    }

    private void Update()
    {
        ballSpawnTimer += Time.deltaTime;

        if(ballSpawnTimer >= ballSpawnCooldown)
        {
            EnqueueTop();
            ballSpawnTimer = 0;
        }
    }

    public Ball CreateClone() // Creamos una nueva instancia y nodo
    {
        var clone = Instantiate(ball); // instanciamos una nueva Sphere
        clone.name += $" ({counter})"; // le cambiamos el nombre para diferenciarlas
        counter++; // aumentamos el contador
        return clone; // devolvemos el clone creado
    }

    public void ShowQueue()
    {
        Node<Ball> auxNode = queueDynamic.rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode
        int index = 0; // iniciamos el index

        // Para mostrar el Nodo Raíz
        if (auxNode != null) // si el auxNode es distinto de null
        {
            auxNode.element.transform.position = new Vector3(index, 2f, 0f); // lo movemos en x según el valor del index
            index++; // aumentamos el index
        }

        // Para mostrar el resto de los Nodos
        while (auxNode.nextNode != null) // nos fijamos si es el ultimo
        {
            auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
            auxNode.element.transform.position = new Vector3(index, 2f, 0f); // lo movemos en x según el valor del index
            index++; // aumentamos el index
        }
    }

    public void EnqueueTop()
    {
        queueDynamic.EnqueueTop(CreateClone());
        ShowQueue();
    }

    public void EnqueueMiddleAfter(Ball newBall, Ball afterBall)
    {
        queueDynamic.EnqueueMiddleAfter(newBall, afterBall);
        CheckColors(newBall);
        ShowQueue();
    }

    public void EnqueueMiddleBefore(Ball newBall, Ball beforeBall)
    {
        queueDynamic.EnqueueMiddleBefore(newBall, beforeBall);
        CheckColors(newBall);
        ShowQueue();
    }

    public void EnqueueBottom()
    {
        queueDynamic.EnqueueBottom(CreateClone());
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

    public void DesqueueMiddle(Ball targetBall)
    {
        if (targetBall != null) queueDynamic.DesqueueMiddle(targetBall);
        ShowQueue();
        targetBall = null;
    }

    public void CheckColors(Ball ball)
    {
        var auxNode = queueDynamic.rootNode;

        var ballList = new List<Ball>();
        ballList.Add(ball);

        while (auxNode.element != ball && auxNode.nextNode != null)
        {
            auxNode = auxNode.nextNode;
        }

        if (auxNode.element == ball)
        {
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
            }
        }

        if (ballList.Count >= 3)
        {
            for (int i = 0; i < ballList.Count; i++)
            {
                gameManager.queueDynamic.DesqueueMiddle(ballList[i]);
            }
        }
    }
}
