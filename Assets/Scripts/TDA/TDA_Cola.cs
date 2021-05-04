using System;
using System.Collections.Generic;
using System.Text;

namespace TDA_COLA
{
    public class TDA_Cola: I_Cola<int> // Como la I_Cola ahora tiene un <Type> hay que indicarle el tipo de dato, por eso tiene el <int>
    {
        int[] a;
        int indice;

        public TDA_Cola(int valor)
        {
            InicializarCola(valor);
        }


        public void InicializarCola(int valor)
        {
            a = new int[valor];
            indice = 0;
        }

        public void Acolar(int x)
        {
            for (int i = (indice - 1); i >= 0; i++)
            {
                a[i + 1] = a[i];
            }
            a[0] = x;
            indice++;
        }

        public void Desacolar()
        {
            indice--;
        }

        public bool ColaVacia()
        {
            return (indice == 0);
        }

        public int Primero()
        {
            return a[indice - 1];
        }
    }
}