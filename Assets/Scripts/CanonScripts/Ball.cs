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

            Debug.Log(contactOnCollision);
            if (imProyectile && contactOnCollision.x > 0)
            {
                queueManager.EnqueueTop();
                Debug.Log("Estoy del lado Der");
                imProyectile = false;
                transform.position = new Vector3(collision.transform.position.x + 1 , collision.transform.position.y,0);
                speed = 0;
            }
            else if (imProyectile && contactOnCollision.x <= 0)
            {
                queueManager.EnqueueBottom();
                transform.position = new Vector3(collision.transform.position.x - 1, collision.transform.position.y, 0);
                imProyectile = false;
                speed = 0;
            }
        }
    }
}
