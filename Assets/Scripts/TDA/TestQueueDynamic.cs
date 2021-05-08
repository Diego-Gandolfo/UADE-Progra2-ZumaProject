using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQueueDynamic : MonoBehaviour
{
    [SerializeField] private Ball sphere;
    [SerializeField] private LayerMask layerMask;

    private QueueDymamic queueDymamic;
    private int counter;
    private Camera mainCamera;
    private Ball selectedSphere;

    private void Start()
    {
        mainCamera = Camera.main;

        queueDymamic = gameObject.AddComponent<QueueDymamic>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // cuando hace click
        {
            Vector2 cameraPosition = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y); // tomamos la posicion del mouse

            RaycastHit2D hit = Physics2D.Raycast(cameraPosition, new Vector2(0f, 0f), 0f, layerMask); // tiramos un rayo para ver si choca con algo

            if (hit != false)
            {
                if (hit.transform.CompareTag("Sphere")) // si lo que chocamos tiene el tag de "Sphere"
                {
                    selectedSphere = hit.transform.gameObject.GetComponent<Ball>(); // guardamos la referencia
                }
            }
        }
    }

    public void InitializeQueue()
    {
        if (queueDymamic.IsEmpty())
        {
            queueDymamic.Initialize(CreateClone());
            ShowQueue();
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
        if (!queueDymamic.IsEmpty())
        {
            NodeBall auxNode = queueDymamic.rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode
            int index = 0; // iniciamos el index

            // Para mostrar el Nodo Raíz
            if (auxNode != null) // si el auxNode es distinto de null
            {
                auxNode.element.transform.position = new Vector3(index, 0f, 0f); // lo movemos en x según el valor del index
                index++; // aumentamos el index
            }

            // Para mostrar el resto de los Nodos
            while (auxNode.nextNode != null) // nos fijamos si es el ultimo
            {
                auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
                auxNode.element.transform.position = new Vector3(index, 0f, 0f); // lo movemos en x según el valor del index
                index++; // aumentamos el index
            }
        }
    }

    public void EnqueueTop()
    {
        if (!queueDymamic.IsEmpty())
        {
            queueDymamic.EnqueueTop(CreateClone());
            ShowQueue();
        }
    }

    public void EnqueueMiddleAfter()
    {
        if (!queueDymamic.IsEmpty())
        {
            if (selectedSphere != null) queueDymamic.EnqueueMiddleAfter(CreateClone(), selectedSphere);
            ShowQueue();
            selectedSphere = null;
        }
    }

    public void EnqueueMiddleBefore()
    {
        if (!queueDymamic.IsEmpty())
        {
            if (selectedSphere != null) queueDymamic.EnqueueMiddleBefore(CreateClone(), selectedSphere);
            ShowQueue();
            selectedSphere = null;
        }
    }

    public void EnqueueBottom()
    {
        if (!queueDymamic.IsEmpty())
        {
            queueDymamic.EnqueueBottom(CreateClone());
            ShowQueue();
        }
    }

    public void DesqueueTop()
    {
        if (!queueDymamic.IsEmpty())
        {
            queueDymamic.DesqueueTop();
            ShowQueue();
        }
    }

    public void DesqueueBottom()
    {
        if (!queueDymamic.IsEmpty())
        {
            queueDymamic.DesqueueBottom();
            ShowQueue();
        }
    }

    public void DesqueueMiddle()
    {
        if (!queueDymamic.IsEmpty())
        {
            if (selectedSphere != null) queueDymamic.DesqueueMiddle(selectedSphere);
            ShowQueue();
            selectedSphere = null;
        }
    }
}
