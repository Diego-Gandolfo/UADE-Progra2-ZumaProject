using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonRayCastScript : MonoBehaviour
{
    [SerializeField] private float rayLenght = 5f;
	[SerializeField] private Transform raycastPoint;
	//[SerializeField] private LayerMask layersToHit;

	private Vector2 actualPositionMouse;
    private RaycastHit2D hit2D;
    private LineRenderer laser;
	private Vector2 direction;

	private CanonQueue canonQueue;
	private CanonStack canonStack;

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

		CastLaser(); //Esto dibuja el laser. //TODO: Laser más largo que la posicion del mouse.
		
		if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
			hit2D = Physics2D.Raycast(raycastPoint.position, direction);
			if (hit2D)
            {
				//TODO: ACA DEBERIA HACER EL SHOOT BALL. Y DESDE ACA CONTROLARIA SI ES DESDE EL QUEUE O DESDE EL STACK.

				//Por ahora dejamos esto ya que los chicos hicieron el otro
				canonStack.Absorb(hit2D.collider.GetComponent<Ball>());
			}
		}

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
			hit2D = Physics2D.Raycast(raycastPoint.position, direction);
			if (hit2D)			
				print(hit2D.collider.name);
			//TODO: ACA DEBERIA HACER EL ABSORBER LA PELOTA
		}
	}

	void CastLaser()
	{
		laser.SetPosition(0, raycastPoint.position);
		laser.SetPosition(1, actualPositionMouse);
	}
}
