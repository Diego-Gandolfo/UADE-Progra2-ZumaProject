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
        if (!IsStackFull()) 
        {
            //print(selected.QueueController.name);
            if(selected.QueueController.GetNumberOfCurrentBalls() > maxStack)
            {
                canonStack.Push(selected);
                var node = selected.BallSQ.Node;
                var nextNode = node.nextNode;
                var previousNode = node.previousNode;

                selected.QueueController.DesqueueMiddle(selected);
                selected.transform.position = new Vector3(0f, 0f, 0f);
                selected.gameObject.SetActive(false);
                //TODO: animacion de la pelota, al salirse de la pila (?)

                Ball nextBall = nextNode != null ? nextNode.element as Ball : null;
                Ball previousBall = previousNode != null ? previousNode.element as Ball : null;

                if (nextNode != null) nextNode.element.BallSQ.Regroup(1);

                if (nextNode != null && previousNode != null)
                    if (previousBall.Color == nextBall.Color)
                        selected.QueueController.CheckColors(nextNode);

                AudioManager.instance.PlaySound(SoundClips.Absorb);
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
