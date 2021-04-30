using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallQueue : MonoBehaviour,IQueueDynamic<Ball>
{
    public Node<Ball> rootNode; // Acá creo el Nodo Raíz, que va a trabajar con el tipo de dato Balls

    public void Initialize(Ball ball) // Para inicializar la Cola, recibe una Ball
    {
        Node<Ball> newNode = new Node<Ball>(); // creamos un nuevo Nodo
        newNode.InitializeNode(ball, null, null); // lo inicializa con ball como su elemento y tanto su previo como el siguiente son null
        rootNode = newNode; // decimos que el Nodo Raíz es el nuevo Nodo creado
    }

    public void EnqueueTop(Ball ball) // Acá metemos en la cola un Ball como primer elemento
    {
        Node<Ball> newNode = new Node<Ball>(); // creamos un nuevo elemento
        newNode.InitializeNode(ball, null, rootNode); // lo inicializa con ball como su elemento, el siguiente es el que era el Nodo Raíz y el anterior es null
        rootNode.InitializeNode(rootNode.element, newNode, rootNode.nextNode); // cambiamos los valores al Nodo que está referenciado en rootNode para que su elemento siga siendo el mismo, pero que ahora su previo sea el nuevo Nodo
        rootNode = newNode; // decimos que el Nodo Raíz es el nuevo Nodo creado
    }
    public void EnqueueBottom(Ball ball) // este sirve para meter un Ball al final de la cola
    {
        Node<Ball> auxNode = rootNode; // creamos un nodo Auxiliar y nos guardamos la referencia al rootNode

        while (auxNode.nextNode != null) // nos fijamos si es el ultimo
        {
            auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
        }

        Node<Ball> newNode = new Node<Ball>(); // creamos un nuevo nodo
        newNode.InitializeNode(ball, auxNode, null); // inicializamos el newNode con ball como su elemento, auxNode como el previo y null como siguiente
        auxNode.nextNode = newNode; // indicamos que el siguente del auxNode es el newNode
    }
    public void EnqueueMiddleAfter(Ball ball, Ball afterSphere) // esta sirve para meter una Ball despues del nodo que tiene la afterSphere
    {
        Node<Ball> auxNode = rootNode; // creamos un auxNode y guaramos la referencia al rootNode

        while (auxNode.element != afterSphere && auxNode.nextNode != null) // bucle donde nos fijamos si el elementos del auxNode es el afterSphere o si llegamos al final
        {
            auxNode = auxNode.nextNode; // si no es, almacenamos en auxNode el nodo siguiente de auxNode y vuelve al bucle
        }

        if (auxNode.nextNode != null) // si lo encontramos
        {
            Node<Ball> newNode = new Node<Ball>(); // creamos un newNode
            newNode.InitializeNode(ball, auxNode, auxNode.nextNode); // lo inicializamos con la ball como su elemento, el auxNode como su previo y el siguiente del auxNode como su siguiente

            auxNode.nextNode.previousNode = newNode; // ahora el nodo previo del nodo siguiente de auxNode es el newNode
            auxNode.nextNode = newNode; // y el nodo siguiente del auxNode es el newNode
        }
        else // si no lo encontramos
        {
            Debug.LogError($"No se ha encontrado {afterSphere} en ningún nodo!"); // tiramos error en consola
        }
    }

    public void EnqueueMiddleBefore(Ball ball, Ball beforeSphere) // esta sirve para meter una Ball antes del nodo que tiene la beforeSphere
    {
        Node<Ball> auxNode = rootNode; // creamos un auxNode y guaramos la referencia al rootNode

        while (auxNode.element != beforeSphere && auxNode.nextNode != null) // bucle donde nos fijamos si el elementos del auxNode es el beforeSphere o si llegamos al final
        {
            auxNode = auxNode.nextNode; // si no es, almacenamos en auxNode el nodo siguiente de auxNode y vuelve al bucle
        }

        if (auxNode.nextNode != null) // si lo encontramos
        {
            Node<Ball> newNode = new Node<Ball>(); // creamos un newNode
            newNode.InitializeNode(ball, auxNode.previousNode, auxNode);// lo inicializamos con la ball como su elemento, el nodo previo del auxNode como su previo y el auxNode como su siguiente

            auxNode.previousNode.nextNode = newNode; // ahora el nodo siguiente del nodo previo de auxNode es el newNode
            auxNode.previousNode = newNode; // y el nodo previo del auxNode es el newNode
        }
        else // si no lo encontramos
        {
            Debug.LogError($"No se ha encontrado {beforeSphere} en ningún nodo!"); // tiramos error en consola
        }
    }


    public Ball DesqueueTop() // para sacar y devolver el primer elemento
    {
        Ball auxSphere = rootNode.element; // nos guardamos la referencia a la Ball guarada en el rootNode
        rootNode.nextNode.previousNode.element.gameObject.SetActive(false);
        rootNode.nextNode.previousNode = null; // decimos que el nodo previo del nodo siguiente de rootNode es null, antes referenciaba a rootNode
        rootNode = rootNode.nextNode; // decimos que el nuevo rootNode es el qe antes era el siguiente del rootNode
        return auxSphere; // devolvemos la Ball
    }

    public void DesqueueMiddle(Ball ball) // para quitar un elemento de la mitad de la cola
    {
        Node<Ball> auxNode = rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode

        while (auxNode.element != ball && auxNode.nextNode != null) // bucle donde nos fijamos si el elemento del auxNode es el ball o si llegamos al final
        {
            auxNode = auxNode.nextNode; // si no es, almacenamos en auxNode el nodo siguiente de auxNode y vuelve al bucle
        }

        if (auxNode.nextNode != null) // si lo encontramos
        {
            auxNode.element.gameObject.SetActive(false);
            auxNode.previousNode.nextNode = auxNode.nextNode; // decimos que el siguiente del previo del auxNode ahora es el siguiente del auxNode
            auxNode.nextNode.previousNode = auxNode.previousNode; // decimos que el previo del siguiente del auxNode es ahora el previo del auxNode
        }
        else // si no lo encontramos
        {
            Debug.LogError($"No se ha encontrado {ball} en ningún nodo!"); // tiramos error en consola
        }
    }

    public Ball DesqueueBottom() // para quitar y que nos devuleva el utlimo elemento
    {
        Node<Ball> auxNode = rootNode; // creamos un nodo auxiliar y le asignamos la referencia del rootNode

        while (auxNode.nextNode != null) // nos fijamos si es el ultimo
        {
            auxNode = auxNode.nextNode; // sino guardamos el siguiente en auxNode y repetimos
        }

        Ball auxSphere = auxNode.element; // nos guardamos la referencia a la Ball guarada en el auxNode
        auxNode.previousNode.nextNode.element.gameObject.SetActive(false);
        auxNode.previousNode.nextNode = null; // decimos que el siguiente del previo del auxNode es null
        return auxSphere;  // devolvemos la Ball
    }

    public bool IsEmpty()
    {
        return (rootNode == null); // solo devuelve true cuando el rootNode es null, sino la cola tiene al menos un elemento
    }
}
