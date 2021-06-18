﻿using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    [SerializeField] private Color[] colorBucket = new Color[3];
    [SerializeField] private float speed = 10;
    [SerializeField] private float lifeTime;
    private float lifeTimeTimer;

    public QueueDynamicController QueueController { get; private set; }

    public Color Color { get; private set; }

    public bool IsProjectile { get; set; }
    public int IndexValue { get; private set; }

    private void Awake()
    {
        IndexValue = (int) Random.Range(0, colorBucket.Length);
        Color = colorBucket[IndexValue];
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
            if (ball != null)
            {
                print("Es pelota");
                QueueController = collision.gameObject.GetComponent<Ball>().QueueController;
                ResetOnCollision();

                Vector2 contactOnCollision = collision.GetContact(0).point;//Encuentra el punto de colision (devuelve un vector)
                contactOnCollision.x -= collision.transform.position.x;
                if (contactOnCollision.x > 0)
                {
                    var afterBall = collision.gameObject.GetComponent<Ball>();
                    QueueController.EnqueueMiddleAfter(this, afterBall);
                }
                else if (contactOnCollision.x <= 0)
                {
                    var beforeBall = collision.gameObject.GetComponent<Ball>();
                    QueueController.EnqueueMiddleBefore(this, beforeBall);
                }
            }
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

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
