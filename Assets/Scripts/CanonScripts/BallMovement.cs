using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Transform[] path;
    private int currentPosition = 0;

    public float Speed { get; set ; }
    public int multiplierSpeed = 2;
    public bool CanMove; //{ get; set; }
    public NodeBall Node { get; set; }

    public float timer = 1;
    private bool canSpeedUp;
    private bool canStartMoving;
    private bool canCheck = true;
    private float currentCountdown;

    void Update()
    {
        if (CanMove && currentPosition < path.Length)
        {
            var currentTarget = path[currentPosition].position;
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);
            
            //transform.position += transform.right * Speed * Time.deltaTime;

             var distance = Vector2.Distance(transform.position, currentTarget);
            if (distance < 0.1f)
            {
                currentPosition++;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            print(Speed * Time.deltaTime);
            StartSpeedUp();
            print("current " + Speed * Time.deltaTime);
        }

        if (canSpeedUp && currentCountdown > Time.time)
        {
            ResetSpeed();
        }

        if(canStartMoving && currentCountdown > Time.time)
        {
            CanMove = true;
            canCheck = true;
        }
    }

    public void GetTargetBallInfo(NodeBall ball) //SI o si paso la que se va a correr a la derecha
    {
        print("GO: "+ gameObject.name + " Target: " + ball.element.gameObject.name);
        var targetBall = ball.element.GetComponent<BallMovement>();
        transform.position = ball.element.transform.position;

        path = targetBall.GetPathInfo();

        currentPosition = targetBall.GetCurrentPosition();
        targetBall.MakeSpaceToRight(); //Le digo a todas las pelotas que me hagan lugar 
        Node.previousNode.element.GetComponent<BallMovement>().MakeSpaceToLeft(); //Le digo a las de la izquierda que esperen por el mismo tiempo
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(canCheck)
            SetNextNodesCanMove(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (canCheck)
        {
            var rootNode = Node.element.QueueController.GetRootNode();

            if (rootNode != Node)
                if (rootNode.nextNode != null && rootNode.previousNode != null)
                    SetNextNodesCanMove(false);
        }

    }

    private void SetNextNodesCanMove(bool value) // recorre los nodos siguientes para cambiarles el valor de CanMove
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

    private void SetPreviousNodesCanMove(bool value) // recorre los nodos anteriores para cambiarles el valor de CanMove
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

    private void MakeSpaceToRight()
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
            var auxNode = Node.previousNode;
            while (auxNode != null)
            {
                var ballMovement = auxNode.element.gameObject.GetComponent<BallMovement>();
                print(ballMovement + " " + auxNode.element.name);
                if (ballMovement != null)
                    StopMovement();

                auxNode = auxNode.previousNode;
            }
        }
    }

    private void ResetSpeed()
    {
        canSpeedUp = false;
        canCheck = true;
        Speed -= multiplierSpeed;
    }

    private void StartSpeedUp()
    {

        currentCountdown = Time.time + timer;
        canCheck = false;
        canSpeedUp = true;
        Speed += multiplierSpeed;
        print("StartSpeedUp" + gameObject.name + " " + Speed);
    }

    private void StopMovement()
    {
        CanMove = false;
        canCheck = false;
        print(gameObject.name + " " + CanMove);
        currentCountdown = Time.time + timer;
        canStartMoving = true;
    }
}
