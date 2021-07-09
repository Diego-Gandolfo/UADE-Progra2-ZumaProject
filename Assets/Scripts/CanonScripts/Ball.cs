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
    private Animator animator;
    private Material material;

    public QueueDynamicController QueueController { get; private set; }

    public BallShowQueue BallSQ { get; private set; }

    public Color Color { get; private set; }

    public bool IsProjectile { get; set; }
    public int IndexValue { get; private set; }

    //ONLY FOR EXPLODE
    private int explosionNumber;
    private NodeBall nextNode;
    private NodeBall previousNode;
    public System.Action<int, Ball, NodeBall, NodeBall> OnDestroyed; //numero de explotadas, this, previousNode, nextNode

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

    public void OnExplosion(int number, NodeBall previous, NodeBall next)
    {
        explosionNumber = number;
        previousNode = previous;
        nextNode = next;
        animator.SetBool("CanDestroy", true);
        AudioManager.instance.PlaySound(SoundClips.Explosion);
    }

    public void OnExplosionDestroy()
    {
        QueueController.DesqueueMiddle(this);
        OnDestroyed?.Invoke(explosionNumber, this, previousNode, nextNode);
        Destroy(this);
    }

    public void OnAbsorb()
    {
        animator.SetBool("CanAbsorb", true);
    }

    public void OnAbsorbDeque() //Esto lo triggerea el final de la animacion de Absorb
    {
        QueueController.DesqueueMiddle(this); //Lo saca de la cola

        if(BallSQ.Node.nextNode != null) BallSQ.Node.nextNode.element.BallSQ.Regroup(1); //Hace que se corran si hay algo a la derecha

        QueueController.CanCheckColorsAgain(BallSQ.Node.nextNode, BallSQ.Node.previousNode); //Acá hacemos el check de colores

        transform.position = new Vector3(0f, 0f, 0f);
        animator.SetTrigger("Reset"); //Reseteamos la animacion
        gameObject.SetActive(false); 
    }
}