using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    //[SerializeField] Transform[] testPath;
    private Transform[] path;
    private int currentPosition = 0;

    public float Speed { get; set ; }
    public bool CanMove { get; set; }

    void Start()
    {
   
    }

    void Update()
    {
        if (CanMove && currentPosition < path.Length)
        {
            var currentTarget = path[currentPosition].position;
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);

            var distance = Vector2.Distance(transform.position, currentTarget);
            if (distance < 0.1f)
            {
                currentPosition++;
            }
        }
    }

    public void GetPath(Transform[] recorrido)
    {
        path = recorrido;
        print("recorrido " +path.Length);
        currentPosition = 0;
        CanMove = true;
        this.transform.position = path[currentPosition].position;
        currentPosition++;
    }

}
