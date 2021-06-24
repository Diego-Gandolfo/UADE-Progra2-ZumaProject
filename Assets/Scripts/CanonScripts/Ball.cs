using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Color[] colorBucket = new Color[3];
    [SerializeField] private float speed = 10;
    [SerializeField] private float lifeTime;
    private float lifeTimeTimer;
    private BallMovement myBallMovement;
    
    public QueueDynamicController QueueController { get; private set; }

    public BallShowQueue BallSQ { get; private set; }

    public Color Color { get; private set; }

    public bool IsProjectile { get; set; }

    private void Awake()
    {
        Color = colorBucket[Random.Range(0, colorBucket.Length)];
        gameObject.GetComponent<SpriteRenderer>().color = Color;
        lifeTimeTimer = lifeTime;
        myBallMovement = this.GetComponent<BallMovement>();
        BallSQ = GetComponent<BallShowQueue>();
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
            var collisionBall = collision.gameObject.GetComponent<Ball>();
            var collisionBallMovement = collisionBall.GetComponent<BallMovement>();
            QueueController = collisionBall.QueueController;

            Vector2 contactOnCollision = collision.GetContact(0).point;//Encuentra el punto de colision (devuelve un vector)
            contactOnCollision.x -= collision.transform.position.x;

            if (contactOnCollision.x > 0)
            {
                print($"<<<<< Derecha - {collisionBall.name}-{this.name}");
                QueueController.EnqueueMiddleAfter(this, collisionBall);
                ResetOnCollision();
            }
            else if (contactOnCollision.x <= 0)
            {
                print($"Izquierda >>> - {this.name}-{collisionBall.name}");
                QueueController.EnqueueMiddleBefore(this, collisionBall);
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
        this.QueueController = queueController;
    }
}
