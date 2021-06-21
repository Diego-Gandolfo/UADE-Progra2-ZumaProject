using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float reduceSpeedMultiplier;
    [SerializeField] private float inreaseSpeedMultiplier;

    private Transform[] path;
    private int currentPosition = 0;

    public float Speed { get; set ; }
    public bool CanMove { get; set; }
    public NodeBall Node { get; set; }

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
        currentPosition = 0;
        CanMove = true;
        this.transform.position = path[currentPosition].position;
        currentPosition++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Node != null)
        {
            var ball = collision.gameObject.GetComponent<Ball>();

            if (ball != null)
            {
                if (!ball.IsProjectile)
                {
                    if (Node.nextNode != null)
                    {
                        if (collision.gameObject == Node.nextNode.element.gameObject)
                        {
                            CanMove = true;
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Node != null)
        {
            var ball = collision.gameObject.GetComponent<Ball>();

            if (ball != null)
            {
                if (!ball.IsProjectile)
                {
                    if (Node.nextNode != null)
                    {
                        if (collision.gameObject == Node.nextNode.element.gameObject)
                        {
                            CanMove = false;
                        }
                    }
                }
            }
        }
    }
}
