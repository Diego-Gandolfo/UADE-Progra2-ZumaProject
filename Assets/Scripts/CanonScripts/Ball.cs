using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Color[] colorBucket = new Color[3];
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed = 10;

    private float lifeTimeTimer;
    private QueueDynamicController queueController;

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
            Vector2 contactOnCollision = collision.GetContact(0).point;//Encuentra el punto de colision (devuelve un vector)
            contactOnCollision.x -= collision.transform.position.x;
            if (contactOnCollision.x > 0)
            {
                var afterBall = collision.gameObject.GetComponent<Ball>();
                queueController.EnqueueMiddleAfter(this, afterBall);
                ResetOnCollision();
            }
            else if (contactOnCollision.x <= 0)
            {
                var beforeBall = collision.gameObject.GetComponent<Ball>();
                queueController.EnqueueMiddleBefore(this, beforeBall);
                ResetOnCollision();
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
        this.queueController = queueController;
    }
}
