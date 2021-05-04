using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour
{
	[Header("Shoot Settings")]
	[SerializeField] private Transform shootPoint;
	[SerializeField] private float shootSpeed;

	[Header("Raycast Settings")]
	[SerializeField] private float rayLenght = 5f;
	[SerializeField] private Transform raycastPoint;
	//[SerializeField] private LayerMask layersToHit;
	private Vector2 actualPositionMouse;
	private RaycastHit2D hit2D;
	private LineRenderer laser;
	private Vector2 direction;

	//Scripts
	private TestQueueDynamic queueDymamic;
	private CanonQueue canonQueue;
	private CanonStack canonStack;
	private Ball proyectile;

	void Start()
    {
		canonStack = gameObject.GetComponent<CanonStack>();
		canonQueue = gameObject.GetComponent<CanonQueue>();

		//LASER
        laser = GetComponent<LineRenderer>();
        laser.useWorldSpace = true;
		laser.enabled = true;
    }

    void Update()
    {
		actualPositionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		direction = actualPositionMouse - (Vector2)raycastPoint.position;
		direction.Normalize();

		CastLaser(); 
		MoverProyectil(); 

        if (Input.GetKeyDown(KeyCode.Mouse0)) //SHOOT
        {
			hit2D = Physics2D.Raycast(raycastPoint.position, direction);
			if (hit2D)
            {
				//TODO: ACA DEBERIA HACER EL SHOOT BALL. 
				CheckStack(); 
			}
		}

		if (Input.GetKeyDown(KeyCode.Mouse1)) //ABSORB
		{
			hit2D = Physics2D.Raycast(raycastPoint.position, direction);
			if (hit2D)
			{
				canonStack.Absorb(hit2D.collider.GetComponent<Ball>());
				CheckStack();
			}
		}
	}

	void CastLaser()
	{
		laser.SetPosition(0, raycastPoint.position);
		laser.SetPosition(1, actualPositionMouse);
	}

	public void MoverProyectil()
	{
		//ESTO DEBERIA MOVER EL CURRENT PROYECTIL QUE HAY, INDEPENTIENTEMENTE DE QUE TDA SEA
		/*
		if (proyectile != null)
        {
			proyectile.transform.position = shootPoint.position;
			proyectile.transform.rotation = shootPoint.rotation;
		}
		*/
	}

	public void CheckStack() //Esto se haria una vez que se dispara, para chequear si sigue habiendo cosas en stack
	{
		/*
		 * Tiene que poner cual es el Current Projectile
		 * y una vez que dispara tiene que hacer el Dequeue o Pop, segun corresponda
		*/

		if (canonStack.IsEmpty())
		{
			print("Cambio el proyectil a uno de cola");
			//canonQueue.LastBall();
		}
		else
		{
			print("Cambio el proyectil a uno de pila");
			//canonStack.LastBall();
		}
	}

	public void Shoot()
	{
		//Acá deberia disparar el current proyectil que hay
	}
}
