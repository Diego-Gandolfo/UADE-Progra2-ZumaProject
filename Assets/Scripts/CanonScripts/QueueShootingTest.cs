using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueShootingTest : MonoBehaviour
{
    [SerializeField] private Ball sphere;

    private BallQueue queueDymamic;
    private int counter;
    private Camera mainCamera;
    private Ball selectedSphere;
    private float ballSpawnTimer;
    [SerializeField] private float ballSpawnCooldown;
    private int maxBalls;

    private void Start()
    {
        mainCamera = Camera.main;

        queueDymamic = gameObject.AddComponent<BallQueue>();

        queueDymamic.Initialize(CreateClone());
        ShowQueue();
    }

    private void Update()
    {
        ballSpawnTimer += Time.deltaTime;
        if(ballSpawnTimer >= ballSpawnCooldown)
        {
            EnqueueBottom();
            Debug.Log("Cree Clon");
            ballSpawnTimer = 0;
        }
    }

    public Ball CreateClone() // Creamos una nueva instancia y nodo
    {
        var clone = Instantiate(sphere); // instanciamos una nueva Sphere
        clone.name += $" ({counter})"; // le cambiamos el nombre para diferenciarlas
        clone.GetComponent<SpriteRenderer>().color = Random.ColorHSV(); // les ponemos un color random
        counter++; // aumentamos el contador
        return clone; // devolvemos el clone creado
    }

    public void ShowQueue()
    {
        print("----- Start -----");

        Node<Ball> auxNode = queueDymamic.rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode
        int index = 0; // iniciamos el index

        // Para mostrar el Nodo Raíz
        if (auxNode != null) // si el auxNode es distinto de null
        {
            print(auxNode.element); // imprimimos en consola el elemento del auxNode
            auxNode.element.transform.position = new Vector3(index, 2f, 0f); // lo movemos en x según el valor del index
            index++; // aumentamos el index
        }

        // Para mostrar el resto de los Nodos
        while (auxNode.nextNode != null) // nos fijamos si es el ultimo
        {
            auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
            print(auxNode.element); // imprimimos en consola el elemento del auxNode
            auxNode.element.transform.position = new Vector3(index, 2f, 0f); // lo movemos en x según el valor del index
            index++; // aumentamos el index
        }
    }

    public void EnqueueTop()
    {
        queueDymamic.EnqueueTop(CreateClone());
        ShowQueue();
    }

    public void EnqueueMiddleAfter()
    {
        if (selectedSphere != null) queueDymamic.EnqueueMiddleAfter(CreateClone(), selectedSphere);
        ShowQueue();
        selectedSphere = null;
    }

    public void EnqueueMiddleBefore()
    {
        if (selectedSphere != null) queueDymamic.EnqueueMiddleBefore(CreateClone(), selectedSphere);
        ShowQueue();
        selectedSphere = null;
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

    public void DesqueueMiddle()
    {
        if (selectedSphere != null) queueDymamic.DesqueueMiddle(selectedSphere);
        ShowQueue();
        selectedSphere = null;
    }
}
