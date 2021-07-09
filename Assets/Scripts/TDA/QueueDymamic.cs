﻿using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueDymamic : MonoBehaviour, IQueueDynamic<IBall> // Esta es la implementación, y el tipo de dato a usar son Spheres
{
    public NodeBall rootNode = null; // Acá creo el Nodo Raíz, que va a trabajar con el tipo de dato Spheres

    public void Initialize(IBall ball) // Para inicializar la Cola, recibe una Sphere
    {
        NodeBall newNode = new NodeBall(); // creamos un nuevo Nodo
        newNode.InitializeNode(ball, null, null); // lo inicializa con sphere como su elemento y tanto su previo como el siguiente son null
        rootNode = newNode; // decimos que el Nodo Raíz es el nuevo Nodo creado
    }

    public void EnqueueTop(IBall ball) // Acá metemos en la cola un Sphere como primer elemento
    {
        if (!IsEmpty())
        {
            NodeBall newNode = new NodeBall(); // creamos un nuevo elemento
            newNode.InitializeNode(ball, null, rootNode); // lo inicializa con sphere como su elemento, el siguiente es el que era el Nodo Raíz y el anterior es null
            rootNode.InitializeNode(rootNode.element, newNode, rootNode.nextNode); // cambiamos los valores al Nodo que está referenciado en rootNode para que su elemento siga siendo el mismo, pero que ahora su previo sea el nuevo Nodo
            rootNode = newNode; // decimos que el Nodo Raíz es el nuevo Nodo creado
        }
    }

    public void EnqueueMiddleAfter(IBall newBall, IBall afterBall) // esta sirve para meter una Sphere despues del nodo que tiene la afterBall
    {
        if (!IsEmpty())
        {
            NodeBall auxNode = rootNode; // creamos un auxNode y guaramos la referencia al rootNode

            while (auxNode.element != afterBall && auxNode.nextNode != null) // bucle donde nos fijamos si el elementos del auxNode es el afterSphere o si llegamos al final
            {
                auxNode = auxNode.nextNode; // si no es, almacenamos en auxNode el nodo siguiente de auxNode y vuelve al bucle
            }

            if (auxNode.nextNode != null) // si lo encontramos
            {
                NodeBall newNode = new NodeBall(); // creamos un newNode
                newNode.InitializeNode(newBall, auxNode, auxNode.nextNode); // lo inicializamos con la sphere como su elemento, el auxNode como su previo y el siguiente del auxNode como su siguiente

                auxNode.nextNode.previousNode = newNode; // ahora el nodo previo del nodo siguiente de auxNode es el newNode
                auxNode.nextNode = newNode; // y el nodo siguiente del auxNode es el newNode
            }
            else // si no lo encontramos
            {
                EnqueueBottom(newBall);
            }
        }
    }

    public void EnqueueMiddleBefore(IBall newBall, IBall beforeBall) // esta sirve para meter una Sphere antes del nodo que tiene la beforeSphere
    {
        if (!IsEmpty())
        {
            NodeBall auxNode = rootNode; // creamos un auxNode y guaramos la referencia al rootNode

            while (auxNode.element != beforeBall && auxNode.nextNode != null) // bucle donde nos fijamos si el elementos del auxNode es el beforeSphere o si llegamos al final
            {
                auxNode = auxNode.nextNode; // si no es, almacenamos en auxNode el nodo siguiente de auxNode y vuelve al bucle
            }

            if (auxNode.previousNode != null) // si lo encontramos
            {
                NodeBall newNode = new NodeBall(); // creamos un newNode
                newNode.InitializeNode(newBall, auxNode.previousNode, auxNode);// lo inicializamos con la sphere como su elemento, el nodo previo del auxNode como su previo y el auxNode como su siguiente

                auxNode.previousNode.nextNode = newNode; // ahora el nodo siguiente del nodo previo de auxNode es el newNode
                auxNode.previousNode = newNode; // y el nodo previo del auxNode es el newNode
            }
            else // si no lo encontramos
            {
                EnqueueTop(newBall);
            }
        }
    }

    public void EnqueueBottom(IBall ball) // este sirve para meter un Sphere al final de la cola
    {
        if (!IsEmpty())
        {
            NodeBall auxNode = rootNode; // creamos un nodo Auxiliar y nos guardamos la referencia al rootNode

            while (auxNode.nextNode != null) // nos fijamos si es el ultimo
            {
                auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
            }

            NodeBall newNode = new NodeBall(); // creamos un nuevo nodo
            newNode.InitializeNode(ball, auxNode, null); // inicializamos el newNode con sphere como su elemento, auxNode como el previo y null como siguiente
            auxNode.nextNode = newNode; // indicamos que el siguente del auxNode es el newNode
        }
    }

    public IBall DesqueueTop() // para sacar y devolver el primer elemento
    {
        if (!IsEmpty())
        {
            IBall auxBall = rootNode.element; // nos guardamos la referencia a la Sphere guarada en el rootNode
            rootNode.element.GetGameObject().SetActive(false);
            if (rootNode.nextNode != null)
            {
                rootNode.nextNode.previousNode = null; // decimos que el nodo previo del nodo siguiente de rootNode es null, antes referenciaba a rootNode
                rootNode = rootNode.nextNode; // decimos que el nuevo rootNode es el qe antes era el siguiente del rootNode
            }
            else
            {
                rootNode = null;
            }
            return auxBall; // devolvemos la Sphere
        }
        else
        {
            return null;
        }
    }

    public IBall DesqueueMiddle(IBall ball) // para quitar un elemento de la mitad de la cola
    {
        NodeBall auxNode = rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode
            
        while (auxNode.element != ball && (auxNode.nextNode != null || auxNode == rootNode)) // bucle donde nos fijamos si el elemento del auxNode es el sphere o si llegamos al final
        {
            auxNode = auxNode.nextNode; // si no es, almacenamos en auxNode el nodo siguiente de auxNode y vuelve al bucle
        }

        if (auxNode.nextNode != null && auxNode.previousNode != null) // si lo encontramos
        {
            auxNode.previousNode.nextNode = auxNode.nextNode; // decimos que el siguiente del previo del auxNode ahora es el siguiente del auxNode
            auxNode.nextNode.previousNode = auxNode.previousNode; // decimos que el previo del siguiente del auxNode es ahora el previo del auxNode
        }
        else if (auxNode.nextNode == null && auxNode.previousNode != null) //es el ultimo
        {
            auxNode.previousNode.nextNode = null; // decimos que el siguiente del previo del auxNode ahora es el siguiente del auxNode
        }
        else if (auxNode == rootNode)
        {
            if (auxNode.nextNode != null)
            {
                auxNode.nextNode.previousNode = null;    // decimos que el previo del siguiente del auxNode es ahora el previo del auxNode
                rootNode = auxNode.nextNode;
            }
            else
            {
                rootNode = null;
            }
        }
        else // si no lo encontramos
        {
            Debug.LogError($"No se ha encontrado {ball.GetGameObject().name} en ningún nodo!"); // tiramos error en consola
        }

        //Destroy(auxNode.element.gameObject);
        return auxNode.element;
    }

    public IBall DesqueueBottom() // para quitar y que nos devuleva el utlimo elemento
    {
        if (!IsEmpty())
        {
            NodeBall auxNode = rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode

            while (auxNode.nextNode != null) // nos fijamos si es el ultimo
            {
                auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
            }

            if (auxNode != rootNode)
            {
                IBall auxBall = auxNode.element; // nos guardamos la referencia a la Sphere guarada en el auxNode
                auxNode.element.GetGameObject().SetActive(false);
                auxNode.previousNode.nextNode = null; // decimos que el siguiente del previo del auxNode es null
                return auxBall;  // devolvemos la Sphere
            }
            else
            {
                IBall auxBall = rootNode.element; // nos guardamos la referencia a la Sphere guarada en el auxNode
                rootNode.element.GetGameObject().SetActive(false);
                rootNode = null; // decimos que el siguiente del previo del auxNode es null
                return auxBall;  // devolvemos la Sphere
            }
        }
        else
        {
            return null;
        }
    }

    public bool IsEmpty()
    {
        return (rootNode == null); // solo devuelve true cuando el rootNode es null, sino la cola tiene al menos un elemento
    }

    public NodeBall FindNode(IBall ball) //Recibe una pelota y le busca el nodo
    {
        var auxNode = rootNode;

        while (auxNode.element != ball && auxNode.nextNode != null)
        {
            auxNode = auxNode.nextNode;
        }
        if (auxNode.element == ball)
            return auxNode;
        else
            return null;
    }
}
