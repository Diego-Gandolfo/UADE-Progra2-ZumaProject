using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IConjuntoTDATransform
{
    void InicializarConjunto();
    bool ConjuntoVacio();
    void Agregar(int x);
    int Elegir();
    void Sacar(int x);
    bool Pertenece(int x);
}

public class NodoT
{
    public int info;
    public NodoT sig;
}

// IMPLEMENTACIÓN DINÁMICA //
public class TDA_ConjuntoTransform : IConjuntoTDATransform
{
    NodoT raiz;

    public void InicializarConjunto()
    {
        raiz = null;
    }

    public bool ConjuntoVacio()
    {
        return (raiz == null);
    }

    public void Agregar(int x)
    {
        /* Verifica que x no este en el conjunto */
        if (!this.Pertenece(x))
        {
            NodoT aux = new NodoT();
            aux.info = x;
            aux.sig = raiz;
            raiz = aux;
        }
    }

    public int Elegir()
    {
        return raiz.info;
    }

    public void Sacar(int x)
    {
        if (raiz != null)
        {
            // si es el primer elemento de la lista
            if (raiz.info == x)
            {
                raiz = raiz.sig;
            }
            else
            {
                NodoT aux = raiz;
                while (aux.sig != null && aux.sig.info != x)
                    aux = aux.sig;
                if (aux.sig != null)
                    aux.sig = aux.sig.sig;
            }
        }
    }

    public bool Pertenece(int x)
    {
        NodoT aux = raiz;
        while ((aux != null) && (aux.info != x))
        {
            aux = aux.sig;
        }
        return (aux != null);
    }
}

