using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStack<Type> // Le agregué el <Type> para que pueda aceptar cualquier tipo de dato
{
    void Initialize(int value);
    void Push(Type type);
    Type Pop();
    bool IsEmpty();
    Type Peek();
}
