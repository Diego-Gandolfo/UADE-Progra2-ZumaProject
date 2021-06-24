using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonStack : MonoBehaviour
{
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
        if (!IsStackFull()) //TODO: que no pueda absorber las ultimas pelotas (tamaño de la pila)
        {
            if(selected.QueueController.GetNumberOfCurrentBalls() > maxStack)
            {
                canonStack.Push(selected);
                var node = selected.QueueController.FindNode(selected);
                var nextNode = node.nextNode;
                var previousNode = node.previousNode;

                selected.QueueController.DesqueueMiddle(selected);
                selected.transform.position = new Vector3(0f, 0f, 0f);
                selected.gameObject.SetActive(false);
                //TODO: ANIMACION DE QUE LA PELOTA SE SALE DE LA PILA.

                Ball nextBall = nextNode.element as Ball;
                Ball previousBall = previousNode.element as Ball;

                if (nextNode != null) nextNode.element.BallSQ.Regroup(1);

                if (nextNode != null && previousNode != null)
                    if (previousBall.Color == nextBall.Color)
                        selected.QueueController.CheckColors(nextNode);
            }
            else
            {
                print("No se pueden absorber las ultimas 5 de la cola");
            }
        }
        else
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

    public int GetIndex()
    {
        return canonStack.Index();
    }
}
