using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Pila<Type> // Le agregué el <Type> para que pueda aceptar cualquier tipo de dato
{
    void InicializarPila(int value);
    void Apilar(Type type);
    void Desapilar();
    bool PilaVacia();
    Type Tope();
}
