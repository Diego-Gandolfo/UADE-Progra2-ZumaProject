using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonStack : MonoBehaviour
{
    [SerializeField] private QueueShootingTest ballQueue;
    private static readonly Stack canonStack = new Stack(); //es singleton porque no deberia haber más de un stack de absorcion. A lo sumo en cada nivel se vacia?
    private int maxStack = 5;
    private Ball currentBall;

    void Start()
    {
        canonStack.Initialize(maxStack);
    }

    public bool IsStackFull()
    {
        return canonStack.Index() < maxStack ? false : true;
    }

    public void Absorb(Ball selected)
    {
        if (!IsStackFull())
        {
            currentBall = selected;
            currentBall.imProyectile = true;
            canonStack.Push(selected);
            ballQueue.DesqueueMiddle(selected);
            print($"Absorbi una pelota {canonStack.Peek()} y el current index es: {canonStack.Index()}");

            //TODO: ANIMACION DE QUE LA PELOTA SE SALE DE LA PILA.
            //TODO: REPRESENTACION DEL COLOR DE LA PELOTA EN LA PILA
        } else
        {
            print("Stack Full");
        }
    }

    public bool IsEmpty()
    {
        return canonStack.IsEmpty();
    }

    public Ball LastBall()
    {
        var ball = canonStack.Peek();
        canonStack.Pop();
        return ball;
    }

    public Ball PeekColor() //Esto es solo para mostrar la proxima bola, no se desapila. 
    {
        //TODO: DEBERIA SOLO DEVOLVER EL COLOR DE LA BOLA.
        var ball = canonStack.Peek();
        return ball;
    }
}
