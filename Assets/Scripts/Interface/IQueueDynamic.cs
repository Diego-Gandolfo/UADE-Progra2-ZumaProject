using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQueueDynamic<Type> // Esta armado para que la Interfaz pueda aceptar cualquier tipo de dato, eso se hace con el <Type>
{
    void Initialize(Type type); // En el Initialize hay que pasarle un dato del tipo de dato que usa la Cola para poder inicializarla, por eso el tipo de dato es Type
    void EnqueueTop(Type type); // Agrega un elemento al principio de la cola
    void EnqueueMiddleAfter(Type type, Type afrerType); // Recibe dos elementos, el que tiene que ingresar y después de cual lo tiene que agregar
    void EnqueueMiddleBefore(Type type, Type beforeType); // Recibe dos elementos, el que tiene que ingresar y antes de cual lo tiene que agregar
    void EnqueueBottom(Type type); // // Agrega un elemento al final de la cola
    Type DesqueueTop(); // Devuelve y saca el primer elemento (raiz)
    Type DesqueueMiddle(Type type); // Saca el elemento que se le pasa
    Type DesqueueBottom(); // Devuelve y saca el último elemento
    bool IsEmpty(); // Para preguntar si es que está vacía
}
