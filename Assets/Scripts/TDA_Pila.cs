using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class TDA_Pila : MonoBehaviour, I_Pila<Anillo>
{
    private Anillo [] a;
    private int cantidad_max;
    private int indice;

    public void InicializarPila(int cantidad)
    {
        cantidad_max = cantidad;
        a = new Anillo[cantidad];
        indice = 0;
    }

    public void Apilar(Anillo x)
    {
        if (indice <= cantidad_max)
        {
            a[indice] = x;
            indice++;
        }
    }

    public void Desapilar()
    {
        if (!PilaVacia())
        {
            indice--;
        }
    }
    public bool PilaVacia()
    {
        return (indice == 0);
    }

    public Anillo Tope()
    {
        if (PilaVacia())
        {
            return a[indice];
        }
        return a[indice - 1];
    }

    public int Indice()
    {
        return indice;
    }
}
*/