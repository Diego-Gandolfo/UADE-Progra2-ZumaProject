using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonStack : MonoBehaviour
{
    //[SerializeField] private QueueDynamicController queueController;
    private static readonly Stack canonStack = new Stack(); //es singleton porque no deberia haber más de un stack de absorcion. A lo sumo en cada nivel se vacia?
    private int maxStack = 5;

    private void Awake()
    {
        canonStack.Initialize(maxStack);
    }

    public bool IsStackFull()
    {
        return canonStack.Index() < maxStack ? false : true;
    }

    public void Absorb(Ball selected)
    {
        if (!IsStackFull() && selected != selected.QueueController.GetRootNode())
        {
            canonStack.Push(selected);
            //var aux = queueController.DesqueueMiddle(selected);
            var aux = selected.QueueController.DesqueueMiddle(selected);
            aux.transform.position = new Vector3(0f, -10f, 0f);
            //TODO: ANIMACION DE QUE LA PELOTA SE SALE DE LA PILA.
        } else
        {
            print("Stack Full or is the RootNode");
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

    public Color PeekPreviousColor()
    {
        var ball = canonStack.PeekPrevious();
        return ball.Color;
    }

    public int CheckNumber() //TODO: cambiar nombre a CheckIndex
    {
        return canonStack.Index();
    }
}
