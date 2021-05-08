using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBall // Esta clase también la hice para que puedan ser Nodos de cualquier tipo de dato, creo que funciona pero lo tengo que probar
{
    public Ball element; // Este es el elemento del Nodo, en nuestro caso son los Spheres quiero ver si esto funciona, sino lo simplifico a que funcione solo con Spheres y listo!

    public NodeBall previousNode; // Necesitamos tener referencia también al Nodo anterior, para poder eliminar una del medio
    public NodeBall nextNode; // Referencia al Nodo siguiente

    public void InitializeNode(Ball element, NodeBall previousNode, NodeBall nextNode) // Para inicializar todos los datos del nodo
    {
        this.element = element;
        this.previousNode = previousNode;
        this.nextNode = nextNode;
    }
}

