using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private int multiplierSpeed = 4;
    private Transform[] path;
    private int currentPosition = 0;

    public float Speed { get; set ; }
    public bool CanMove { get; set; }
    public NodeBall Node { get; set; }

    private bool canSpeedUp;
    private bool canStartMoving;
    private float currentCountdown;

    void Update()
    {
        if (CanMove && currentPosition < path.Length)
        {
            var currentTarget = path[currentPosition].position;
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);
            
            var distance = Vector2.Distance(transform.position, currentTarget);
            if (distance < 0.1f)
                currentPosition++;
        }

        if (canSpeedUp && currentCountdown < Time.time)
            ResetSpeed();

        if(canStartMoving && currentCountdown < Time.time)
            StartMovement();
    }

    public void GetTargetBallInfo(NodeBall targetNode) //SI o si paso la que se va a correr a la derecha
    {
        var targetBallMovement = targetNode.element.GetComponent<BallMovement>();
        Speed = targetBallMovement.Speed;

        path = targetBallMovement.GetPathInfo(); // agarramos el path
        currentPosition = targetBallMovement.GetCurrentPosition(); // agarramos el index de la posicion actual del path

        if (targetNode.nextNode != null)
            targetBallMovement.MakeSpaceToRight(); //Le digo a todas las pelotas que me hagan lugar 
        if (targetNode.previousNode != null)
            targetBallMovement.MakeSpaceToLeft(); //Le digo a las de la izquierda que esperen por el mismo tiempo
    }

    public Transform[] GetPathInfo()
    {
        return path;
    }

    public int GetCurrentPosition()
    {
        return currentPosition;
    }

    public void GetPath(Transform[] recorrido)
    {
        path = recorrido;
        currentPosition = 0;
        CanMove = true;
        this.transform.position = path[currentPosition].position;
        currentPosition++;
    }

    public void SetNextNodesCanMove(bool value) // recorre los nodos siguientes para cambiarles el valor de CanMove
    {
        if (Node != null) // nos aseguramos que esté seteado el Node
        {
            var auxNode = Node.nextNode; // creamos una variable auxiliar para guardar el nodo siguiente

            while (auxNode != null) // mientras el auxiliar no sea null, es que hay un nodo sobre el que trabajar
            {
                var ballMovement = auxNode.element.gameObject.GetComponent<BallMovement>(); // obtenemos el BallMovement

                if (ballMovement != null) ballMovement.CanMove = value; // si lo tiene, ponemos CanMove según el parametro recibido

                auxNode = auxNode.nextNode; // guardamos en auxiliar la referencia al nodo siguiente
            }
        }
    }

    public void SetPreviousNodesCanMove(bool value) // recorre los nodos anteriores para cambiarles el valor de CanMove
    {
        if (Node != null)
        {
            var auxNode = Node.previousNode;

            while (auxNode != null)
            {
                var ballMovement = auxNode.element.gameObject.GetComponent<BallMovement>();

                if (ballMovement != null) ballMovement.CanMove = value;

                auxNode = auxNode.previousNode;
            }
        }
    }

    public void MakeSpaceToRight()
    {
        if (Node != null) // nos aseguramos que esté seteado el Node
        {
            var auxNode = Node.nextNode; // creamos una variable auxiliar para guardar el nodo siguiente

            while (auxNode != null) // mientras el auxiliar no sea null, es que hay un nodo sobre el que trabajar
            {
                var ballMovement = auxNode.element.gameObject.GetComponent<BallMovement>(); // obtenemos el BallMovement

                if (ballMovement != null)
                    ballMovement.StartSpeedUp();
                
                auxNode = auxNode.nextNode; // guardamos en auxiliar la referencia al nodo siguiente
            }
        }
    }

    public void MakeSpaceToLeft()
    {
        if (Node != null)
        {
            StopMovement();

            var auxNode = Node.previousNode;
            while (auxNode != null)
            {
                var ballMovement = auxNode.element.gameObject.GetComponent<BallMovement>();
                
                if (ballMovement != null)
                    ballMovement.StopMovement();

                auxNode = auxNode.previousNode;
            }
        }
    }

    private void ResetSpeed()
    {
        canSpeedUp = false;
        Speed /= multiplierSpeed;
    }

    private void StartSpeedUp()
    {
        canSpeedUp = true;
        currentCountdown = Time.time + (1 / (Speed * multiplierSpeed));
        Speed *= multiplierSpeed;
    }

    public void StopMovement()
    {
        CanMove = false;
        canStartMoving = true;
        currentCountdown = Time.time + (1 / (Speed * multiplierSpeed));
    }

    public void StartMovement()
    {
        CanMove = true;
        canStartMoving = false;
    }
}
