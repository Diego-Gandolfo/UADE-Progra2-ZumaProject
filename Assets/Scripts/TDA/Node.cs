using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<Type> // Esta clase también la hice para que puedan ser Nodos de cualquier tipo de dato, creo que funciona pero lo tengo que probar
{
    public Type element; // Este es el elemento del Nodo, en nuestro caso son los Spheres quiero ver si esto funciona, sino lo simplifico a que funcione solo con Spheres y listo!

    public Node<Type> previousNode; // Necesitamos tener referencia también al Nodo anterior, para poder eliminar una del medio
    public Node<Type> nextNode; // Referencia al Nodo siguiente

    public void InitializeNode(Type element, Node<Type> previousNode, Node<Type> nextNode) // Para inicializar todos los datos del nodo
    {
        this.element = element;
        this.previousNode = previousNode;
        this.nextNode = nextNode;
    }
}

