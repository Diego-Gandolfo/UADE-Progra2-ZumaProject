using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonQueue : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private float shootSpeed;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private QueueController queueController;
    [SerializeField] private Ball proyectile;
    private int counter;

    private Queue canonQueue;
    private int maxQuantity = 2;


    private void Start()
    {
        //canonQueue.Initialize(maxQuantity);

        InstanceProyectile();
    }

    private void Update()
    {
        Vector3 worldScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 diff = worldScreenPosition - transform.position;
        transform.up = diff.normalized;
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            proyectile.IsProjectile = true;
            proyectile.Speed = shootSpeed;
            proyectile = null;        
        }
        else if (proyectile != null)
        {
            proyectile.transform.position = shootPoint.position;
            proyectile.transform.rotation = shootPoint.rotation;
        }
    }

    public Ball CreateBall() // Creamos una nueva instancia y nodo
    {
        var ball = Instantiate(this.ballPrefab); // instanciamos una nueva Sphere
        ball.name = $"CannonBall ({counter})"; // le cambiamos el nombre para diferenciarlas
        ball.SetQueueController(queueController);
        counter++; // aumentamos el contador
        return ball; // devolvemos el clone creado
    }

    public void InstanceProyectile() //Acá se instancian y agregan a la cola las nuevas pelotas
    {
        proyectile = CreateBall();
        //var newProyectile = CreateBall();
        //canonQueue.Enqueue(newProyectile);
    }

    public Ball LastBall() //Devuelve y desacola una pelota
    {
        var ball = canonQueue.Peek();
        canonQueue.Dequeue();
        return ball;
    }

    public Ball PeekColor() //Esto es solo para mostrar la proxima bola, no se desacola
    {
        //TODO: DEBERIA SOLO DEVOLVER EL COLOR DE LA BOLA.
        var ball = canonQueue.Peek();
        return ball;
    }
}
