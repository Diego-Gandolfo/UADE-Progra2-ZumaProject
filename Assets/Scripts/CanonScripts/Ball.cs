﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Color[] colorBucket = new Color[3];
    [SerializeField] private float speed = 10;
    [SerializeField] private float lifeTime;
    private float lifeTimeTimer;

    public QueueDynamicController QueueController { get; private set; }

    public Color Color { get; private set; }

    public bool IsProjectile { get; set; }

    private void Awake()
    {
        Color = colorBucket[Random.Range(0, colorBucket.Length)];
        gameObject.GetComponent<SpriteRenderer>().color = Color;
        lifeTimeTimer = lifeTime;
    }

    private void Update()
    {
        if (IsProjectile)
        {
            transform.position += transform.up * speed * Time.deltaTime;
            lifeTimeTimer -= Time.deltaTime;           
        }

        if (lifeTimeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsProjectile)
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            var ballMovement = ball.GetComponent<BallMovement>();
            QueueController = ball.QueueController;
            NodeBall aux = null;

            Vector2 contactOnCollision = collision.GetContact(0).point;//Encuentra el punto de colision (devuelve un vector)
            contactOnCollision.x -= collision.transform.position.x;

            if (contactOnCollision.x > 0)
            {
                print("<<<<<< Desde la derecha");
                if (ballMovement != null)
                    aux = ball.GetComponent<BallMovement>().Node;//Si entro a la derecha, necesito correr a la derecha la pelota que sigue
                QueueController.EnqueueMiddleAfter(this, ball); 
                
            }
            else if (contactOnCollision.x <= 0)
            {
                print("Desde la izquierda >>>>");
                if (ballMovement != null)
                    if (ballMovement.Node.previousNode != null)
                        aux = ball.GetComponent<BallMovement>().Node.previousNode; //Corro a la derecha a la que entro en contacto
                QueueController.EnqueueMiddleBefore(this, ball);
            }

            if (aux != null)
                this.GetComponent<BallMovement>().GetTargetBallInfo(aux);

            ResetOnCollision();
        }
    }

    private void ResetOnCollision()
    {
        IsProjectile = false;
        transform.rotation = Quaternion.identity;
    }

    public void SetQueueController(QueueDynamicController queueController)
    {
        this.QueueController = queueController;
    }
}
