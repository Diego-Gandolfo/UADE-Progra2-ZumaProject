using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonQueue : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    private QueueDymamic queueDymamic;
    [SerializeField] private float shootSpeed;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private QueueController queueShootingTest;
    [SerializeField] private Ball proyectile;

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
            proyectile.imProyectile = true;
            proyectile.speed = shootSpeed;
            proyectile = null;        
        }
        else if (proyectile != null)
        {
            proyectile.transform.position = shootPoint.position;
            proyectile.transform.rotation = shootPoint.rotation;
        }
        
    }

    public void InstanceProyectile() //Acá se instancian y agregan a la cola las nuevas pelotas
    {
        proyectile = queueShootingTest.CreateClone();
        //var newProyectile = Instantiate(ballPrefab);
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
