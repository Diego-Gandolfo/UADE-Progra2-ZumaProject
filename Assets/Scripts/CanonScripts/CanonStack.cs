using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonStack : MonoBehaviour
{
    [SerializeField] private QueueDynamicController queueController;
    private static readonly Stack canonStack = new Stack(); //es singleton porque no deberia haber más de un stack de absorcion. A lo sumo en cada nivel se vacia?
    private int maxStack = 5;

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
        if (!IsStackFull())//TODO: && selected != queueController.GetRootNode()
        {
            canonStack.Push(selected);
            var aux = queueController.DesqueueMiddle(selected);
            aux.transform.position = new Vector3(0f, -10f, 0f);
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

    public Ball Peek()
    {
        var ball = canonStack.Peek();
        return ball;
    }

    public Ball Pop()
    {
        var ball = canonStack.Pop();
        return ball;
    }

    public Ball PeekColor() //Esto es solo para mostrar la proxima bola, no se desapila. 
    {
        //TODO: DEBERIA SOLO DEVOLVER EL COLOR DE LA BOLA.
        var ball = canonStack.Peek();
        return ball;
    }
}
