using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private int multiplierSpeed = 4;
    private Transform[] path;
    public int CurrentPosition { get; private set; }

    public float Speed { get; set ; }
    public bool CanMove { get; set; }
    public NodeBall Node { get; set; }

    private bool canSpeedUp;
    private bool canStartMoving;
    private float currentCountdown;
    private bool canCheck = true;

    public bool StartingPoint { get;set; }

    void Start()
    {
        CurrentPosition = 0;
    }

    void Update()
    {
        if (CanMove && CurrentPosition < path.Length)
        {
            var currentTarget = path[CurrentPosition].position;
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);
            
            var distance = Vector2.Distance(transform.position, currentTarget);
            if (distance < 0.1f)
                CurrentPosition++;
        }

        if (canSpeedUp && currentCountdown < Time.time)
            ResetSpeed();

        if(canStartMoving && currentCountdown < Time.time)
            StartMovement();

        if (Node != null)
        {
            if (!StartingPoint) //Solo chequear cuando salieron el punto de inicio
            {
                if (Node.nextNode != null && Node.element.transform.position == Node.nextNode.element.transform.position)
                {
                    if (Node.previousNode != null && Node.nextNode.element.GetComponent<BallMovement>().CurrentPosition == Node.previousNode.element.GetComponent<BallMovement>().CurrentPosition)
                    {
                        var diference = Node.nextNode.element.transform.position - Node.previousNode.element.transform.position;
                        transform.position -= diference.normalized;
                    }
                    else
                    {
                        //TOMAR LA QUE ESTA ADELANTE UNICAMENTE (?)
                    }
                }
            }
        }
    }

    public void GetTargetBallInfo(NodeBall targetNode) //SI o si paso la que se va a correr a la derecha
    {
        var targetBallMovement = targetNode.element.GetComponent<BallMovement>();
        Speed = targetBallMovement.Speed;

        //var diference = Node.element.transform.position - Node.nextNode.element.transform.position;

        path = targetBallMovement.GetPathInfo(); // agarramos el path
        CurrentPosition = targetBallMovement.GetCurrentPosition(); // agarramos el index de la posicion actual del path

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
        return CurrentPosition;
    }

    public void GetPath(Transform[] recorrido)
    {
        path = recorrido;
        CurrentPosition = 0;
        CanMove = true;
        this.transform.position = path[CurrentPosition].position;
        CurrentPosition++;
    }

    public void SetNextNodesCanMove(bool value) // recorre los nodos siguientes para cambiarles el valor de CanMove
    {
        if (Node != null) // nos aseguramos que esté seteado el Node
        {
            var auxNode = Node.nextNode; // creamos una variable auxiliar para guardar el nodo siguiente

            while (auxNode != null) // mientras el auxiliar no sea null, es que hay un nodo sobre el que trabajar
            {
                auxNode.element.gameObject.GetComponent<BallMovement>().CanMove = value; // ponemos CanMove según el parametro recibido
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
                auxNode.element.gameObject.GetComponent<BallMovement>().CanMove = value;
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
                auxNode.element.gameObject.GetComponent<BallMovement>().StartSpeedUp();
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
                auxNode.element.gameObject.GetComponent<BallMovement>().StopMovement();
                auxNode = auxNode.previousNode;
            }
        }
    }

    private void ResetSpeed()
    {
        canSpeedUp = false;
        canCheck = true;
        Speed /= multiplierSpeed;
    }

    public void StartSpeedUp()
    {
        canSpeedUp = true;
        canCheck = false;
        currentCountdown = Time.time + (1 / (Speed * multiplierSpeed));
        Speed *= multiplierSpeed;
    }

    public void StopMovement()
    {
        CanMove = false;
        canCheck = false;
        canStartMoving = true;
        currentCountdown = Time.time + (1 / (Speed * multiplierSpeed));
    }

    public void StartMovement()
    {
        CanMove = true;
        canCheck = true;
        canStartMoving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Node != null && !StartingPoint)
        {
            if (canCheck && collision.gameObject.name == Node.nextNode.element.name)
                SetNextNodesCanMove(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Node != null)
        {
            if (StartingPoint) //ESTO ES SOLO PARA EL INICIO, PARA QUE SE MUEVAN EN CADENA, NO BORRAR
            {
                if (collision.gameObject.name == Node.previousNode?.element.name)
                {
                    StartingPoint = false;
                    Node.previousNode.element.GetComponent<BallMovement>().CanMove = true;
                }
            }

            //if (canCheck)
            //{
            //    var rootNode = Node.element.QueueController.GetRootNode();

            //    if (rootNode != Node)
            //        if (rootNode.nextNode != null && rootNode.previousNode != null)
            //        SetNextNodesCanMove(false);
            //}
        }
    }
}
