using System;
using System.Collections.Generic;
using System.Text;
/*
namespace TDA_COLA
{
    class TDA_ColaPrioridad : I_ColaPrioridad<Jugador>
    {
        Jugador[] elementos;
        int indice; 

        public void InicializarCola(int valor)
        {
            elementos = new Jugador[valor];
            indice = 0;
        }
        public void AcolarPrioridad(Jugador x)
        {
            int j;
            // al ingresar cada elemento se ingresa en el orden de acuerdo a su prioridad
            for (j = indice; j > 0 && elementos[j - 1].Puntaje >= x.Puntaje; j--)
            {
                elementos[j] = elementos[j - 1];
            }
            elementos[j] = x;

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

        public Jugador Primero()
        {
            return elementos[indice - 1];
        }

        public int Prioridad()
        {
            return elementos[indice - 1].Puntaje;
        }
    }
}
*/