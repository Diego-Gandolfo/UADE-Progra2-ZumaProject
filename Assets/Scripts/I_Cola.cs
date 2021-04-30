using System;
using System.Collections.Generic;
using System.Text;

namespace TDA_COLA
{
    interface I_Cola<Type> // Le agregué el <Type> para que pueda aceptar cualquier tipo de dato
    {
        void InicializarCola(int valor);

        void Acolar(Type type);

        void Desacolar();

        bool ColaVacia();

        Type Primero();
    }
}
