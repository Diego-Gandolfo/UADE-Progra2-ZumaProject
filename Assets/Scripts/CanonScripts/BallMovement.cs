﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Transform[] path;
    private int currentPosition = 0;

    public float Speed { get; set ; }
    public bool CanMove; //{ get; set; }
    public NodeBall Node { get; set; }

    void Update()
    {
        if (CanMove && currentPosition < path.Length)
        {
            var currentTarget = path[currentPosition].position;
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);

            var distance = Vector2.Distance(transform.position, currentTarget);
            if (distance < 0.1f)
            {
                currentPosition++;
            }
        }
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
        CanMove = true;
        SetNextNodesCanMove(true);
        SetPreviousNodesCanMove(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CanMove = false;
        SetNextNodesCanMove(false);
        SetPreviousNodesCanMove(false);
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
}
