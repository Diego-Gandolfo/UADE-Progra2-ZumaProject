using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerics<Type> // Esta clase también la hice para que puedan ser Nodos de cualquier tipo de dato, creo que funciona pero lo tengo que probar
{
    public Type element; // Este es el elemento del Nodo, en nuestro caso son los Spheres quiero ver si esto funciona, sino lo simplifico a que funcione solo con Spheres y listo!

    public NodeGenerics<Type> previousNode; // Necesitamos tener referencia también al Nodo anterior, para poder eliminar una del medio
    public NodeGenerics<Type> nextNode; // Referencia al Nodo siguiente

    public void InitializeNode(Type element, NodeGenerics<Type> previousNode, NodeGenerics<Type> nextNode) // Para inicializar todos los datos del nodo
    {
        this.element = element;
        this.previousNode = previousNode;
        this.nextNode = nextNode;
    }
}

