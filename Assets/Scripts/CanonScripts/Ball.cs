using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] public bool imProyectile;
    [SerializeField] private Transform player;
    private float ballColliderRadius;
    private Collider2D ballCollider;
    private float halfBallColliderRadius;
    private float angle;
    private  Vector3 playerPos;
    public float speed = 0f;
    private Rigidbody2D rb2d;
    private QueueShootingTest queueManager;
    private CanonQueue canon;
    [SerializeField] private Color[] colorBucket = new Color[3];
    [SerializeField]private float lifeTime;
    private float lifeTimeTimer;
    private GameManager gameManager;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = colorBucket[Random.Range(0, colorBucket.Length)];
        rb2d = GetComponent<Rigidbody2D>();
        queueManager = FindObjectOfType<QueueShootingTest>();
        canon = FindObjectOfType<CanonQueue>();
        gameManager = FindObjectOfType<GameManager>();
        player = canon.transform;
        if (!imProyectile)
        {
            playerPos = player.transform.position;
        }
        lifeTimeTimer = lifeTime;
    }

    private void Update()
    {
        if (imProyectile)
        {
            transform.position += transform.up * speed * Time.deltaTime;
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
        //Debug.Log($"Entre en el collision con {collision}");
        if (imProyectile)
        {
            Vector2 contactOnCollision = collision.GetContact(0).point;//Encuentra el punto de colision (devuelve un vector)
            contactOnCollision.x -= collision.transform.position.x;
            //Debug.Log(contactOnCollision);
            if (contactOnCollision.x > 0)
            {
                var afterBall = collision.gameObject.GetComponent<Ball>();
                queueManager.EnqueueMiddleAfter(this, afterBall);
                gameManager.CheckColors(this);
                //Debug.Log($"{this.name} estoy del lado Der de {afterBall.name}");
                ResetOnCollision();
            }
            else if (contactOnCollision.x <= 0)
            {
                var beforeBall = collision.gameObject.GetComponent<Ball>();
                queueManager.EnqueueMiddleBefore(this, beforeBall);
                gameManager.CheckColors(this);
                //Debug.Log($"{this.name} estoy del lado Izq {beforeBall.name}");
                ResetOnCollision();
            }
        }
    }

    private void ResetOnCollision()
    {
        imProyectile = false;
        speed = 0;
        transform.rotation = Quaternion.identity;
        canon.InstanceProyectile();
    }
    private void OnColorMatch(GameObject objectToDestroy)
    {
       if(gameObject.GetComponent<SpriteRenderer>().color == objectToDestroy.GetComponent<SpriteRenderer>().color)
        {
            Destroy(objectToDestroy);
            Destroy(gameObject);
        }
    }

    
}
