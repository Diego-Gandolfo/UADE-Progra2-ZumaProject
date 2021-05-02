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

    private void Start()
    {
        queueManager = FindObjectOfType<QueueShootingTest>();
        player = FindObjectOfType<CanonQueue>().transform;
        rb2d = GetComponent<Rigidbody2D>();
        //ballColliderRadius = GetComponent<CircleCollider2D>().radius;
        if (!imProyectile)
        {
            playerPos = player.transform.position;
        }
    }

    private void Update()
    {
        if (imProyectile)
        {
            transform.position += transform.up * speed * Time.deltaTime; 
        }
       
        // transform.LookAt(player);
        //transform.rotation.x = Quaternion.identity;
     //if(player != null && !imProyectile)
     //  {
     //   playerPos = player.position;       
     //   playerPos.x = playerPos.x - transform.position.x;
     //   playerPos.y = playerPos.y - transform.position.y;
     //   angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
     //   transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
     //   }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (imProyectile)
        {
            Vector2 contactOnCollision = collision.GetContact(0).point;//Encuentra el punto de colision (devuelve un vector)
            contactOnCollision.x -= collision.transform.position.x;
            Debug.Log(contactOnCollision);
            if (contactOnCollision.x > 0)
            {
                var afterBall = collision.gameObject.GetComponent<Ball>();
                queueManager.EnqueueMiddleAfter(this, afterBall); 
                Debug.Log($"{this.name} estoy del lado Der de {afterBall.name}");
                //transform.position = new Vector3(collision.transform.position.x + 1 , collision.transform.position.y,0);
                ResetOnCollision();
            }
            else if (contactOnCollision.x <= 0)
            {
                var beforeBall = collision.gameObject.GetComponent<Ball>();
                queueManager.EnqueueMiddleBefore(this, beforeBall);
                Debug.Log($"{this.name} estoy del lado Izq {beforeBall.name}");
                //transform.position = new Vector3(collision.transform.position.x - 1, collision.transform.position.y, 0);
                ResetOnCollision();
            }
        }
    }

    private void ResetOnCollision()
    {
        imProyectile = false;
        speed = 0;
        transform.rotation = Quaternion.identity;
    }
}
