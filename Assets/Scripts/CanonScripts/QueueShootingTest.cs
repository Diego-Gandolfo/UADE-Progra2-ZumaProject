using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueShootingTest : MonoBehaviour
{
    [SerializeField] private Ball ball = null;
    [SerializeField] private float ballSpawnCooldown = 0f;
    private float ballSpawnTimer = 0f;

    private QueueDymamic queueDymamic = null;
    
    private int counter = 0;
    private int maxBalls = 0;
    //private Camera mainCamera = null;
    private GameManager gameManager = null;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //mainCamera = Camera.main;
        

        queueDymamic = gameObject.AddComponent<QueueDymamic>();
        gameManager.queueDynamic = queueDymamic;
        queueDymamic.Initialize(CreateClone());
       
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
        Node<Ball> auxNode = queueDymamic.rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode
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
        queueDymamic.EnqueueTop(CreateClone());
        ShowQueue();
    }

    public void EnqueueMiddleAfter(Ball newBall, Ball afterBall)
    {
        queueDymamic.EnqueueMiddleAfter(newBall, afterBall);
        ShowQueue();
    }

    public void EnqueueMiddleBefore(Ball newBall, Ball beforeBall)
    {
        queueDymamic.EnqueueMiddleBefore(newBall, beforeBall);
        ShowQueue();
    }

    public void EnqueueBottom()
    {
        queueDymamic.EnqueueBottom(CreateClone());
        ShowQueue();
    }

    public void DesqueueTop()
    {
        queueDymamic.DesqueueTop();
        ShowQueue();
    }

    public void DesqueueBottom()
    {
        queueDymamic.DesqueueBottom();
        ShowQueue();
    }

    public void DesqueueMiddle(Ball targetBall)
    {
        if (targetBall != null) queueDymamic.DesqueueMiddle(targetBall);
        ShowQueue();
        targetBall = null;
    }
}
