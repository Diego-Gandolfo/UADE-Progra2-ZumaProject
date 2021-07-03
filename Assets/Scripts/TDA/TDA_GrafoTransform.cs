using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGrafoTDATransform
{
    void InicializarGrafo(int n);
    void AgregarVertice(int v);
    void EliminarVertice(int v);
    IConjuntoTDATransform Vertices();
    void AgregarArista(int v1, int v2, int peso);
    void EliminarArista(int v1, int v2);
    bool ExisteArista(int v1, int v2);
    int PesoArista(int v1,int v2);
}

public class TDA_GrafoTransform : IGrafoTDATransform
{
    public int[,] MAdy { get; private set; }
    public int[] Etiqs { get; private set; }
    public int CantNodos { get; private set; }

    public void InicializarGrafo(int n)
    {
        MAdy = new int[n, n];
        Etiqs = new int[n];
        CantNodos = 0;
    }

    public void AgregarVertice(int v)
    {
        Etiqs[CantNodos] = v;
        for (int i = 0; i <= CantNodos; i++)
        {
            MAdy[CantNodos, i] = 0;
            MAdy[i, CantNodos] = 0;
        }
        CantNodos++;
    }

    public void EliminarVertice(int v)
    {
        int ind = Vert2Indice(v);

        for (int k = 0; k < CantNodos; k++)
        {
            MAdy[k, ind] = MAdy[k, CantNodos - 1];
        }

        for (int k = 0; k < CantNodos; k++)
        {
            MAdy[ind, k] = MAdy[CantNodos - 1, k];
        }

        Etiqs[ind] = Etiqs[CantNodos - 1];
        CantNodos--;
    }

    public int Vert2Indice(int v)
    {
        int i = CantNodos - 1;
        while (i >= 0 && Etiqs[i] != v)
        {
            i--;
        }

        return i;
    }

    public IConjuntoTDATransform Vertices()
    {
        IConjuntoTDATransform Vert = new TDA_ConjuntoTransform();
        Vert.InicializarConjunto();
        for (int i = 0; i < CantNodos; i++)
        {
            Vert.Agregar(Etiqs[i]);
        }
        return Vert;
    }

    public void AgregarArista(int v1, int v2, int peso)
    {
        int o = Vert2Indice(v1);
        int d = Vert2Indice(v2);
        if (o != d) MAdy[o, d] = peso;
    }

    public void EliminarArista(int v1, int v2)
    {
        int o = Vert2Indice(v1);
        int d = Vert2Indice(v2);
        MAdy[o, d] = 0;
    }

    public bool ExisteArista(int v1, int v2)
    {
        int o = Vert2Indice(v1);
        int d = Vert2Indice(v2);
        return MAdy[o, d] != 0;
    }

    public int PesoArista(int v1, int v2)
    {
        int o = Vert2Indice(v1);
        int d = Vert2Indice(v2);
        return MAdy[o, d];
    }
}
