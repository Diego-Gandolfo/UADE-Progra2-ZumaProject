using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    [SerializeField] private Color[] colorBucket = new Color[3];
    [SerializeField] private float speed = 10;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject deathPrefab;
    [SerializeField] private GameObject absorbPrefab;
    private float lifeTimeTimer;
    
    public QueueDynamicController QueueController { get; private set; }

    public BallShowQueue BallSQ { get; private set; }

    public Color Color { get; private set; }

    public bool IsProjectile { get; set; }
    public int IndexValue { get; private set; }

    private void Awake()
    {
        IndexValue = (int) Random.Range(0, colorBucket.Length);
        Color = colorBucket[IndexValue];
        gameObject.GetComponent<SpriteRenderer>().color = Color;
        lifeTimeTimer = lifeTime;
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

            if (collisionBall != null)
            {
                QueueController = collisionBall.QueueController;

                Vector2 contactOnCollision = collision.GetContact(0).point;//Encuentra el punto de colision (devuelve un vector)
                contactOnCollision.x -= collision.transform.position.x;

                if (contactOnCollision.x > 0)
                {
                    QueueController.EnqueueMiddleAfter(this, collisionBall);
                    ResetOnCollision();
                }
                else if (contactOnCollision.x <= 0)
                {
                    QueueController.EnqueueMiddleBefore(this, collisionBall);
                    ResetOnCollision();
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

    public void OnExplosion()
    {
        var death = Instantiate(deathPrefab, transform.position, transform.rotation);
        death.GetComponent<DeathController>().SetColor(Color);
    }

    public void OnAbsorb()
    {
        print("entre");
        Instantiate(absorbPrefab, transform.position, transform.rotation);
        //var absorb = Instantiate(absorbPrefab, transform.position, transform.rotation);
        //absorb.GetComponent<DeathController>().SetColor(Color);
    }
}
