using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour, IStack<Ball>
{
    private Ball [] a;
    private int max_quantity;
    private int index;

    public void Initialize(int cantidad)
    {
        max_quantity = cantidad;
        a = new Ball[cantidad];
        index = 0;
    }

    public void Push(Ball x)
    {
        if (index <= max_quantity)
        {
            a[index] = x;
            index++;
        }
    }

    public Ball Pop()
    {
        if (!IsEmpty())
        {
            var ball = a[index - 1];
            index--;
            return ball;
        }
        return null;
    }
    public bool IsEmpty()
    {
        return (index == 0);
    }

    public Ball Peek()
    {
        if (!IsEmpty())
        {
            return a[index - 1];
        }
        return null;
    }

    public int Index()
    {
        return index;
    }
}