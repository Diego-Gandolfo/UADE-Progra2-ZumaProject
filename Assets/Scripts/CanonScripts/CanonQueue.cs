using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonQueue : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private QueueDynamicController queueController;
    private int counter;
    private Queue canonQueue;
    private int maxQuantity = 2;


    private void Awake()
    {
        canonQueue = new Queue(maxQuantity);
        for (int i = 0; i < maxQuantity; i++)
        {
            InstanceProyectile();
        }
    }

    public Ball CreateBall() // Creamos una nueva instancia y nodo
    {
        var ball = Instantiate(this.ballPrefab); // instanciamos una nueva Sphere
        ball.name = $"CannonController Ball ({counter})"; // le cambiamos el nombre para diferenciarlas
        ball.SetQueueController(queueController);
        counter++; // aumentamos el contador
        return ball; // devolvemos el clone creado
    }

    public void InstanceProyectile() //Acá se instancian y agregan a la cola las nuevas pelotas
    {
        var newProyectile = CreateBall();
        canonQueue.Enqueue(newProyectile);
    }

    public Ball Dequeue() //Devuelve y desacola una pelota
    {
        var ball = canonQueue.Dequeue();
        return ball;
    }

    public Ball Peek()
    {
        return canonQueue.Peek();
    }

    public Ball PeekColor() //Esto es solo para mostrar la proxima bola, no se desacola
    {
        //TODO: DEBERIA SOLO DEVOLVER EL COLOR DE LA BOLA.
        var ball = canonQueue.Peek();
        return ball;
    }
}
