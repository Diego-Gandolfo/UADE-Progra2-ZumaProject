﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABBNode
{
    public Ball info;

    public ABBNode hijoDer;
    public ABBNode hijoIzq;
}
public interface ABBTDA
{
    int Raiz();
    ABBNode HijoIzq();
    ABBNode HijoDer();
    bool ArbolVacio();
    void InicializarArbol();
    void AgregarElem(ref ABBNode n, Ball x);
    void EliminarElem(ref ABBNode n, Ball x);
}
public class ABBBall : MonoBehaviour, ABBTDA
{
    public ABBNode raiz;
    public void AgregarElem(ref ABBNode raiz, Ball x)
    {
        if (raiz == null)
        {
            raiz = new ABBNode();
            raiz.info = x;
        }
        else if (raiz.info.ColorValue() > x.ColorValue())
        {
            AgregarElem(ref raiz.hijoIzq, x);
        }
        else if (raiz.info.ColorValue() <= x.ColorValue())
        {
            AgregarElem(ref raiz.hijoDer, x);
        }
    }

    public bool ArbolVacio()
    {
        return (raiz == null);
    }

    public void EliminarElem(ref ABBNode n, Ball x)
    {
        if (raiz != null)
        {
            if (raiz.info == x && (raiz.hijoIzq == null) && (raiz.hijoDer == null))
            {
                raiz = null;
            }
            else if (raiz.info == x && raiz.hijoIzq != null)
            {
                raiz.info = this.mayor(raiz.hijoIzq);
                EliminarElem(ref raiz.hijoIzq, raiz.info);
            }
            else if (raiz.info == x && raiz.hijoIzq == null)
            {
                raiz.info = this.menor(raiz.hijoDer);
                EliminarElem(ref raiz.hijoDer, raiz.info);
            }
            else if (raiz.info.ColorValue() < x.ColorValue())
            {
                EliminarElem(ref raiz.hijoDer, x);
            }
            else
            {
                EliminarElem(ref raiz.hijoIzq, x);
            }
        }
    }

    public ABBNode HijoDer()
    {
        return raiz.hijoIzq;
    }

    public ABBNode HijoIzq()
    {
        return raiz.hijoDer;
    }

    public void InicializarArbol()
    {
        raiz = null;
    }

    public int Raiz()
    {
        return raiz.info.ColorValue();
    }

   public void preOrder(ABBNode a)
    {
        if (a != null)
        {
            print(a.info.ColorValue());
            preOrder(a.hijoIzq);
            preOrder(a.hijoDer);
        }
    }
    public void inOrder(ABBNode a,List<Ball> abbNode)
    {
        if (a != null)
        {
           
            inOrder(a.hijoIzq,abbNode);
            abbNode.Add(a.info);
            inOrder(a.hijoDer,abbNode);
        }
    }
    public void postOrder(ABBNode a)
    {
        if (a != null)
        {
            postOrder(a.hijoIzq);
            postOrder(a.hijoDer);
            // Console.WriteLine(a.info.ToString());    IMPLEMENTACION
        }
    }
    public void levelOrder(ABBNode nodo)
    {
        Queue<ABBNode> q = new Queue<ABBNode>();

        q.Enqueue(nodo);

        while (q.Count > 0)
        {
            nodo = q.Dequeue();

            //Console.WriteLine("Padre: " + nodo.info.ToString());    IMPLEMENTACION

            if (nodo.hijoIzq != null)
            {
                q.Enqueue(nodo.hijoIzq);
                // Console.WriteLine("Hijo Izq: " + nodo.hijoIzq.info.ToString());
            }
            else
            {
                //Console.WriteLine("Hijo Izq: null");
            }

            if (nodo.hijoDer != null)
            {
                q.Enqueue(nodo.hijoDer);
                //Console.WriteLine("Hijo Der: " + nodo.hijoDer.info.ToString());
            }
            else
            {
                //Console.WriteLine("Hijo Der: null");
            }
        }

    }
    public Ball mayor(ABBNode a)
    {
        if (a.hijoDer == null)
        {
            return a.info;
        }
        else
        {
            return mayor(a.hijoDer);
        }
    }

    public Ball menor(ABBNode a)
    {
        if (a.hijoIzq == null)
        {
            return a.info;
        }
        else
        {
            return menor(a.hijoIzq);
        }
    }
}
