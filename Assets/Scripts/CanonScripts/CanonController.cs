using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour
{
	[Header("Shoot Settings")]
	[SerializeField] SpriteRenderer currentBall;
	[SerializeField] SpriteRenderer nextBall;
	[SerializeField] private float minDistanceToPoint;


	[Header("Raycast Settings")]
	[SerializeField] private float rayLenght = 5f;
	private Transform raycastPoint;
	private Vector2 actualPositionMouse;
	private RaycastHit2D hit2D;
	private LineRenderer laser;
	private Vector2 direction;

	//Scripts
	private CanonQueue canonQueue;
	private CanonStack canonStack;
	private Ball projectile;
	void Start()
    {
		canonStack = gameObject.GetComponent<CanonStack>();
		canonQueue = gameObject.GetComponent<CanonQueue>();
		raycastPoint = currentBall.gameObject.transform;
		laser = GetComponent<LineRenderer>();
        laser.useWorldSpace = true;
		laser.enabled = true;
		SetColors();
	}

    void Update()
    {
        if (!GameManager.instance.IsGameFreeze)
        {
			actualPositionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			direction = actualPositionMouse - (Vector2)raycastPoint.position;
			var distance = Vector3.Distance(transform.position, actualPositionMouse);
			if (distance >= minDistanceToPoint)
			{
				transform.up = direction.normalized;
			}

			CastLaser();

			if (Input.GetKeyDown(KeyCode.Mouse0)) //SHOOT
			{
				Shoot();
				SetColors();
			}

			if (Input.GetKeyDown(KeyCode.Mouse1)) //ABSORB
			{
				hit2D = Physics2D.Raycast(raycastPoint.position, direction);
				if (hit2D)
				{
					canonStack.Absorb(hit2D.collider.GetComponent<Ball>());
					SetColors();
				}
			}
		}
	}

	void CastLaser()
	{
		laser.SetPosition(0, raycastPoint.position);
		laser.SetPosition(1, actualPositionMouse);
	}

	public void SetColors() // Esto se haria una vez que se dispara, para chequear si sigue habiendo cosas en stack
	{
		if (canonStack.IsEmpty())
		{
			currentBall.color = canonQueue.Peek().Color;
			nextBall.color = canonQueue.PeekPreviousColor();
		}
		else
		{
			currentBall.color = canonStack.Peek().Color;
            if (canonStack.GetIndex() > 1)
                nextBall.color = canonStack.PeekPreviousColor();
            else
                nextBall.color = canonQueue.Peek().Color; // cambie esta, porque sacaba el PreviousColor de la Cola, pero tiene que sacar el primero
		}
	}

	public void Shoot()
	{
		if (canonStack.IsEmpty())
        {
			projectile = canonQueue.Dequeue();
			canonQueue.InstanceProyectile();
        } else
        {
			projectile = canonStack.Pop();
        }

		projectile.gameObject.SetActive(true);
		projectile.transform.position = currentBall.transform.position;
		projectile.transform.rotation = currentBall.transform.rotation;
		projectile.IsProjectile = true;
		projectile = null;
	}
}
