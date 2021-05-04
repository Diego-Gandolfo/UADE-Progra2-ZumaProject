using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Color[] colorBucket = new Color[3];
    [SerializeField] private float lifeTime;
    private float lifeTimeTimer;

    private QueueController queueController;
    private CanonQueue canon; //TODO: controlarlo en el CanonController con un cooldown

    public Color Color { get; private set; }
    public float Speed { get; set; }
    public bool IsProjectile { get; set; }


    private void Start()
    {
        Color = colorBucket[Random.Range(0, colorBucket.Length)];
        gameObject.GetComponent<SpriteRenderer>().color = Color;

        canon = FindObjectOfType<CanonQueue>();

        lifeTimeTimer = lifeTime;
    }

    private void Update()
    {
        if (IsProjectile)
        {
            transform.position += transform.up * Speed * Time.deltaTime;
            lifeTimeTimer -= Time.deltaTime;           
        }

        if (lifeTimeTimer <= 0)
        {
            canon.InstanceProyectile();
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
        Speed = 0;
        transform.rotation = Quaternion.identity;
        canon.InstanceProyectile();
    }

    public void SetQueueController(QueueController queueController)
    {
        this.queueController = queueController;
    }
}
