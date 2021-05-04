using System;
using System.Collections.Generic;
using System.Text;
interface IQueue<Type> // Le agregué el <Type> para que pueda aceptar cualquier tipo de dato
{
    void Initialize(int valor);

    void Enqueue(Type type);

    Type Dequeue();

    bool IsEmpty();

    Type Peek();
}