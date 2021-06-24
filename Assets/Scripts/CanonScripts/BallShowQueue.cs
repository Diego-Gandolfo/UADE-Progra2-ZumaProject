using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShowQueue : MonoBehaviour
{
    public int CurrentPosition { get; private set; }

    public Transform[] Path { get; private set; }
    public NodeBall Node { get; set; }

    public bool CanMove { get; set; }

    public float Timer { get; private set; }

    private float timerCountdown;

    void Update()
    {
        timerCountdown += Time.deltaTime;
        if (CanMove)
        {
            if (timerCountdown >= Timer)
            {
                Move();
            }

        } else
        {
            if (timerCountdown >= Timer*2)
            {
                Move();
                CanMove = true;
            }
        }
    }

    public void GetTargetBallInfo(NodeBall targetNode) //SI o si paso la que se va a correr a la derecha
    {
        var targetBallMovement = targetNode.element.GetComponent<BallShowQueue>();
        Path = targetBallMovement.Path;
        CurrentPosition = targetBallMovement.CurrentPosition; // agarramos el index de la posicion actual del path

        if (targetNode.nextNode != null)
            targetBallMovement.MoveRight(); //Le digo a la pelota que sigua que se mueva un lugar a la derecha (y esa le avisa a la que sigue)
        if (targetNode.previousNode != null)
            targetBallMovement.WaitASecondLeft(); //Le digo a la izquierda que espere
    }

    public void InitializePath(Transform[] recorrido, bool value, float timer)
    {
        Path = recorrido;
        CurrentPosition = 0;
        CanMove = value;
        Timer = timer;
        transform.position = Path[CurrentPosition].position;
    }

    public void AddNode(NodeBall node)
    {
        Node = node;
        print("Node added to: " + Node.element.name);
    }

    public void MoveRight() //Me muevo un lugar y le aviso a la siguiente (si hay) que tambien se corra un lugar
    {
        if(CurrentPosition < Path.Length - 1)
        {
            CurrentPosition++;
            transform.position = Path[CurrentPosition].position;
            if(Node != null)
                if (Node.nextNode != null)
                    Node.nextNode.element.ballSQ.MoveRight();
        }
    }

    public void MoveLeft()
    {

    }

    public void WaitASecondLeft() //Me quedo quieta y le aviso a la de atras que tambien se quede quieta
    {
        CanMove = false;
        print("wait");
        if (Node.previousNode != null)
            Node.previousNode.element.GetComponent<BallShowQueue>().WaitASecondLeft();
    }

    public void WaitASecondRight()
    {
        CanMove = false;
        if (Node.nextNode != null)
            Node.nextNode.element.GetComponent<BallShowQueue>().WaitASecondRight();
    }

    public void MoveOneBack()
    {
        if (CurrentPosition < Path.Length - 1 && CurrentPosition > 0)
        {
            CurrentPosition--;
            transform.position = Path[CurrentPosition].position;
            if (Node != null)
                if (Node.nextNode != null)
                    Node.nextNode.element.GetComponent<BallShowQueue>().MoveOneBack();
        }
    }

    public void Move()
    {
        if(CurrentPosition < Path.Length - 1)
        {
            if (Node.nextNode.element.GetComponent<BallShowQueue>().CurrentPosition != CurrentPosition + 1)
            {
                CurrentPosition++;
                transform.position = Path[CurrentPosition].position;
                timerCountdown = 0;
                print("move");
            }
            else
            {
                timerCountdown = 0;
                print("wait");
            }
        }
    }
}
