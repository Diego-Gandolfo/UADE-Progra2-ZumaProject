using System;
using System.Collections.Generic;
using System.Text;

public class Queue : IQueue<Ball> // Como la I_Cola ahora tiene un <Type> hay que indicarle el tipo de dato, por eso tiene el <int>
{
    Ball[] a;
    int index;

    public Queue(int valor)
    {
        Initialize(valor);
    }

    public void Initialize(int valor)
    {
        a = new Ball[valor];
        index = 0;
    }

    public void Enqueue(Ball x)
    {
        for (int i = (index - 1); i >= 0; i--)
        {
            a[i + 1] = a[i];
        }
        a[0] = x;
        index++;
    }

    public Ball Dequeue()
    {
        var ball = a[index - 1];
        index--;
        return ball;
    }

    public bool IsEmpty()
    {
        return (index == 0);
    }

    public Ball Peek()
    {
        return a[index - 1];
    }
}