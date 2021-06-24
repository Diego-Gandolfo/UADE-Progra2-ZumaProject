using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShowQueue : MonoBehaviour
{
    public Transform[] Path { get; private set; }
    public NodeBall Node { get; set; }
    public int CurrentPosition { get; private set; }
    public bool CanMove { get; set; }

    public void GetTargetBallInfo(IBall targetBall) //SI o si paso la que se va a correr a la derecha
    {
        Path = targetBall.BallSQ.Path;
        CurrentPosition = targetBall.BallSQ.CurrentPosition; 
        print($"GetInfo: {Path != null}");
    }

    public void InitializePath(Transform[] recorrido, bool value)
    {
        Path = recorrido;
        CurrentPosition = 0;
        CanMove = value;
        transform.position = Path[CurrentPosition].position;
    }
   
    public void Move()
    {
        if(CurrentPosition < Path.Length - 1)
        {
            CurrentPosition++;
            transform.position = Path[CurrentPosition].position;
        }
    }

    public void MakeSpaceToRight()
    {
        Move();

        if (Node !=  null && Node.nextNode != null)
        {
            Node.nextNode.element.BallSQ.MakeSpaceToRight();
        }
    }

    public void Regroup(int ballsDequeued)
    {
        CurrentPosition -= ballsDequeued;
        transform.position = Path[CurrentPosition].position;

        if (Node != null && Node.nextNode != null)
        {
            Node.nextNode.element.BallSQ.Regroup(ballsDequeued);
        }
    }
}
