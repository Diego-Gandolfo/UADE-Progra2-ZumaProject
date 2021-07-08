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
    private float lifeTimeTimer;
    
    public QueueDynamicController QueueController { get; private set; }

    public BallShowQueue BallSQ { get; private set; }

    public Color Color { get; private set; }

    public bool IsProjectile { get; set; }
    public int IndexValue { get; private set; }

    private Animator animator;
    private Material material;

    private void Awake()
    {
        BallSQ = GetComponent<BallShowQueue>();
        IndexValue = (int) Random.Range(0, colorBucket.Length); //Color random de las opciones
        Color = colorBucket[IndexValue]; //Seteamos el color
        material = GetComponent<SpriteRenderer>().material; //Buscamos el material
        material.SetColor("_Color", Color); //Lo pintamos
        animator = GetComponent<Animator>();
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

        if (Input.GetKeyDown(KeyCode.Space))
            OnExplosion();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsProjectile)
        {
            var collisionBall = collision.gameObject.GetComponent<Ball>();

            if (collisionBall != null)
            {
                QueueController = collisionBall.QueueController;
                BallSQ.SetQueueController(QueueController);

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
        BallSQ.SetQueueController(queueController);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OnExplosion()
    {
        animator.SetBool("CanDestroy", true);
    }

    public void OnExploionDestroy()
    {
        QueueController.DesqueueMiddle(this);
        print("MANIP");
        //TODO: Let queue controller know that can continue with regroup and re check
        Destroy(this);
    }

    public void OnAbsorbDeque()
    {
        var node = QueueController.DesqueueMiddle(this).BallSQ.Node;
        var nextNode = node.nextNode;
        var previousNode = node.previousNode;

        if(nextNode != null) nextNode.element.BallSQ.Regroup(1);

        QueueController.CanCheckColorsAgain(nextNode, previousNode); //Acá hacemos el check de colores

        transform.position = new Vector3(0f, 0f, 0f);
        animator.SetTrigger("Reset");
        gameObject.SetActive(false);
    }

    public void OnAbsorb()
    {
        animator.SetBool("CanAbsorb", true);
    }
}
