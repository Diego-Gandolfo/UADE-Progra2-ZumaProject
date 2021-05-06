﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour
{
	[Header("Shoot Settings")]
	[SerializeField] SpriteRenderer currentBall;
	[SerializeField] SpriteRenderer nextBall;

	[Header("Raycast Settings")]
	[SerializeField] private float rayLenght = 5f;
	//[SerializeField] private LayerMask layersToHit;
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
		CheckColor();
	}

    void Update()
    {
		actualPositionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		direction = actualPositionMouse - (Vector2)raycastPoint.position;
		direction.Normalize();
		transform.up = direction;

		CastLaser(); 

        if (Input.GetKeyDown(KeyCode.Mouse0)) //SHOOT
        {
			Shoot();
			CheckColor();
		}

		if (Input.GetKeyDown(KeyCode.Mouse1)) //ABSORB
		{
			hit2D = Physics2D.Raycast(raycastPoint.position, direction);
			if (hit2D)
			{
				canonStack.Absorb(hit2D.collider.GetComponent<Ball>());
				CheckColor();
			}
		}
	}

	void CastLaser()
	{
		laser.SetPosition(0, raycastPoint.position);
		laser.SetPosition(1, actualPositionMouse);
	}

	public void CheckColor() //TODO: cambiar nombre a SetColor
	{ // Esto se haria una vez que se dispara, para chequear si sigue habiendo cosas en stack
		if (canonStack.IsEmpty())
		{
			currentBall.color = canonQueue.Peek().Color;
			nextBall.color = canonQueue.PeekPreviousColor(); // esto se podría hacer como en la linea 72 y nos ahorramos un metodo
		}
		else
		{
			currentBall.color = canonStack.Peek().Color;
            if (canonStack.CheckNumber() > 1)
                nextBall.color = canonStack.PeekPreviousColor(); // esto se podría hacer como en la linea 77 y nos ahorramos un metodo
            else
                nextBall.color = canonQueue.PeekPreviousColor(); // esto se podría hacer como en la linea 72 y nos ahorramos un metodo
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

		projectile.transform.position = currentBall.transform.position;
		projectile.transform.rotation = currentBall.transform.rotation;
		projectile.IsProjectile = true;
		projectile = null;
	}
}
