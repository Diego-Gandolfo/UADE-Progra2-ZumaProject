using System;
using System.Collections.Generic;
using System.Text;

namespace TDA_COLA
{
    interface IQueuePriority<Type> // Le agregué el <Type> para que pueda aceptar cualquier tipo de dato
    {
        // inicializa la estructura
        void InicializarCola(int valor);

        // ingresa un elemento a la estructura, ordenandolo por prioridad
        void AcolarPrioridad(Type type);

        // elimina el "primer" valor de la estructura (el proximo a salir, el de mayor prioridad) 
        void Desacolar();

        // indica si hay elementos en la estructura
        bool ColaVacia();

        // devuelve el "primer" valor de la estructura (el proximo a salir, el de mayor prioridad)
        Type Primero();

        // devuelve la prioridad del "primer" valor de la estructura (el proximo a salir, el de mayor prioridad)
        int Prioridad();
    }
}
